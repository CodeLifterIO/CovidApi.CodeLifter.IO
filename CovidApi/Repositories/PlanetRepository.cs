using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidApi.Models;

namespace CovidApi.Repositories
{
    public interface IPlanetRepository
    {
        Planet Find();
    }

    public class PlanetRepository : IPlanetRepository
    {
        public Planet Find()
        {
            return new Planet();
        }
    }
}
