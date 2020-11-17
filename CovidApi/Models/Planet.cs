using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace CovidApi.Models
{
    [NotMapped]
    public class Planet
    {
        public string Name { get; set; } = "Earth";
        public string PlanetId { get; set; } = "earth";
        public long Population { get; set; } = 7594000000;

        [NotMapped]
        public Total CurrentData { get; set; }

        [NotMapped]
        public List<Total> TimeSeries { get; set; }

        [NotMapped]
        public string CountriesUrl { get { return $"countries"; } }
    }
}
