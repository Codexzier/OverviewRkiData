using OverviewRkiData.Commands;
using OverviewRkiData.Components.Ui.EventBus;
using System.Windows.Controls;

namespace OverviewRkiData.Views.ActivityLoading
{
    /// <summary>
    /// Interaction logic for ActivityLoadingView.xaml
    /// </summary>
    public partial class ActivityLoadingView : UserControl
    {
        private readonly ActivityLoadingViewModel _viewModel;
        public ActivityLoadingView()
        {
            this.InitializeComponent();

            this._viewModel = (ActivityLoadingViewModel)this.DataContext;

            EventBusManager.Register<ActivityLoadingView, BaseMessage>(this.BaseMessageEvent);
        }

        private void BaseMessageEvent(IMessageContainer arg) { }
    }
}
