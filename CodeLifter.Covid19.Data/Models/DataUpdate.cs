using CodeLifter.Covid19.Data.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CodeLifter.Covid19.Data.Models
{
    public class DataUpdate : Entity
    {
        public int RecordsProcessed { get; set; } = 0;  
        public string StartFileName { get; set; }
        public string LastCompletedFileName { get; set; }
        public bool Completed { get; set; } = false;

        public DateTime CompletedAt { get; set; }

        public void Update(string lastCompletedFile)
        {
            LastCompletedFileName = lastCompletedFile;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Complete()
        {
            UpdatedAt = DateTime.UtcNow;
            CompletedAt = DateTime.UtcNow;
            Completed = true;
            Update(this);
        }

        public static void Add(DataUpdate report)
        {
            using (var context = new CovidContext())
            {
                Add(report, context);
            }
        }

        public static void Add(DataUpdate report, CovidContext context)
        {
            context.DataUpdates.Add(report);
            context.SaveChanges();
        }

        public static void Update(DataUpdate report)
        {
            using (var context = new CovidContext())
            {
                Update(report, context);
            }
        }

        public static void Update(DataUpdate report, CovidContext context)
        {
            context.DataUpdates.Update(report);
            context.SaveChanges();
        }
    }
}
