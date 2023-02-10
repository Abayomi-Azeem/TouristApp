namespace TouristApp.Models.ViewModel
{
    public class AddHotel
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public IFormFile Image { get; set; }
    }
}
