using System;
using System.ComponentModel.DataAnnotations.Schema;
using CodeLifter.Covid19.Data.Models;
using Newtonsoft.Json;

namespace CodeLifter.Covid19.Data.Interfaces
{
    public interface IStatisticalEntity
    {
        [NotMapped]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Statistic CurrentData { get; set; }

        [NotMapped]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object TimeSeries { get; set; }
    }
}
