namespace OverviewRkiData.Components.RkiCoronaLandkreise
{
    public class RkiCoronaLandkreiseResult
    {
        // ReSharper disable once InconsistentNaming
        public Landkreis[] features { get; set; }

        // ReSharper disable once IdentifierTypo
        public class Landkreis
        {
            // ReSharper disable once InconsistentNaming
            public Attribute attributes { get; set; }

            public class Attribute
            {
                // ReSharper disable once InconsistentNaming
                public string GEN { get; set; }

                // ReSharper disable once InconsistentNaming
                public double cases7_per_100k { get; set; }

                // ReSharper disable once InconsistentNaming
                public int deaths { get; set; }

                // ReSharper disable once InconsistentNaming
                public int cases { get; set; }

                // ReSharper disable once InconsistentNaming
                public string last_update { get; set; }
            }
        }
    }
}