using System.Windows.Controls;

namespace OverviewRkiData.Views.DialogContent
{
    /// <summary>
    /// Interaction logic for DialogContentView.xaml
    /// </summary>
    public partial class DialogContentView : UserControl
    {
        private readonly DialogContentViewModel _viewModel;
        public DialogContentView()
        {
            this.InitializeComponent();

            this._viewModel = (DialogContentViewModel)this.DataContext;
        }
    }
}
