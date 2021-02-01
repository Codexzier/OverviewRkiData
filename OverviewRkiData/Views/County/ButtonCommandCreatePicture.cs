using System;
using System.Windows;
using System.Windows.Input;
using OverviewRkiData.Components.WpfRender;
using OverviewRkiData.Views.Base;
using OverviewRkiData.Views.RenderPicture;

namespace OverviewRkiData.Views.County
{
    public class ButtonCommandCreatePicture : ICommand
    {
        private readonly CountyViewModel _viewModel;
        private readonly RenderPicturePrint _renderPicturePrint;

        public ButtonCommandCreatePicture(CountyViewModel viewModel, RenderPicturePrint renderPicturePrint)
        {
            this._viewModel = viewModel;
            this._renderPicturePrint = renderPicturePrint;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var filename = $"{Environment.CurrentDirectory}/rki-status-{this._viewModel.DistrictData.Date:dd-MM-yyyy}.jpg";
            
            if (!WpfControlToBitmap.SaveControlImage(this._renderPicturePrint, filename))
            {
                SimpleStatusOverlays.Show("ERROR", "Can't save picture!");
            }

            this._renderPicturePrint.Visibility = Visibility.Hidden;
        }

        public event EventHandler CanExecuteChanged;
    }
}