using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace CodeLifter.Covid19.Data.Models
{
    [NotMapped]
    public class StoredProcedure
    {
        public static void GenerateDatabaseBackup()
        {
            using (var context = new CovidContext())
            {
                //int result = context.Database.ExecuteSqlRaw("EXEC SP_Backup_Database;");
                //context.StoredProcedures.FromSqlRaw("EXEC SP_Backup_Database;").FirstOrDefaultAsync();
            }
        }

        public static void SummarizeEntities()
        {
            SummarizeCountries();
            SummarizeProvinces();
            SummarizeDistricts();
        }


        public static void SummarizeCountries()
        {
            using (var context = new CovidContext())
            {
                //int result = context.Database.ExecuteSqlRaw($"EXEC SP_Update_Summary_On_Country;");
            }
        }

        public static void SummarizeProvinces()
        {
            using (var context = new CovidContext())
            {
                //int result = context.Database.ExecuteSqlRaw($"EXEC SP_Update_Summary_On_Province;");
            }
        }

        public static void SummarizeDistricts()
        {
            using (var context = new CovidContext())
            {
                //int result = context.Database.ExecuteSqlRaw($"EXEC SP_Update_Summary_On_District;");
            }
        }
    }
}
