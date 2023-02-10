using System.ComponentModel.DataAnnotations;

namespace TouristApp.Models.DBModel
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address {get; set;}

        public string Description { get; set; }

        public string? Comments { get; set; }

        public byte[] Picture { get; set; }
    }
}
