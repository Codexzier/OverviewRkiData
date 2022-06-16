using Codexzier.Wpf.ApplicationFramework.Controls.Diagram;
using Codexzier.Wpf.ApplicationFramework.Views.Base;
using OverviewRkiData.Views.Data;
using System.Collections.Generic;
using System.Windows.Input;

namespace OverviewRkiData.Views.County
{
    public class CountyViewModel : BaseViewModel
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
        private List<DiagramLevelItem> _countyDeathResults;

        public List<DiagramLevelItem> CountyResults
        {
            get => this._countyResults;
            set
            {
                this._countyResults = value;
                this.OnNotifyPropertyChanged(nameof(this.CountyResults));
            }
        }

        public List<DiagramLevelItem> CountyDeathResults
        {
            get => this._countyDeathResults;
            set
            {
                this._countyDeathResults = value;
                this.OnNotifyPropertyChanged(nameof(this.CountyDeathResults));
            }
        }


        private ICommand _commandCreatePicture;
        private string _trend;
        private string _weekTrend;
        private bool _showBarsFromRightToLeft;
        private bool _showAnimationOn;
        private double _scaledDiagram = 1d;

        public ICommand CommandCreatePicture
        {
            get => this._commandCreatePicture;
            set
            {
                this._commandCreatePicture = value;
                this.OnNotifyPropertyChanged(nameof(this.CommandCreatePicture));
            }
        }

        public string Trend
        {
            get => this._trend;
            set
            {
                this._trend = value;
                this.OnNotifyPropertyChanged(nameof(this.Trend));
            }
        }

        public string WeekTrend
        {
            get => this._weekTrend;
            set
            {
                this._weekTrend = value;
                this.OnNotifyPropertyChanged(nameof(this.WeekTrend));
            }
        }

        public bool ShowBarsFromRightToLeft
        {
            get => this._showBarsFromRightToLeft;
            set
            {
                this._showBarsFromRightToLeft = value;
                this.OnNotifyPropertyChanged(nameof(this.ShowBarsFromRightToLeft));
            }
        }

        public bool ShowAnimationOn
        {
            get => this._showAnimationOn;
            set
            {
                this._showAnimationOn = value;
                this.OnNotifyPropertyChanged(nameof(this.ShowAnimationOn));
            }
        }

        public bool OnlyShowLast200Values { get; set; }

        public double ScaledDiagram
        {
            get => this._scaledDiagram;
            set
            {
                this._scaledDiagram = value;
                this.OnNotifyPropertyChanged(nameof(this.ScaledDiagram));
            }
        }
    }
}