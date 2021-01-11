using OverviewRkiData.Commands;
using OverviewRkiData.Components.Ui.EventBus;
using OverviewRkiData.Views.County;
using System;
using System.Windows.Input;

namespace OverviewRkiData.Views.Main
{
    internal class ChangedCommandSelectedDistrict : ICommand
    {
        private MainViewModel _viewModel;

        public ChangedCommandSelectedDistrict(MainViewModel viewModel) => this._viewModel = viewModel;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => throw new NotImplementedException();
        public void Execute(object parameter)
        {
            if(parameter == null)
            {
                return;
            }

            if (!EventBusManager.IsViewOpen(typeof(CountyView), 1))
            {
                EventBusManager.OpenView<CountyView>(1);
                
            }
            
            EventBusManager.Send<CountyView, BaseMessage>(new BaseMessage(parameter), 1);
        }
    }
}