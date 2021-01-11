namespace OverviewRkiData.Controls.FolderBrowser
{
    public class SelectedDirectory
    {
        private string _folderName;

        public string FolderName
        {
            get => this._folderName;
            set
            {
                this._folderName = value;
                this.FolderNameHasChangedEvent?.Invoke(value);
            }
        }

        public delegate void FolderNameHasChangedEventHandler(string folderName);

        public event FolderNameHasChangedEventHandler FolderNameHasChangedEvent;
    }
}