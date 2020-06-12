using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeLifter.Covid19.Data.Models
{
    public class Totals : Entity
    {
        [DefaultValue(0)]
        public int? Count { get; set; }
        [DefaultValue(0)]
        public int? Confirmed { get; set; }
        [DefaultValue(0)]
        public int? Deaths { get; set; }
        [DefaultValue(0)]
        public int? Active { get; set; }
        [DefaultValue(0)]
        public int? Recovered { get; set; }
        public string SourceFile { get; set; }

        public int? CountryId { get; set; }
        public Country Country { get; set; }

        public int? ProvinceId { get; set; }
        public Province Province { get; set; }

        public int? DistrictId { get; set; }
        public District District { get; set; }
    }
}
