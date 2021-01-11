using OverviewRkiData.Controls.Diagram;
using OverviewRkiData.Views.Base;
using OverviewRkiData.Views.Data;
using System.Collections.Generic;

namespace OverviewRkiData.Views.County
{
    internal class CountyViewModel : BaseViewModel
    {
        private DistrictItem _districtData;

        public DistrictItem DistrictData
        {
            get => this._districtData;
            set
            {
                this._districtData = value;
                this.OnNotifyPropertyChanged(nameof(this.DistrictData));
            }
        }

        private List<DiagramLevelItem> _countyResults;

        public List<DiagramLevelItem> CountyResults
        {
            get => this._countyResults;
            set
            {
                this._countyResults = value;
                this.OnNotifyPropertyChanged(nameof(this.CountyResults));
            }
        }
    }
}