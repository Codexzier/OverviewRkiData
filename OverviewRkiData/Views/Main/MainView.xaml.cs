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
                if (arg.Content is BaseMessageOptions option && option == BaseMessageOptions.LoadActualData)
                {
                    var component = RkiCoronaLandkreiseComponent.GetInstance();
                    var landkreise = component.LoadData();

                    var di = landkreise.Districts.Select(s => new DistrictItem
                    {
                        Name = s.Name,
                        Deaths = s.Deaths,
                        WeekIncidence = s.WeekIncidence
                    });

                    StaticDataManager.ActualLoadedDataDate = landkreise.Date;
                    StaticDataManager.ActualLoadedData = di;
                }

                this._viewModel.ActualDataFromDate = StaticDataManager.ActualLoadedDataDate.ToShortDateString();
                   
                this.Dispatcher.Invoke(() =>
                {
                    this._viewModel.Districts.Clear();
                });

                foreach (var item in StaticDataManager.ActualLoadedData)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        this._viewModel.Districts.Add(item);
                    });
                }

                SimpleStatusOverlays.ActivityOff();
            });
        }

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
