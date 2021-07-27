using OverviewRkiData.Components.UserSettings;
using System.Windows.Controls;

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
            this._viewModel.CommandLoadRkiData = new ButtonCommandLoadRkiData(this._viewModel);

            this._viewModel.CommandDiagramAnimationOn = new CheckBoxCommandDiagramAnimationOn(this._viewModel);
            this._viewModel.CommandDiagramAnimationRightToLeft = new CheckBoxCommandDiagramAnimationRightToLeft(this._viewModel);
            this._viewModel.CommandFillMissingDataWithDummyValues = new CheckBoxCommandFillMissingDataWithDummyValues(this._viewModel);
        }

        public override void OnApplyTemplate()
        {
            var setting = UserSettingsLoader.GetInstance().Load();
            this._viewModel.LoadRkiDataByApplicationStart = setting.LoadRkiDataByApplicationStart;
            this._viewModel.DiagramAnimationOn = setting.DiagramAnimationOn;
            this._viewModel.DiagramAnimationRightToLeft = setting.DiagramAnimationRightToLeft;
            this._viewModel.FillMissingDataWithDummyValues = setting.FillMissingDataWithDummyValues;
        }
               
    }
}
