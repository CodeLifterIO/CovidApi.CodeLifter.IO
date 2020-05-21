
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CodeLifter.Covid19.Data.Models;

namespace CodeLifter.Covid19.Data.Interfaces
{
    public interface IStatisticalEntity
    {
        [NotMapped]
        public Statistic CurrentData { get; set; }

        [NotMapped]
        public List<Statistic> TimeSeries { get; set; }
    }
}
