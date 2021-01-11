using OverviewRkiData.Commands;
using OverviewRkiData.Components.Ui.EventBus;
using System.Windows.Controls;

namespace OverviewRkiData.Views.Dialog
{
    /// <summary>
    /// Interaction logic for DialogView.xaml
    /// </summary>
    public partial class DialogView : UserControl
    {
        private readonly DialogViewModel _viewModel;

        public DialogView()
        {
            this.InitializeComponent();

            this._viewModel = (DialogViewModel)this.DataContext;

            EventBusManager.Register<DialogView, BaseMessage>(this.BaseMessageEvent);

            this._viewModel.CommandSelectedPathDialogAccept = new ButtonCommandSelectedPathDialogAccept(this._viewModel);
        }

        private void BaseMessageEvent(IMessageContainer obj)
        {
            if (!(obj.Content is DataDialogContent dialogContent))
            {
                return;
            }

            this._viewModel.Header = dialogContent.Header;
        }
    }
}
