using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TouristApp.Models.DBModel;
using TouristApp.Models.ViewModel;

namespace TouristApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly string salt = "HotelAppHashSalt";
        private readonly TouristAppDbContext _context;

        public AuthenticationController(TouristAppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(Login model)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Email.ToLower() == model.Email);
                if (user == null)
                {
                    return NotFound("User Not Found");
                }
                if (user.PasswordHash == HashPassword(model.Password))
                {
                    return Ok(new {Role = user.Role, Id = user.Id});
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest($"An Error occurred during User Login ----- {ex.Message}");
            }
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(Register model)
        {
            try
            {
                var findUser = _context.Users.FirstOrDefault(x => x.Email.ToLower() == model.Email);
                if (findUser != null)
                {
                    return BadRequest("User already Exists");
                }

                var user = new User
                {
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Email = model.Email,
                    Role = "User",
                    PasswordHash = HashPassword(model.Password)
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                return Ok("User Created Successfully");

            }
            catch (Exception ex)
            {
                return BadRequest($"Error Creating User----- {ex.Message}");           
            }

        }

        private string HashPassword(string password)
        {
            using (var hasher = SHA256.Create())
            {
                var passwordHashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                string passwordHash = Convert.ToBase64String(passwordHashByte);
                return passwordHash;
            }
        }
    }
}
