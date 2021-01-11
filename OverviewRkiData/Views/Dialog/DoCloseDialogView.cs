using System;
using System.Windows.Input;
using OverviewRkiData.Components.Ui.EventBus;
using OverviewRkiData.Views.DialogContent;

namespace OverviewRkiData.Views.Dialog
{
    internal class DoCloseDialogView : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            EventBusManager.CloseView<DialogContentView>(2);
            EventBusManager.CloseView<DialogView>(10);
        }
    }
}