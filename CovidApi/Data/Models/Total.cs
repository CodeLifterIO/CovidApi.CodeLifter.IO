using CovidApi.Models.BaseEntities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CovidApi.Models
{
    public class Total : BaseEntity
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

        public string CountrySlugId { get; set; }
        //public Country Country { get; set; }

        public string ProvinceSlugId { get; set; }
        //public Province Province { get; set; }

        public string DistrictSlugId { get; set; }
        //public District District { get; set; }
    }
}
