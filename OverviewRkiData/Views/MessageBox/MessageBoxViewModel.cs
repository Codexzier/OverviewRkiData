using System.Windows.Input;
using OverviewRkiData.Views.Base;

namespace OverviewRkiData.Views.MessageBox
{
    internal class MessageBoxViewModel : BaseViewModel
    {
        private string _title;
        private string _message;
        private ICommand _commandCancel;
        private ICommand _commandAccept;
        private string _labelAccept;

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

        public string LabelAccept
        {
            get => this._labelAccept;
            set
            {
                this._labelAccept = value;
                this.OnNotifyPropertyChanged(nameof(this.LabelAccept));
            }
        }

        public ICommand CommandAccept
        {
            get => this._commandAccept;
            set
            {
                this._commandAccept = value;
                this.OnNotifyPropertyChanged(nameof(this.CommandAccept));
            }
        }

        public ICommand CommandCancel
        {
            get => this._commandCancel;
            set
            {
                this._commandCancel = value;
                this.OnNotifyPropertyChanged(nameof(this.CommandCancel));
            }
        }
    }
}