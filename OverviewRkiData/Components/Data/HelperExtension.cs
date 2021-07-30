using OverviewRkiData.Components.RkiCoronaLandkreise;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace OverviewRkiData.Components.Data
{
    public static class HelperExtension
    {
        public static string DataFolderName = "rki-data";
        public static string RkiFilename = "rki-corona-data";
        public static string DatabaseFilename = $"{Environment.CurrentDirectory}/rki-database.db";

        public static string CreateFilename()
        {
            var folder = SubFolderRkiData();
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var date = DateTime.Today;

            return $"{folder}/{RkiFilename}-{date:d}.json";
        }

        public static string SubFolderRkiData() => $"{Environment.CurrentDirectory}/{DataFolderName}";

        public static string GetDate(this string filename)
        {
            var strArray = filename.Split("\\").Last();

            var date = strArray.Substring(RkiFilename.Length + 1);
            date = date.Remove(date.Length - 5);

            return date;
        }

        internal static IEnumerable<Landkreis> GetCountyResults(string name, bool settingFillMissingDataWithDummyValues)
        {
            var minDate = DateTime.MaxValue;
            var maxDate = DateTime.MinValue;
            var list = new List<Landkreis>();
            foreach (var filename in GetFiles())
            {
                var result = RkiCoronaLandkreiseComponent
                    .GetInstance()
                    .LoadFromFile(filename);
                
                result.Districts.InsertGermanyIfNotList(result.Date);
                
                var v = result
                    .Districts
                    .FirstOrDefault(w => w.Name.Equals(name));

                if (v == null)
                {
                    continue;
                }

                v.Date = result.Date;

                if (v.Date > maxDate)
                {
                    maxDate = v.Date;
                }

                if (v.Date < minDate)
                {
                    minDate = v.Date;
                }
                
                list.Add(v);
            }

            if (settingFillMissingDataWithDummyValues)
            {
                list = RenewListWithFillMissingDataWithDummyValues(list, minDate);
            }

            return list.OrderBy(o => o.Date).ToList();
        }

        private static List<Landkreis> RenewListWithFillMissingDataWithDummyValues(List<Landkreis> list, DateTime minDate)
        {
            var renewsList = new List<Landkreis>();
            list = list.OrderBy(o => o.Date).ToList();
            var lastLandkreis = list.First();
            foreach (var landkreis in list)
            {
                Debug.Print($"MinDate: {minDate:d}, landkreis.Date: {landkreis.Date}");
                if (landkreis.Date == minDate)
                {
                    lastLandkreis = landkreis;
                    renewsList.Add(landkreis);
                    minDate = minDate.AddDays(1);
                    continue;
                }

                if (landkreis.Date < minDate)
                {
                    // TODO: Tritt nur dann auf, wenn Daten doppelt vorhanden sind.
                    // Also mit dem gleichen Datum
                    continue;
                }
 
                var nextExistLandkreis = list.First(w => w.Date > minDate);

                var days = (nextExistLandkreis.Date - minDate).Days;
                var valueStepWeekIncidence = (nextExistLandkreis.WeekIncidence - lastLandkreis.WeekIncidence) / (days + 1);
                for (var i = 0; i < days; i++)
                {
                    var dummy = new Landkreis
                    {
                        Cases = lastLandkreis.Cases, 
                        Date = minDate, 
                        Name = "Dummy", 
                        Deaths = lastLandkreis.Deaths,
                        WeekIncidence = lastLandkreis.WeekIncidence + valueStepWeekIncidence + valueStepWeekIncidence * i
                    };
                    minDate = minDate.AddDays(1);
                    renewsList.Add(dummy);
                    Debug.Print($"MinDate: {minDate:d}");
                }
                
                lastLandkreis = landkreis;
                renewsList.Add(landkreis);
                minDate = minDate.AddDays(1);
            }

            return renewsList;
        }

        internal static string RemoveTimeFromLastUpdateString(this string lastUpdate) => lastUpdate.Split(',')[0];

        internal static IEnumerable<string> GetFiles()
        {
            return Directory
                .GetFiles(SubFolderRkiData())
                .Where(w => w.EndsWith(".json") &&
                            w.Contains(HelperExtension.RkiFilename));
        }
    }
}
