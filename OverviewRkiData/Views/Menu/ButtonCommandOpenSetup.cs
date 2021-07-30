using Codexzier.Wpf.ApplicationFramework.Components.Ui.EventBus;
using OverviewRkiData.Views.County;
using OverviewRkiData.Views.Setup;
using System;
using System.Windows.Input;
using Codexzier.Wpf.ApplicationFramework.Views.Base;

namespace OverviewRkiData.Views.Menu
{
    internal class ButtonCommandOpenSetup : BaseCommand
    {
        private readonly MenuViewModel _viewModel;

        public ButtonCommandOpenSetup(MenuViewModel viewModel) => this._viewModel = viewModel;

        public override void Execute(object parameter)
        {
            if (EventBusManager.IsViewOpen<SetupView>(0))
            {
                return;
            }

            if (EventBusManager.IsViewOpen<CountyView>(1))
            {
                EventBusManager.CloseView<CountyView>(1);
            }

            EventBusManager.OpenView<SetupView>(0);
        }
    }
}