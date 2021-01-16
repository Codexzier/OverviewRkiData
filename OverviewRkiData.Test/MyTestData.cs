using System;
using OverviewRkiData.Components.Database;

namespace OverviewRkiData.Test
{
    public class MyTestData : ISQLiteData
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        public string MyText { get; set; }
    }
}