using OverviewRkiData.Views.Data;
using System;
using System.Collections.Generic;

namespace OverviewRkiData.Views.Main
{
    public static class StaticDataManager
    {
        public static IEnumerable<DistrictItem> ActualLoadedData { get; internal set; }
        public static DateTime ActualLoadedDataDate { get; internal set; }
    }
}
