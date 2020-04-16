using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EFCoreCodeFirstPostgressBoilerplate.UowRepo.Repository;

namespace EFCoreCodeFirstPostgressBoilerplate.UowRepo.Models
{
    public class Place : IEntity
    {
        [Key]
        public long Id { get; set; }

        [Required, StringLength(300)]
        public string Name { get; set; }

        public ICollection<WeatherForecast> WeatherForecasts { get; set; }
    }
}
