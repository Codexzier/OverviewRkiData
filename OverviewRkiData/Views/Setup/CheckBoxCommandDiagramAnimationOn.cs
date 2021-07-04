using System;
using System.Windows.Input;
using OverviewRkiData.Components.UserSettings;

namespace OverviewRkiData.Views.Setup
{
    public class CheckBoxCommandDiagramAnimationOn : ICommand
    {
        private readonly SetupViewModel _viewModel;

        public CheckBoxCommandDiagramAnimationOn(SetupViewModel viewModel) => this._viewModel = viewModel;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var userSettings = UserSettingsLoader.GetInstance();
            var setting = userSettings.Load();

            setting.DiagramAnimationOn = this._viewModel.DiagramAnimationOn;

            userSettings.Save(setting);
        }

        public event EventHandler CanExecuteChanged;
    }
}