using System;
using OverviewRkiData.Commands;
using OverviewRkiData.Components.RkiCoronaLandkreise;
using OverviewRkiData.Components.Ui.EventBus;
using OverviewRkiData.Views.Base;
using OverviewRkiData.Views.Data;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OverviewRkiData.Views.Main
{
    public partial class MainView : UserControl
    {
        private readonly MainViewModel _viewModel;

        public MainView()
        {
            this.InitializeComponent();

            this._viewModel = (MainViewModel)this.DataContext;

            EventBusManager.Register<MainView, BaseMessage>(this.BaseMessageEvent);
            this._viewModel.CommandSelectedDistrict = new ChangedCommandSelectedDistrict(this._viewModel);
            this._viewModel.CommandSortByWeekIncidence = new ButtonCommandSortByWeekIncidence(this._viewModel);
            this._viewModel.CommandSortByDeaths = new ButtonCommandSortByDeaths(this._viewModel);
        }

        private async void BaseMessageEvent(IMessageContainer arg)
        {
            SimpleStatusOverlays.ActivityOn();

            await Task.Run(() =>
            {
                if (this.CheckOptionAndLoadLandkreise(arg))
                {
                    return;
                }

                this._viewModel.ActualDataFromDate = StaticDataManager.ActualLoadedDataDate;

                this.Dispatcher.Invoke(() =>
                {
                    this._viewModel.Districts.Clear();
                });

                this._viewModel.CountyCount = 0;
                foreach (var item in StaticDataManager.ActualLoadedData)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        this._viewModel.Districts.Add(item);
                        this._viewModel.CountyCount++;
                    });
                }

                SimpleStatusOverlays.ActivityOff();
            });
        }

        private bool CheckOptionAndLoadLandkreise(IMessageContainer arg)
        {
            if (arg.Content is BaseMessageOptions option && option == BaseMessageOptions.LoadActualData)
            {
                var component = RkiCoronaLandkreiseComponent.GetInstance();
                component.RkiDataErrorEvent += this.Component_RkiDataErrorEvent;

                var landkreise = component.LoadData(out var safeData);

                if (safeData != null)
                {
                    SimpleStatusOverlays.ShowAsk("Question", "Overwrite local data with actual loaded data?", safeData);
                }

                if (landkreise == null)
                {
                    SimpleStatusOverlays.ActivityOff();
                    return true;
                }

                // TODO aktuell wird nicht im jeden Datensatz das Datum hinterlegt.
                var districtItems = landkreise.Districts.Select(s => new DistrictItem
                {
                    Name = s.Name,
                    Deaths = s.Deaths,
                    WeekIncidence = s.WeekIncidence,
                    Date = landkreise.Date
                });

                StaticDataManager.ActualLoadedDataDate = landkreise.Date;
                StaticDataManager.ActualLoadedData = districtItems;
            }

            return false;
        }

        private void Component_RkiDataErrorEvent(string message) => SimpleStatusOverlays.Show("ERROR", message);

        private void TextBoxSearch_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(this._viewModel.SearchCounty))
            {
                this._viewModel.Districts = new ObservableCollection<DistrictItem>(StaticDataManager.ActualLoadedData);
                return;
            }

            var searchResult = StaticDataManager
                .ActualLoadedData
                .Where(w => w.Name.ToLower().Contains(this._viewModel.SearchCounty.ToLower()));

            this._viewModel.Districts = new ObservableCollection<DistrictItem>(searchResult);
        }
    }
}
