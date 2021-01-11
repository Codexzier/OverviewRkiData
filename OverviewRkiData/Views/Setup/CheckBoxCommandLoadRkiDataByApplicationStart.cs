using OverviewRkiData.Components.UserSettings;
using System;
using System.Windows.Input;

namespace OverviewRkiData.Views.Setup
{
    internal class CheckBoxCommandLoadRkiDataByApplicationStart : ICommand
    {
        private readonly SetupViewModel _viewModel;

        public CheckBoxCommandLoadRkiDataByApplicationStart(SetupViewModel viewModel) => this._viewModel = viewModel;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter)
        {
            var userSettings = UserSettingsLoader.GetInstance();
            var setting = userSettings.Load();

            setting.LoadRkiDataByApplicationStart = this._viewModel.LoadRkiDataByApplicationStart;

            userSettings.Save(setting);
        }
    }
}