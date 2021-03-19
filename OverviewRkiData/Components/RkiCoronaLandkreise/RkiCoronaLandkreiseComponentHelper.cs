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
    }
}