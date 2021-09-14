using Codexzier.Wpf.ApplicationFramework.Views.Base;
using System.Windows.Input;

namespace OverviewRkiData.Views.Menu
{
    public class MenuViewModel : BaseViewModel
    {
        private ICommand _commandOpenMain;
        private ICommand _commandOpenSetup;
        private ICommand _commandUpdateDataFromRki;

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

        public ICommand CommandUpdateDataFromRki
        {
            get => this._commandUpdateDataFromRki;
            set
            {
                this._commandUpdateDataFromRki = value;
                this.OnNotifyPropertyChanged(nameof(this.CommandUpdateDataFromRki));
            }
        }
    }
}
