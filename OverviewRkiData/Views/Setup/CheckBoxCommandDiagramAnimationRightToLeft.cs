using System;
using System.Windows.Input;
using OverviewRkiData.Components.UserSettings;

namespace OverviewRkiData.Views.Setup
{
    public class CheckBoxCommandDiagramAnimationRightToLeft : ICommand
    {
        private readonly SetupViewModel _viewModel;

        public CheckBoxCommandDiagramAnimationRightToLeft(SetupViewModel viewModel) => this._viewModel = viewModel;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var userSettings = UserSettingsLoader.GetInstance();
            var setting = userSettings.Load();

            setting.DiagramAnimationRightToLeft = this._viewModel.DiagramAnimationRightToLeft;

            userSettings.Save(setting);
        }

        public event EventHandler CanExecuteChanged;
    }
}