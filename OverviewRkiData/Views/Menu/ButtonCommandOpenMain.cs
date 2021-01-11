using OverviewRkiData.Commands;
using OverviewRkiData.Components.Ui.EventBus;
using OverviewRkiData.Views.Main;
using System;
using System.Windows.Input;

namespace OverviewRkiData.Views.Menu
{
    internal class ButtonCommandOpenMain : ICommand
    {
        private readonly MenuViewModel _viewModel;

        public ButtonCommandOpenMain(MenuViewModel viewModel) => this._viewModel = viewModel;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (EventBusManager.IsViewOpen(typeof(MainView), 0))
            {
                return;
            }

            EventBusManager.OpenView<MainView>(0);
            this._viewModel.ViewOpened = EventBusManager.GetViewOpened(0);
            EventBusManager.Send<MainView, BaseMessage>(new BaseMessage(""), 0);
        }
    }
}