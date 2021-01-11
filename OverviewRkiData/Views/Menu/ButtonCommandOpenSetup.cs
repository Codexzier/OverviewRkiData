using OverviewRkiData.Components.Ui.EventBus;
using OverviewRkiData.Views.County;
using OverviewRkiData.Views.Setup;
using System;
using System.Windows.Input;

namespace OverviewRkiData.Views.Menu
{
    internal class ButtonCommandOpenSetup : ICommand
    {
        private readonly MenuViewModel _viewModel;

        public ButtonCommandOpenSetup(MenuViewModel viewModel) => this._viewModel = viewModel;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter)
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
            this._viewModel.ViewOpened = EventBusManager.GetViewOpened(0);
        }
    }
}