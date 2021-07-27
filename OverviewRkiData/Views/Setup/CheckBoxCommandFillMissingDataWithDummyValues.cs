using System;
using System.Windows.Input;
using OverviewRkiData.Components.UserSettings;

namespace OverviewRkiData.Views.Setup
{
    public class CheckBoxCommandFillMissingDataWithDummyValues : ICommand
    {
        private readonly SetupViewModel _viewModel;

        public CheckBoxCommandFillMissingDataWithDummyValues(SetupViewModel viewModel)
        {
            this._viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var userSettings = UserSettingsLoader.GetInstance();
            var setting = userSettings.Load();

            setting.FillMissingDataWithDummyValues = this._viewModel.FillMissingDataWithDummyValues;

            userSettings.Save(setting);
        }

        public event EventHandler CanExecuteChanged;
    }
}