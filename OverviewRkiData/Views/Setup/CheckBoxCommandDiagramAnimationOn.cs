using Codexzier.Wpf.ApplicationFramework.Components.UserSettings;
using Codexzier.Wpf.ApplicationFramework.Views.Base;
using OverviewRkiData.Components;
using OverviewRkiData.Components.UserSettings;

namespace OverviewRkiData.Views.Setup
{
    public class CheckBoxCommandDiagramAnimationOn : BaseCommand
    {
        private readonly SetupViewModel _viewModel;

        public CheckBoxCommandDiagramAnimationOn(SetupViewModel viewModel) => this._viewModel = viewModel;
        
        public override void Execute(object parameter)
        {
            var userSettings = UserSettingsLoader<CustomSettingsFile>.GetInstance(SerializeHelper.Serialize, SerializeHelper.Deserialize);
            var setting = userSettings.Load();

            setting.DiagramAnimationOn = this._viewModel.DiagramAnimationOn;

            userSettings.Save(setting);
        }
    }
}