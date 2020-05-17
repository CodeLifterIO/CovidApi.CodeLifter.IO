using System.ComponentModel.DataAnnotations.Schema;
using CodeLifter.Covid19.Data.Interfaces;
using Newtonsoft.Json;

namespace CodeLifter.Covid19.Data.Models
{
    [NotMapped]
    public class Planet : INamedEntity
    {
        [JsonIgnore]
        public int Id { get; private set; } = 3;
        public string Name { get; set; } = "Earth";
        public string Slug { get; set; } = "earth";
        public long Population { get; set; } = 7594000000;

        [NotMapped]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Statistic CurrentData { get; set; }

        [NotMapped]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object TimeSeries { get; set; }

        [NotMapped]
        public string CountriesUrl { get { return $"countries"; } }

        [NotMapped]
        public string TimeSeriesUrl { get { return $"global/timeseries"; } }
    }
}
