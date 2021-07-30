using System;
using System.Windows.Input;
using Codexzier.Wpf.ApplicationFramework.Components.UserSettings;
using Codexzier.Wpf.ApplicationFramework.Views.Base;
using OverviewRkiData.Components;
using OverviewRkiData.Components.UserSettings;

namespace OverviewRkiData.Views.Setup
{
    public class CheckBoxCommandFillMissingDataWithDummyValues : BaseCommand
    {
        private readonly SetupViewModel _viewModel;

        public CheckBoxCommandFillMissingDataWithDummyValues(SetupViewModel viewModel)
        {
            this._viewModel = viewModel;
        }
        
        public override void Execute(object parameter)
        {
            var userSettings = UserSettingsLoader<CustomSettingsFile>.GetInstance(SerializeHelper.Serialize, SerializeHelper.Deserialize);
            var setting = userSettings.Load();

            setting.FillMissingDataWithDummyValues = this._viewModel.FillMissingDataWithDummyValues;

            userSettings.Save(setting);
        }
    }
}