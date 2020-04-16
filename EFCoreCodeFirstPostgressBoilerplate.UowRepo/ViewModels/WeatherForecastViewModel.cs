using System.ComponentModel.DataAnnotations;

namespace EFCoreCodeFirstPostgressBoilerplate.UowRepo.ViewModels
{
    public class WeatherForecastViewModel
    {
        public long Id { get; set; }

        // For further read-up on validation see:
        // https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-3.1

        [Required]
        public string Date { get; set; }

        [Required]
        public int TemperatureC { get; set; }
        public string Summary { get; set; }

        [Required]
        public long PlaceId { get; set; }
    }
}
