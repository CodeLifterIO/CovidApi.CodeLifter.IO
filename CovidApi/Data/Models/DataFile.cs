using CovidApi.Data;
using CovidApi.Models.BaseEntities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace CovidApi.Models
{
    public class DataFile : Entity
    {
        public int RecordsProcessed { get; set; }
        public bool Completed { get; set; }
        public DateTime CompletedAt { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }

        //[NotMapped]
        //public TimeSpan ElapsedTime 
        //{
        //    get
        //    {
        //        return CompletedAt - CreatedAt;
        //    }
        //}

        //public override string ToString()
        //{
        //    DateTime now = DateTime.Now;
        //    string value = $"{RecordsProcessed} records from {FileName} ";
        //    value += (Completed) ? $"Completed in {ElapsedTime.Minutes}:{ElapsedTime.Seconds}.{ElapsedTime.Milliseconds} MMSSmm" : $"Incomplete as of {now.Hour}:{now.Minute}:{now.Second} HHMMSS";
        //    return value;
        //}

        //public void Complete()
        //{
        //    Completed = true;
        //    CompletedAt = DateTime.UtcNow;
        //}

        //public static void Add(DataFile report)
        //{
        //    using (var context = new CovidContext())
        //    {
        //        Add(report, context);
        //    }
        //}

        //public static void Add(DataFile report, CovidContext context)
        //{
        //    context.DataFiles.Add(report);
        //    context.SaveChanges();
        //}

        //public static void Update(DataFile report)
        //{
        //    using (var context = new CovidContext())
        //    {
        //        Update(report, context);
        //    }
        //}

        //public static void Update(DataFile report, CovidContext context)
        //{
        //    report.Complete();
        //    context.DataFiles.Update(report);
        //    context.SaveChanges();
        //}
    }
}
