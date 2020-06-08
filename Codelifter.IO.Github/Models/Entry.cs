using System;
namespace CodeLifter.IO.Github.Models
{
    public class Entry
    {
        public string FIPS { get; set; }
        public string Admin2 { get; set; }
        public string ProvinceState { get; set; }
        public string CountryRegion { get; set; }
        public DateTime LastUpdate { get; set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
        public int? Confirmed { get; set; }
        public int? Deaths { get; set; }
        public int? Recovered { get; set; }
        public int? Active { get; set; }
        public string CombinedKey { get; set; }
        public string SourceFile { get; set; }

        //easy printing
        public override string ToString()
        {
            return $"{LastUpdate} - {CombinedKey}";
        }
    }
}
