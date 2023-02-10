using System.ComponentModel.DataAnnotations;

namespace TouristApp.Models.DBModel
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Role { get; set; }
    }
}
