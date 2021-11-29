using Codexzier.Wpf.ApplicationFramework.Commands;
using Codexzier.Wpf.ApplicationFramework.Components.Ui.EventBus;

namespace OverviewRkiData.Views.Menu
{
    public partial class MenuView
    {
        private readonly MenuViewModel _viewModel;

        public MenuView()
        {
            this.InitializeComponent();

            this._viewModel = (MenuViewModel)this.DataContext;

            this._viewModel.CommandOpenMain = new ButtonCommandOpenMain();
            this._viewModel.CommandOpenSetup = new ButtonCommandOpenSetup();
            this._viewModel.CommandUpdateDataFromRki = new ButtonCommandUpdateDataFromRki();
            this._viewModel.CommandOpenLandkreise = new ButtonCommandOpenLandkreise();

            EventBusManager.Register<MenuView, BaseMessage>(this.BaseMessageEvent);
            //this._viewModel.ViewOpened = EventBusManager.GetViewOpened(0);
        }

        private void BaseMessageEvent(IMessageContainer arg)
        {
            // do things with the content
        }
    }
}
