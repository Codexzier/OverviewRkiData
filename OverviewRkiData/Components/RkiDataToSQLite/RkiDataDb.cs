using System;

namespace OverviewRkiData.Components.RkiDataToSQLite
{
    public class RkiDataDb
    {
        //[AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        public DateTime StateTime { get; set; }

        public int[] DataValuesDbIds { get; set; }
    }
}