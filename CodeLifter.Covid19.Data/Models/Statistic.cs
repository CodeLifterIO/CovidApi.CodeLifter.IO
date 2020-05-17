using System.ComponentModel.DataAnnotations.Schema;

namespace CodeLifter.Covid19.Data.Models
{
    [NotMapped]
    public class Statistic
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
    }
}
