using System;
using System.Collections.Generic;
using System.Linq;
using OverviewRkiData.Components.Data;

namespace OverviewRkiData.Components.RkiCoronaLandkreise
{
    public static class RkiCoronaLandkreiseComponentHelper
    {
        public static void InsertGermanyIfNotList(this IList<Landkreis> landkreise, DateTime dt)
        {
            if(landkreise == null)
            {
                landkreise = new List<Landkreis>();
                CreateEmtpy(landkreise);
                return;
            }


            const string land = "Deutschland";
            
            if (landkreise.Any(a => a.Name.Equals(land)))
            {
                return;
            }
            
            var landkreis = new Landkreis
            {
                Name = land,
                Deaths = landkreise.Sum(a => a.Deaths),
                WeekIncidence = landkreise.Average(a => a.WeekIncidence),
                Date = dt
            };
            
            landkreise.Insert(0, landkreis);
        }

        private static void CreateEmtpy(IList<Landkreis> landkreise)
        {
            var landkreis = new Landkreis
            {
                Name = "Keine Daten",
                Deaths = 0,
                WeekIncidence = 0,
                Date = DateTime.MinValue
            };

            landkreise.Insert(0, landkreis);
        }
    }
}