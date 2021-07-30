using Codexzier.Wpf.ApplicationFramework.Views.Base;
using System.Windows.Input;

namespace OverviewRkiData.Views.Setup
{
    public class SetupViewModel : BaseViewModel
    {
        private bool _loadRkiDataByApplicationStart;
        private ICommand _commandLoadRkiDataByApplicationStart;
        private ICommand _commandImportDataFromLegacyApplication;
        private ICommand _commandLoadRkiData;
        private bool _diagramAnimationRightToLeft;
        private ICommand _commandDiagramAnimationRightToLeft;
        private bool _diagramAnimationOn;
        private ICommand _commandDiagramAnimationOn;
        private bool _fillMissingDataWithDummyValues;
        private ICommand _commandFillMissingDataWithDummyValues;

        public bool LoadRkiDataByApplicationStart
        {
            get => this._loadRkiDataByApplicationStart;
            set
            {
                this._loadRkiDataByApplicationStart = value;
                this.OnNotifyPropertyChanged(nameof(this.LoadRkiDataByApplicationStart));
            }
        }

        public ICommand CommandLoadRkiDataByApplicationStart
        {
            get => this._commandLoadRkiDataByApplicationStart;
            set
            {
                this._commandLoadRkiDataByApplicationStart = value;
                this.OnNotifyPropertyChanged(nameof(this.CommandLoadRkiDataByApplicationStart));
            }
        }

        public ICommand CommandImportDataFromLegacyApplication
        {
            get => this._commandImportDataFromLegacyApplication;
            set
            {
                this._commandImportDataFromLegacyApplication = value;
                this.OnNotifyPropertyChanged(nameof(this.CommandImportDataFromLegacyApplication));
            }
        }

        public ICommand CommandLoadRkiData
        {
            get => this._commandLoadRkiData;
            set
            {
                this._commandLoadRkiData = value;
                this.OnNotifyPropertyChanged(nameof(this.CommandLoadRkiData));
            }
        }

        public bool DiagramAnimationRightToLeft
        {
            get => this._diagramAnimationRightToLeft;
            set
            {
                this._diagramAnimationRightToLeft = value;
                this.OnNotifyPropertyChanged(nameof(this.DiagramAnimationRightToLeft));
            }
        }

        public ICommand CommandDiagramAnimationRightToLeft
        {
            get => this._commandDiagramAnimationRightToLeft;
            set
            {
                this._commandDiagramAnimationRightToLeft = value;
                this.OnNotifyPropertyChanged(nameof(this.CommandDiagramAnimationRightToLeft));
            }
        }

        public bool DiagramAnimationOn
        {
            get => this._diagramAnimationOn;
            set
            {
                this._diagramAnimationOn = value;
                this.OnNotifyPropertyChanged(nameof(this.DiagramAnimationOn));
            }
        }

        public ICommand CommandDiagramAnimationOn
        {
            get => this._commandDiagramAnimationOn;
            set
            {
                this._commandDiagramAnimationOn = value;
                this.OnNotifyPropertyChanged(nameof(this.CommandDiagramAnimationOn));
            }
        }

        public bool FillMissingDataWithDummyValues
        {
            get => this._fillMissingDataWithDummyValues;
            set
            {
                this._fillMissingDataWithDummyValues = value;
                this.OnNotifyPropertyChanged(nameof(this.FillMissingDataWithDummyValues));
            }
        }

        public ICommand CommandFillMissingDataWithDummyValues
        {
            get => this._commandFillMissingDataWithDummyValues;
            set
            {
                this._commandFillMissingDataWithDummyValues = value;
                this.OnNotifyPropertyChanged(nameof(this.CommandFillMissingDataWithDummyValues));
            }
        }
    }
}