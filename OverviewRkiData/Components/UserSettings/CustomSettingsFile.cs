using Codexzier.Wpf.ApplicationFramework.Components.UserSettings;
using Newtonsoft.Json;

namespace OverviewRkiData.Components.UserSettings
{
    public class CustomSettingsFile : SettingsFile
    {
        private string _lastImportDirectory;
        private string _lastImportFilename;
        private bool _loadFromService;
        private bool _loadRkiDataByApplicationStart;
        private bool _diagramAnimationRightToLeft;
        private bool _diagramAnimationOn;
        private bool _fillMissingDataWithDummyValues;
        
        public string LastImportDirectory
        {
            get => this._lastImportDirectory; set
            {
                this._lastImportDirectory = value;
                this.SetChanged();
            }
        }

        public string LastImportFilename
        {
            get => this._lastImportFilename; set
            {
                this._lastImportFilename = value;
                this.SetChanged();
            }
        }

        public bool LoadFromService
        {
            get => this._loadFromService; set
            {
                this._loadFromService = value;
                this.SetChanged();
            }
        }

        public bool LoadRkiDataByApplicationStart
        {
            get => this._loadRkiDataByApplicationStart;
            set
            {
                this._loadRkiDataByApplicationStart = value;
                this.SetChanged();
            }
        }

        public bool DiagramAnimationRightToLeft
        {
            get => this._diagramAnimationRightToLeft;
            set
            {
                this._diagramAnimationRightToLeft = value;
                this.SetChanged();
            }
        }

        public bool DiagramAnimationOn
        {
            get => this._diagramAnimationOn;
            set
            {
                this._diagramAnimationOn = value;
                this.SetChanged();
            }
        }

        public bool FillMissingDataWithDummyValues
        {
            get => this._fillMissingDataWithDummyValues;
            set
            {
                this._fillMissingDataWithDummyValues = value;
                this.SetChanged();
            }
        }
    }
}