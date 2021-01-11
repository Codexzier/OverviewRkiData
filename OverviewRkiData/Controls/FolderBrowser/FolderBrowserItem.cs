using System.IO;

namespace OverviewRkiData.Controls.FolderBrowser
{
    internal class FolderBrowserItem
    {
        private readonly string _path;

        public bool ReturnFolderItem { get; }

        public FolderBrowserItem(string path, bool returnFolderItem = false)
        {
            this._path = path;
            this.ReturnFolderItem = returnFolderItem;
        }

        public string CompletePath => this._path;
        public string FolderName
        {
            get
            {
                var di = new DirectoryInfo(this._path);
                if(di.Parent == null)
                {
                    return string.Empty;
                }

                if(this.ReturnFolderItem)
                {
                    return "...";
                }

                return di.Name;
            }
        }
    }
}