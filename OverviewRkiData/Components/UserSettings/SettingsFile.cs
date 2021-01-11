using Newtonsoft.Json;

namespace OverviewRkiData.Components.UserSettings
{
    public class SettingsFile
    {
        private int _sizeX;
        private int _sizeY;
        private string _applicationWindowState;
        private int _applicationPositionX;
        private int _applicationPositionY;
        private string _lastImportDirectory;
        private string _lastImportFilename;
        private bool _loadFromService;
        private bool _loadRkiDataByApplicationStart;

        public SettingsFile(bool hasChanged) => this.HasChanged = hasChanged;

        [JsonIgnore]
        public bool HasChanged { get; private set; }

        private void SetChanged() => this.HasChanged = true;
        internal void NoChanged() => this.HasChanged = false;

        public int SizeX
        {
            get => this._sizeX; set
            {
                this._sizeX = value;
                this.SetChanged();
            }
        }
        public int SizeY
        {
            get => this._sizeY;
            set
            {
                this._sizeY = value;
                this.SetChanged();
            }
        }

        public string ApplicationWindowState
        {
            get => this._applicationWindowState;
            set
            {
                this._applicationWindowState = value;
                this.SetChanged();
            }
        }

        public int ApplicationPositionX
        {
            get => this._applicationPositionX;
            set
            {
                this._applicationPositionX = value;
                this.SetChanged();
            }
        }
        public int ApplicationPositionY
        {
            get => this._applicationPositionY;
            set
            {
                this._applicationPositionY = value;
                this.SetChanged();
            }
        }

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
    }
}