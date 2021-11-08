using System;
using System.Windows.Input;
using Codexzier.Wpf.ApplicationFramework.Components.UserSettings;
using Codexzier.Wpf.ApplicationFramework.Views.Base;
using OverviewRkiData.Components;
using OverviewRkiData.Components.UserSettings;

namespace OverviewRkiData.Views.Setup
{
    /// <summary>
    /// Interaction logic for SetupView.xaml
    /// </summary>
    public partial class SetupView
    {
        private readonly SetupViewModel _viewModel;
        public SetupView()
        {
            this.InitializeComponent();

            this._viewModel = (SetupViewModel)this.DataContext;

            this._viewModel.CommandLoadRkiDataByApplicationStart = new CheckBoxCommandLoadRkiDataByApplicationStart(this._viewModel);
            this._viewModel.CommandImportDataFromLegacyApplication = new ButtonCommandImportDataFromLegacyApplication(this._viewModel);
            this._viewModel.CommandLoadRkiData = new ButtonCommandLoadRkiData();

            this._viewModel.CommandDiagramAnimationOn = new CheckBoxCommandDiagramAnimationOn(this._viewModel);
            this._viewModel.CommandDiagramAnimationRightToLeft = new CheckBoxCommandDiagramAnimationRightToLeft(this._viewModel);
            this._viewModel.CommandFillMissingDataWithDummyValues = new CheckBoxCommandFillMissingDataWithDummyValues(this._viewModel);
            this._viewModel.CommandOnlyShowLast200Values = new CheckBoxCommandOnlyShowLast200Values(this._viewModel);
        }

        public override void OnApplyTemplate()
        {
            var setting = UserSettingsLoader<CustomSettingsFile>.GetInstance(SerializeHelper.Serialize, SerializeHelper.Deserialize).Load();
            this._viewModel.LoadRkiDataByApplicationStart = setting.LoadRkiDataByApplicationStart;
            this._viewModel.DiagramAnimationOn = setting.DiagramAnimationOn;
            this._viewModel.DiagramAnimationRightToLeft = setting.DiagramAnimationRightToLeft;
            this._viewModel.FillMissingDataWithDummyValues = setting.FillMissingDataWithDummyValues;
            this._viewModel.OnlyShowLast200Values = setting.OnlyShowLast200Values;
        }
               
    }

    public class CheckBoxCommandOnlyShowLast200Values : BaseCommand
    {
        private readonly SetupViewModel _viewModel;

        public CheckBoxCommandOnlyShowLast200Values(SetupViewModel viewModel)
        {
            this._viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            var userSettings = UserSettingsLoader<CustomSettingsFile>.GetInstance(SerializeHelper.Serialize, SerializeHelper.Deserialize);
            var setting = userSettings.Load();

            setting.OnlyShowLast200Values = this._viewModel.OnlyShowLast200Values;
            
            userSettings.Save(setting);
        }
    }
}
