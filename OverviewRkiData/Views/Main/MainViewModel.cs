using OverviewRkiData.Views.Base;
using OverviewRkiData.Views.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace OverviewRkiData.Views.Main
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            var list = new List<DistrictItem>
            {
                new DistrictItem { Name = "Keine Daten...", Deaths = 0, WeekIncidence = 0 },
            };

            this.Districts = new ObservableCollection<DistrictItem>(list);
        }

        private ObservableCollection<DistrictItem> _districts;
        private DistrictItem _selected;
        private string _searchCounty;
        private ICommand _commandSelectedDistrict;
        private string _actualDataFromDate;
        private ICommand _commandSortByWeekIncidence;
        private ICommand _commandSortByDeaths;

        public ObservableCollection<DistrictItem> Districts
        {
            get => this._districts;
            set
            {
                this._districts = value;
                this.OnNotifyPropertyChanged(nameof(this.Districts));
            }
        }

        public DistrictItem Selected
        {
            get => this._selected;
            set
            {
                if (this._selected == null || !this._selected.Equals(value))
                {
                    this.CommandSelectedDistrict?.Execute(value);
                }
                this._selected = value;
                this.OnNotifyPropertyChanged(nameof(this.Selected));
            }
        }

        public string SearchCounty
        {
            get => this._searchCounty;
            set
            {
                this._searchCounty = value;
                this.OnNotifyPropertyChanged(nameof(this.SearchCounty));
            }
        }

        public ICommand CommandSelectedDistrict
        {
            get => this._commandSelectedDistrict;
            set
            {
                this._commandSelectedDistrict = value;
                this.OnNotifyPropertyChanged(nameof(this.CommandSelectedDistrict));
            }
        }

        public string ActualDataFromDate
        {
            get => this._actualDataFromDate;
            set
            {
                this._actualDataFromDate = value;
                this.OnNotifyPropertyChanged(nameof(this.ActualDataFromDate));
            }
        }

        public ICommand CommandSortByWeekIncidence
        {
            get => this._commandSortByWeekIncidence;
            set
            {
                this._commandSortByWeekIncidence = value;
                this.OnNotifyPropertyChanged(nameof(this.CommandSortByWeekIncidence));
            }
        }

        public ICommand CommandSortByDeaths
        {
            get => this._commandSortByDeaths;
            set
            {
                this._commandSortByDeaths = value;
                this.OnNotifyPropertyChanged(nameof(this.CommandSortByDeaths));
            }
        }
    }
}
