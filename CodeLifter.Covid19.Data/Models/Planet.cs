using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace CodeLifter.Covid19.Data.Models
{
    [NotMapped]
    public class Planet
    {
        [JsonIgnore]
        public int Id { get; private set; } = 3;
        public string Name { get; set; } = "Earth";
        public string Slug { get; set; } = "earth";
        public long Population { get; set; } = 7594000000;

        [NotMapped]
        public Statistic CurrentData { get; set; }

        [NotMapped]
        public List<Statistic> TimeSeries { get; set; }

        [NotMapped]
        public string CountriesUrl { get { return $"countries"; } }
    }
}
