using System.ComponentModel.DataAnnotations.Schema;

namespace CodeLifter.Covid19.Data.Models
{
    public class Totals : Entity
    {
        public int Count { get; set; }
        public int Confirmed { get; set; }
        public int Deaths { get; set; }
        public int Active { get; set; }
        public int Recovered { get; set; }
        public string SourceFile { get; set; }
        public int Total {
            get
            {
                return Confirmed + Deaths + Active + Recovered;
            }
        }

        public int? CountryId { get; set; }
        public Country Country { get; set; }

        public int? ProvinceId { get; set; }
        public Province Province { get; set; }

        public int? DistrictId { get; set; }
        public District District { get; set; }
    }
}
