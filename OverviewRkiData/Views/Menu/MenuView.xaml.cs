using OverviewRkiData.Commands;
using OverviewRkiData.Components.Ui.EventBus;
using System.Windows.Controls;

namespace OverviewRkiData.Views.Menu
{
    public partial class MenuView : UserControl
    {
        private readonly MenuViewModel _viewModel;

        public MenuView()
        {
            this.InitializeComponent();

            this._viewModel = (MenuViewModel)this.DataContext;

            this._viewModel.CommandOpenMain = new ButtonCommandOpenMain(this._viewModel);
            this._viewModel.CommandOpenSetup = new ButtonCommandOpenSetup(this._viewModel);

            EventBusManager.Register<MenuView, BaseMessage>(this.BaseMessageEvent);
            this._viewModel.ViewOpened = EventBusManager.GetViewOpened(0);
        }

        private void BaseMessageEvent(IMessageContainer arg)
        {
            // do things with the content
        }
    }
}
