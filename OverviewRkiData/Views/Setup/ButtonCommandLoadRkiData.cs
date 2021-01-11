using OverviewRkiData.Components.RkiCoronaLandkreise;
using System;
using System.Windows.Input;

namespace OverviewRkiData.Views.Setup
{
    internal class ButtonCommandLoadRkiData : ICommand
    {
        private SetupViewModel _viewModel;

        public ButtonCommandLoadRkiData(SetupViewModel viewModel) => this._viewModel = viewModel;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter)
        {
            var component = RkiCoronaLandkreiseComponent.GetInstance();
            component.LoadData(true);

        }
    }
}