using OverviewRkiData.Views.Base;
using System.Windows.Input;

namespace OverviewRkiData.Views.Setup
{
    internal class SetupViewModel : BaseViewModel
    {
        private bool _loadRkiDataByApplicationStart;
        private ICommand _commandLoadRkiDataByApplicationStart;
        private ICommand _commandImportDataFromLegacyApplication;
        private ICommand _commandLoadRkiData;

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
    }
}