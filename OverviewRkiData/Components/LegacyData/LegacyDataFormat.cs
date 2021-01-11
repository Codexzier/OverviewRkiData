using System.Collections.Generic;

namespace OverviewRkiData.Components.LegacyData
{
    public class LegacyDataFormat
    {
        public string lastUpdate { get; set; }
        public IList<LegacyDistrictItem> districts { get; set; }
    }
}