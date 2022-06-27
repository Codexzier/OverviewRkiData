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

        internal static IEnumerable<Landkreis> GetCountyResults(
            string name, 
            bool settingFillMissingDataWithDummyValues,
            int getLastDays)
        {
            var minDate = DateTime.MaxValue;
            var maxDate = DateTime.MinValue;
            var list = new List<Landkreis>();
            foreach (var filename in GetFiles(getLastDays))
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

        /// <summary>
        /// Get all files with save data about the rki data.
        /// </summary>
        /// <param name="getLastDays">Set 0 for all files. How far in the past should data be retrieved?</param>
        /// <returns></returns>
        internal static IEnumerable<string> GetFiles(int getLastDays)
        {
            if (getLastDays < 0)
            {
                getLastDays *= -1;
            }

            var listDate = new List<string>();
            var date = DateTime.Today.AddDays(getLastDays * -1);
            for (int i = 0; i < getLastDays + 1; i++)
            {
                listDate.Add($"-{date:d}");
                date = date.AddDays(1);
            }
            
            
            return Directory
                .GetFiles(SubFolderRkiData())
                .Where(filename =>
                {
                    var resultIsJson = filename.EndsWith(".json");
                    var resultIsRkiFile = filename.Contains(RkiFilename);

                    var resultGetLastDays = true;
                    if (getLastDays != 0)
                    {
                        resultGetLastDays = listDate.Any(filename.Contains);
                    }
                    
                    return resultIsJson && resultIsRkiFile && resultGetLastDays;
                });
        }
    }
}
