using OverviewRkiData.Components.Data;
using OverviewRkiData.Components.RkiCoronaLandkreise;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using OverviewRkiData.Components.Database;

namespace OverviewRkiData.Components.LegacyData
{
    public class InsertDataToSQLiteDatabase
    {
        private readonly string _subFolder;
        private readonly RkiDatabaseConnector _databaseConnector;

        public InsertDataToSQLiteDatabase(string databaseFilename)
        : this(databaseFilename, $"{Environment.CurrentDirectory}/{HelperExtension.DataFolderName}")
        {
        }

        public InsertDataToSQLiteDatabase(string databaseFilename, string subFolder)
        {
            this._subFolder = subFolder;
            this._databaseConnector = new RkiDatabaseConnector(databaseFilename);
        }

        private static readonly Type[] _tables =
        {
            typeof(RkiDataDb), 
            typeof(DataValuesDb), 
            typeof(LandkreisDb)
        };

        // TODO: Wird noch umgebaut
        public void Import()
        {
            var files = Directory.GetFiles(this._subFolder)
                .Where(w => w.Contains(HelperExtension.RkiFilename))
                .ToArray();

            if (!files.Any())
            {
                return;
            }

            if (!ExistTables(this._databaseConnector))
            {
                this._databaseConnector.Create<RkiDataDb>();
            }

            var allExistDate = this._databaseConnector
                .Select<RkiDataDb>()
                .Select(s => s.StateTime)
                .ToArray();

            var landkreise = this._databaseConnector
                .Select<LandkreisDb>()
                .ToArray();

            foreach (var file in files)
            {
                var result = RkiCoronaLandkreiseComponent
                    .GetInstance()
                    .LoadFromFile(file);

                if (allExistDate.Any(a => a.Equals(result.Date)))
                {
                    continue;
                }

                var dataValuesIds = new List<int>();
                foreach (var item in result.Districts)
                {
                    var landkreis = landkreise.FirstOrDefault(f => f.Name.Equals(item.Name));

                    if (landkreis == null)
                    {
                        continue;
                    }

                    var id = this._databaseConnector.Insert(new DataValuesDb
                    {
                        LandkreisDbId = landkreis.Id,
                        WeekIncidence = item.WeekIncidence,
                        Deaths = item.Deaths,
                        Cases = item.Cases,
                        Date = item.Date
                    });
                }

                var rkiDataDb = new RkiDataDb
                {
                    StateTime = result.Date,
                    DataValuesDbIds = dataValuesIds.ToArray()
                };
                this._databaseConnector.Insert(rkiDataDb);
            }

            var data = this._databaseConnector.Select<DataValuesDb>();
            Debug.WriteLine($"Rki data value count: {data.Count()}");

            static bool ExistTables(RkiDatabaseConnector dbConnector) =>
                _tables.All(dbConnector.Exist);

            static bool CreateTables(RkiDatabaseConnector dbConnector) =>
                _tables.All(a => dbConnector.Create(a));
        }
    }
}
