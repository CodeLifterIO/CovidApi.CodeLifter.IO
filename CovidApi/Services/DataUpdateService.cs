using CovidApi.Models;
using CovidApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CovidApi.Services
{
    public interface IDataUpdateService
    {

    }

    public class DataUpdateService : IDataUpdateService
    {
        public bool IsStarted { get; set; } = false;

        public DataUpdate CurrentUpdateState { get; set; } = new DataUpdate();
        public DataFile CurrentFile { get; private set; }

        //public Dictionary<string, Country> Countries { get; set; } = new Dictionary<string, Country>();
        //public int CountriesCount { get { return Countries.Count; } }

        //public Dictionary<string, Province> Provinces { get; set; } = new Dictionary<string, Province>();
        //public int ProvincesCount { get { return Provinces.Count; } }

        //public Dictionary<string, District> Districts { get; set; } = new Dictionary<string, District>();
        //public int DistrictsCount { get { return Districts.Count; } }

        //public Dictionary<string, GeoCoordinate> GeoCoordinates { get; set; } = new Dictionary<string, GeoCoordinate>();
        //public int GeoCoordinatesCount { get { return GeoCoordinates.Count; } }

        //public List<DataPoint> DataPoints { get; set; } = new List<DataPoint>();
        //public int DataPointsCount { get { return DataPoints.Count; } }

        public void StartRun()
        {
            if (IsStarted == true) return;
            IsStarted = true;

            //if (null != gFile) CurrentUpdateState.StartFileName = gFile.FileName;
            //DataUpdate.Add(CurrentUpdateState);
        }
    }
}


//        public void StartFile(GithubDataFile gFile)
//        {
//            CurrentFile = new CovidApi.Models.DataFile();
//            CurrentFile.FileName = gFile.FileName;
//            DataFile.Add(CurrentFile);
//        }

//        public void FinishFile(int recordsProcessed)
//        {
//            // complete last file's report and update file
//            CurrentFile.RecordsProcessed = recordsProcessed;

//            //SaveDataToDb();

//            CurrentFile.CompletedAt = DateTime.UtcNow;
//            DataFile.Update(CurrentFile);
//            CurrentUpdateState.LastCompletedFileName = CurrentFile.FileName;
//            CurrentUpdateState.RecordsProcessed += recordsProcessed;

//            Console.Out.WriteLine(CurrentFile.ToString());

//            CurrentFile = null;
//            DataUpdate.Update(CurrentUpdateState);
//        }

//        public void Finish()
//        {
//            Console.Out.WriteLine(CurrentUpdateState.ToString());

//            SaveDataToDb();
//            CurrentUpdateState.Complete();

//            Console.Out.WriteLine(CurrentUpdateState.ToString());
//        }


//        public void SaveDataToDb()
//        {
//            using (var context = new CovidContext())
//            {
//                SaveDataToDb(context);
//            }
//        }

//        public void SaveDataToDb(CovidContext context)
//        {
//            //GeoCoordinate.UpsertRange(GeoCoordinates.Values.ToList(), context);
//            //GeoCoordinates.Clear();

//            //Country.UpsertRange(Countries.Values.ToList(), context);
//            //Countries.Clear();

//            //Province.UpsertRange(Provinces.Values.ToList(), context);
//            //Provinces.Clear();

//            //District.UpsertRange(Districts.Values.ToList(), context);
//            //Districts.Clear();

//            DataPoint.AddRange(DataPoints, context);  //uses UpsertRange on later runs for integrity reasons
//            DataPoints.Clear();

//            context.SaveChanges();
//        }
//    }
//}
