namespace OverviewRkiData.Components.LegacyData
{
    public class LegacyDistrictItem
    {
        public string name { get; set; }
        public string county { get; set; }
        public int count { get; set; }
        public int deaths { get; set; }
        public double weekIncidence { get; set; }
        public double casesPer100k { get; set; }
        public double casesPerPopulation { get; set; }
        public string Date { get; internal set; }
    }
}