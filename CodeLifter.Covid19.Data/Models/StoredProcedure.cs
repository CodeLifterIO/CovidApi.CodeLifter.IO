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
                context.StoredProcedures.FromSqlRaw("EXEC SP_Backup_Database;");
            }
        }
    }
}
