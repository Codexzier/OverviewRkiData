using Codexzier.Wpf.ApplicationFramework.Views.Base;
using OverviewRkiData.Views.Data;
using System.Collections.ObjectModel;
using System.Linq;

namespace OverviewRkiData.Views.Main
{
    internal class ButtonCommandSortByWeekIncidence : BaseCommand
    {
        private readonly MainViewModel _viewModel;

        public ButtonCommandSortByWeekIncidence(MainViewModel viewModel) => this._viewModel = viewModel;

        public override void Execute(object parameter)
        {
            if(StaticDataManager.ActualLoadedData == null || !StaticDataManager.ActualLoadedData.Any())
            {
                SimpleStatusOverlays.Show("TIP", "No data loaded");
                return;
            }

            var ordered = StaticDataManager.ActualLoadedData.OrderByDescending(order => order.WeekIncidence);
            this._viewModel.Districts = new ObservableCollection<DistrictItem>(ordered);
        }
    }
}