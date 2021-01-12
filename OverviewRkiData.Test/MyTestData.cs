using System;
using OverviewRkiData.Components.Database;

namespace OverviewRkiData.Test
{
    public class MyTestData : ISQLiteData
    {
        public Int64 Id { get; set; }

        public string MyText { get; set; }
    }
}