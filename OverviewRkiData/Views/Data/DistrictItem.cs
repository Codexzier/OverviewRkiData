using System;

namespace OverviewRkiData.Views.Data
{
    public class DistrictItem
    {
        public string Name { get; set; }

        public double WeekIncidence { get; set; }

        public int Deaths { get; set; }

        public bool Selected { get; set; }
        public DateTime Date { get; internal set; }
    }
}