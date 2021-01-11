using OverviewRkiData.Components.Ui.EventBus;
using OverviewRkiData.Views.Base;
using System.Windows.Input;

namespace OverviewRkiData.Views.Menu
{
    public class MenuViewModel : BaseViewModel
    {
        private ViewOpen _viewOpened = ViewOpen.Nothing;
        private ICommand _commandOpenMain;
        private ICommand _commandOpenSetup;

        public ViewOpen ViewOpened
        {
            get => this._viewOpened;
            set
            {
                this._viewOpened = value;
                this.OnNotifyPropertyChanged(nameof(this.ViewOpened));
            }
        }

        public ICommand CommandOpenMain
        {
            get => this._commandOpenMain;
            set
            {
                this._commandOpenMain = value;
                this.OnNotifyPropertyChanged(nameof(this.CommandOpenMain));
            }
        }

        public ICommand CommandOpenSetup
        {
            get => this._commandOpenSetup;
            set
            {
                this._commandOpenSetup = value;
                this.OnNotifyPropertyChanged(nameof(this.CommandOpenSetup));
            }
        }
    }
}
