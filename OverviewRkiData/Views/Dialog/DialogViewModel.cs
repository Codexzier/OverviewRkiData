using OverviewRkiData.Views.Base;
using System.Windows.Input;
using OverviewRkiData.Controls.FolderBrowser;

namespace OverviewRkiData.Views.Dialog
{
    internal class DialogViewModel : BaseViewModel
    {
        private string _header;
        private ICommand _commandCloseDialogView = new DoCloseDialogView();
        private ICommand _commandSelectedPathDialogAccept;
        private SelectedDirectory _selectedDirectoryPath = new SelectedDirectory();

        public DialogViewModel()
        {
            //this._selectedDirectoryPath.FolderNameHasChangedEvent += (string folderName) =>
            //{
            //    this._selectedDirectoryPath.FolderName = folderName;
            //};
        }
        
        public string Header
        {
            get => this._header; set
            {
                this._header = value;
                this.OnNotifyPropertyChanged(nameof(this.Header));
            }
        }

        public ICommand CommandCloseDialogView
        {
            get => this._commandCloseDialogView; set
            {
                this._commandCloseDialogView = value;
                this.OnNotifyPropertyChanged(nameof(this.CommandCloseDialogView));
            }
        }

        public ICommand CommandSelectedPathDialogAccept
        {
            get => this._commandSelectedPathDialogAccept;
            set
            {
                this._commandSelectedPathDialogAccept = value;
                this.OnNotifyPropertyChanged(nameof(this.CommandSelectedPathDialogAccept));
            }
        }

        public SelectedDirectory SelectedDirectoryPath
        {
            get => this._selectedDirectoryPath;
            set
            {
                this._selectedDirectoryPath = value;
                this.OnNotifyPropertyChanged(nameof(this.SelectedDirectoryPath));
            }
        }
    }
}