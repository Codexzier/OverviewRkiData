using System;
using System.Collections.Generic;

namespace OverviewRkiData.Components.Data
{
    public class Landkreise
    {
        public DateTime Date { get; set; }

        public IList<Landkreis> Districts { get; set; }
    }
}
