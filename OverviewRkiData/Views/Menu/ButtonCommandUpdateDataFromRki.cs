using Codexzier.Wpf.ApplicationFramework.Commands;
using Codexzier.Wpf.ApplicationFramework.Components.Ui.EventBus;
using OverviewRkiData.Views.Base;
using OverviewRkiData.Views.Main;
using System;
using System.Windows.Input;

namespace OverviewRkiData.Views.Menu
{
    internal class ButtonCommandUpdateDataFromRki : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter)
        {
            EventBusManager.Send<MainView, BaseMessage>(new BaseMessage(BaseMessageOptions.LoadActualData), 0);
        }
    }
}