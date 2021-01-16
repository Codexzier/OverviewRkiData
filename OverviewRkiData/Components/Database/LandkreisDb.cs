using System;
using OverviewRkiData.Components.RkiCoronaLandkreise;

namespace OverviewRkiData.Components.Database
{
    public class LandkreisDb
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class PrimaryKeyAttribute : Attribute { }
    public class AutoIncrementAttribute : Attribute { }
}