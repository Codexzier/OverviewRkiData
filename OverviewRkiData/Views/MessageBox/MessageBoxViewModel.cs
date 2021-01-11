using OverviewRkiData.Views.Base;

namespace OverviewRkiData.Views.MessageBox
{
    internal class MessageBoxViewModel : BaseViewModel
    {
        private string _title;
        private string _message;

        public string Title
        {
            get => this._title;
            set
            {
                this._title = value;
                this.OnNotifyPropertyChanged(nameof(this.Title));
            }
        }

        public string Message
        {
            get => this._message;
            set
            {
                this._message = value;
                this.OnNotifyPropertyChanged(nameof(this.Message));
            }
        }
    }
}