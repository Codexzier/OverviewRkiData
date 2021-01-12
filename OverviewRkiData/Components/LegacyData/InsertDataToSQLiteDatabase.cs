using OverviewRkiData.Components.Data;
using OverviewRkiData.Components.RkiCoronaLandkreise;
using OverviewRkiData.Components.RkiDataToSQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace OverviewRkiData.Components.LegacyData
{
    public class InsertDataToSQLiteDatabase
    {
        private RkiDatabaseConnector _databaseConnector;

        public InsertDataToSQLiteDatabase()
        {
            this._databaseConnector = new RkiDatabaseConnector();

        }

        //public void Import()
        //{
        //    this._databaseConnector.Init();

        //    var subFolder = $"{Environment.CurrentDirectory}/{HelperExtension.DataFolderName}";
        //    var files = Directory.GetFiles(subFolder)
        //       .Where(w => w.Contains(HelperExtension.RkiFilename))
        //       .ToArray();

        //    if (!files.Any())
        //    {
        //        return;
        //    }

        //    var allExistDate = this._databaseConnector
        //        .LoadAll()
        //        .Select(s => s.StateTime)
        //        .ToArray();

        //    var landkreise = this._databaseConnector
        //        .AllLandkreise()
        //        .Select(s => s)
        //        .ToArray();

        //    foreach (var file in files)
        //    {
        //        var result = RkiCoronaLandkreiseComponent
        //            .GetInstance()
        //            .LoadFromFile(file);

        //        if (allExistDate.Any(a => a.Equals(result.Date)))
        //        {
        //            continue;
        //        }

        //        var dataValuesIds = new List<int>();
        //        foreach (var item in result.Districts)
        //        {
        //            var landkreis = landkreise.FirstOrDefault(f => f.Name.Equals(item.Name));

        //            if (landkreis == null)
        //            {
        //                continue;
        //            }

        //            var id = this._databaseConnector.Add(new DataValuesDb
        //            {
        //                LandkreisDbId = landkreis.Id,
        //                WeekIncidence = item.WeekIncidence,
        //                Deaths = item.Deaths,
        //                Cases = item.Cases,
        //                Date = item.Date
        //            });
        //        }

        //        var rkiDataDb = new RkiDataDb
        //        {
        //            StateTime = result.Date,
        //            DataValuesDbIds = dataValuesIds.ToArray()
        //        };
        //        this._databaseConnector.Add(rkiDataDb);
        //    }


        //    var data = this._databaseConnector.LoadAll();
        //    Debug.WriteLine($"Rki Data count: {data.Count()}");
        //}
    }
}
