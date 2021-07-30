using Codexzier.Wpf.ApplicationFramework.Commands;
using Codexzier.Wpf.ApplicationFramework.Components.Ui.EventBus;
using OverviewRkiData.Views.Main;
using System;
using System.Windows.Input;
using Codexzier.Wpf.ApplicationFramework.Views.Base;

namespace OverviewRkiData.Views.Menu
{
    internal class ButtonCommandOpenMain : BaseCommand
    {
        private readonly MenuViewModel _viewModel;

        public ButtonCommandOpenMain(MenuViewModel viewModel) => this._viewModel = viewModel;

        public override void Execute(object parameter)
        {
            if (EventBusManager.IsViewOpen<MainView>(0))
            {
                return;
            }

            EventBusManager.OpenView<MainView>(0);
            EventBusManager.Send<MainView, BaseMessage>(new BaseMessage(""), 0);
        }
    }
}