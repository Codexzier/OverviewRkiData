using System;

namespace OverviewRkiData.Components.Data
{
    public class Landkreis
    {
        public string Name { get; set; }

        public double WeekIncidence { get; set; }

        public int Deaths { get; set; }

        public int Cases { get; set; }

        public DateTime Date { get; internal set; }
    }
}
