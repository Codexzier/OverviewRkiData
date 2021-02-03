using OverviewRkiData.Components.RkiCoronaLandkreise;
using System;
using System.Windows.Input;
using OverviewRkiData.Views.Base;

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
            component.RkiDataErrorEvent += this.Component_RkiDataErrorEvent;
            component.LoadData(out var saveIt);
            saveIt(true);
            component.RkiDataErrorEvent -= this.Component_RkiDataErrorEvent;
        }

        private void Component_RkiDataErrorEvent(string message) => SimpleStatusOverlays.Show("ERROR", message);
    }
}