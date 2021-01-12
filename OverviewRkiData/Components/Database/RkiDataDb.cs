using System;

namespace OverviewRkiData.Components.Database
{
    public class RkiDataDb
    {
        //[AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        public DateTime StateTime { get; set; }

        public int[] DataValuesDbIds { get; set; }
    }
}