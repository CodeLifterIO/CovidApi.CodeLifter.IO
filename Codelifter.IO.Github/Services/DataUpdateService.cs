using CodeLifter.Covid19.Data;
using CodeLifter.Covid19.Data.Migrations;
using CodeLifter.Covid19.Data.Models;
using CodeLifter.IO.Github.Models;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace Codelifter.IO.Github.Services
{
    public class DataUpdateService
    {
        public bool IsStarted { get; set; } = false;

        public DataUpdate CurrentUpdateState { get; set; } = new DataUpdate();
        public DataFile CurrentFile { get; private set; }


        //public List<DataFile> DataFiles => new List<DataFile>();
        //public int DataFilesCount { get { return DataFiles.Count; } }

        //public Dictionary<string, Planet> Planets => new Dictionary<string,Planet>();
        //public int PlanetsCount { get { return Planets.Count; } }

        public Dictionary<string, Country> Countries => new Dictionary<string, Country>();
        public int CountriesCount { get { return Countries.Count; } }

        public Dictionary<string, Province> Provinces => new Dictionary<string, Province>();
        public int ProvincesCount { get { return Provinces.Count; } }

        public Dictionary<string, District> Districts => new Dictionary<string, District>();
        public int DistrictsCount { get { return Districts.Count; } }

        public List<GeoCoordinate> GeoCoordinates => new List<GeoCoordinate>();
        public int GeoCoordinatesCount { get { return GeoCoordinates.Count; } }

        public List<DataPoint> DataPoints => new List<DataPoint>();
        public int DataPointsCount{ get { return DataPoints.Count; } }

        public void StartRun(GithubDataFile gFile)
        {
            if (IsStarted == true) return;

            IsStarted = true;
            if (null != gFile) CurrentUpdateState.StartFileName = gFile.FileName;
            DataUpdate.Add(CurrentUpdateState);
        }

        public void StartFile(GithubDataFile gFile)
        {
            CurrentFile = new CodeLifter.Covid19.Data.Models.DataFile();
            CurrentFile.FileName = gFile.FileName;
            DataFile.Add(CurrentFile);
        }

        public void FinishFile(int recordsProcessed)
        {
            // complete last file's report and update file
            CurrentFile.RecordsProcessed = recordsProcessed;
            CurrentFile.CompletedAt = DateTime.UtcNow;
            DataFile.Update(CurrentFile);
            CurrentUpdateState.LastCompletedFileName = CurrentFile.FileName;
            CurrentUpdateState.RecordsProcessed += recordsProcessed;

            Console.Out.WriteLine(CurrentFile.ToString());

            CurrentFile = null;
            DataUpdate.Update(CurrentUpdateState);
        }

        public void Finish()
        {
            // close last file
            CurrentUpdateState.Complete();
            //finalize global run data
        }


        public void SaveDataToDb()
        {
            using (var context = new CovidContext())
            {

            }
        }

        public void SaveDataToDb(CovidContext context)
        {

        }

    }
}
