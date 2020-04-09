using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFCoreCodeFirstPostgressBoilerplate.Models;

namespace EFCoreCodeFirstPostgressBoilerplate
{
    public class WeatherForecast
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int TemperatureC { get; set; }
        public string Summary { get; set; }

        [NotMapped]
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public long PlaceId { get; set; }
        [ForeignKey("PlaceId")]
        public Place Place { get; set; }
    }
}
