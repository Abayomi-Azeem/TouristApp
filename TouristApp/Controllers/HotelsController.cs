using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using TouristApp.Models.DBModel;
using TouristApp.Models.ViewModel;

namespace TouristApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly TouristAppDbContext _context;

        public HotelsController(TouristAppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("addhotel")]
        public IActionResult AddHotel([FromForm] AddHotel model)
        {
            byte[] imageDetails;
            
            using (var stream = new MemoryStream())
            {
                 model.Image.CopyTo(stream);
                 imageDetails = stream.ToArray();
            }
            var hotel = new Hotel()
            {
                Name = model.Name,
                Address = model.Address,
                Description = model.Description,
                Picture = imageDetails
            };
            _context.Hotels.Add(hotel);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("viewhotel")]
        public IActionResult ViewHotel(int hotelid)
        {
            var hotel = _context.Hotels.FirstOrDefault(x => x.Id == hotelid);
            var responseHotel = new ViewHotel()
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Address = hotel.Address,
                Comments = hotel.Comments,
                Description = hotel.Description,
                Image = Convert.ToBase64String(hotel.Picture)
            };
            return Ok(responseHotel);
        }

        //listhotels
        [HttpGet]
        [Route("listhotels")]
        public IActionResult ListHotels()
        {
            var hotels = _context.Hotels.ToList();
            return Ok(hotels);
        }


        [HttpPost]
        [Route("addcomment")]
        public IActionResult AddComments([FromBody] AddComment model)
        {

            var hotel = _context.Hotels.FirstOrDefault(x => x.Id == model.HotelId);
            if (hotel.Comments == null)
            {
                hotel.Comments = "";
            }
            var comments = JsonConvert.DeserializeObject<List<string>>(hotel.Comments) ?? new List<string>();
            comments.Add(model.Comment.Trim());
            hotel.Comments = JsonConvert.SerializeObject(comments);
            _context.Update(hotel);
            _context.SaveChanges();
            return Ok("Comment Added Succesfully");
        }

        [HttpGet]
        [Route("removecomment")]
        public IActionResult RemoveComment(int userId, int hotelId, int commentIndex)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user.Role.ToLower() == "user")
            {
                return Unauthorized("Unauthroised Action");
            }

            var hotel = _context.Hotels.FirstOrDefault(x => x.Id == hotelId);
            var comments = JsonConvert.DeserializeObject<List<string>>(hotel.Comments);
            comments.Remove(comments[commentIndex]);
            hotel.Comments = JsonConvert.SerializeObject(comments);
            _context.Hotels.Update(hotel);
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("listusers")]
        public IActionResult ListUsers(int userId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user.Role.ToLower() == "user")
            {
                return Unauthorized("Unauthorized Action");
            }
            var users = _context.Users.Where(x=> x.Role.ToLower() != "admin").ToList();
            return Ok(users);
        }

        [HttpDelete]
        [Route("deleteuser")]
        public IActionResult DeleteUser(int userId, int deletedUser)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user.Role.ToLower() == "user")
            {
                return Unauthorized("Unauthorized Action");
            }
            var userToBeRemoved = _context.Users.Find(deletedUser);
            _context.Users.Remove(userToBeRemoved);
            _context.SaveChanges();
            return Ok();
        }
    }
}
