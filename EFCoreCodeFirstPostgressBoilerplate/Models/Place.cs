using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFCoreCodeFirstPostgressBoilerplate.Models
{
    public class Place
    {
        [Key]
        public long Id { get; set; }

        [Required, StringLength(300)]
        public string Name { get; set; }

        public ICollection<WeatherForecast> WeatherForecasts { get; set; }
    }
}
