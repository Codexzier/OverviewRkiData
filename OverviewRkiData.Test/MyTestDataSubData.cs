using OverviewRkiData.Components.Database;

namespace OverviewRkiData.Test
{
    public class MyTestDataSubData
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        public int[] CollectionOfInteger { get; set; }
    }
}