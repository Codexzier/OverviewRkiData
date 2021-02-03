using System;
using OverviewRkiData.Components.Ui.EventBus;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            EventBusManager.Register<MessageBoxView, AskBoxMessage>(this.ASkMessageEvent);
        }

        private void ASkMessageEvent(IMessageContainer arg)
        {
            this.BaseMessageEvent(arg);

            if (arg is AskBoxMessage askBoxMessage)
            {
                this._viewModel.LabelAccept = "Accept";
                this._viewModel.CommandAccept = new ButtonCommandAccept(askBoxMessage);
                this._viewModel.CommandCancel = new ButtonCommandCancel();
            }

        }

        private void BaseMessageEvent(IMessageContainer arg)
        {
            if (arg is MessageBoxMessage boxMessage)
            {
                this._viewModel.CommandAccept = new ButtonCommandOk();
                this._viewModel.LabelAccept = "OK";
                this._viewModel.Title = boxMessage.Title;
                this._viewModel.Message = $"{boxMessage.Content}";
            }
        }
    }

    internal class ButtonCommandAccept : ICommand
    {
        private readonly AskBoxMessage _askBoxMessage;

        public ButtonCommandAccept(AskBoxMessage askBoxMessage)
        {
            this._askBoxMessage = askBoxMessage;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            this._askBoxMessage.Execute(true);
            EventBusManager.CloseView<MessageBoxView>(10);
        }

        public event EventHandler CanExecuteChanged;
    }

    internal class ButtonCommandOk : ICommand
    {
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            EventBusManager.CloseView<MessageBoxView>(10);
        }

        public event EventHandler CanExecuteChanged;
    }

    internal class ButtonCommandCancel : ICommand
    {
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            EventBusManager.CloseView<MessageBoxView>(10);
        }

        public event EventHandler CanExecuteChanged;
    }
}
