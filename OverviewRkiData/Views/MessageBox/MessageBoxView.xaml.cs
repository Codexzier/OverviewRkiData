using OverviewRkiData.Components.Ui.EventBus;
using System.Windows;
using System.Windows.Controls;

namespace OverviewRkiData.Views.MessageBox
{
    /// <summary>
    /// Interaction logic for MessageBoxView.xaml
    /// </summary>
    public partial class MessageBoxView : UserControl
    {
        private readonly MessageBoxViewModel _viewModel;
        public MessageBoxView()
        {
            this.InitializeComponent();

            this._viewModel = (MessageBoxViewModel)this.DataContext;

            EventBusManager.Register<MessageBoxView, MessageBoxMessage>(this.BaseMessageEvent);
        }

        private void BaseMessageEvent(IMessageContainer arg)
        {
            if (arg is MessageBoxMessage boxMessage)
            {
                this._viewModel.Title = boxMessage.Title;
                this._viewModel.Message = $"{boxMessage.Content}";
            }
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            EventBusManager.CloseView<MessageBoxView>(10);
        }
    }
}
