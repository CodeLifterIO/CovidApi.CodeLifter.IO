using CovidApi.Data;
using CovidApi.Models.BaseEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CovidApi.Models  
{
    public class GeoCoordinate : BaseEntity
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
