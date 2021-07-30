using Codexzier.Wpf.ApplicationFramework.Commands;
using Codexzier.Wpf.ApplicationFramework.Components.Ui.EventBus;
using Codexzier.Wpf.ApplicationFramework.Views.Base;
using OverviewRkiData.Views.County;

namespace OverviewRkiData.Views.Main
{
    internal class ChangedCommandSelectedDistrict : BaseCommand
    {
        private MainViewModel _viewModel;

        public ChangedCommandSelectedDistrict(MainViewModel viewModel) => this._viewModel = viewModel;

        public override void Execute(object parameter)
        {
            if(parameter == null)
            {
                return;
            }

            if (!EventBusManager.IsViewOpen<CountyView>(1))
            {
                EventBusManager.OpenView<CountyView>(1);
            }
            
            EventBusManager.Send<CountyView, BaseMessage>(new BaseMessage(parameter), 1);
        }
    }
}