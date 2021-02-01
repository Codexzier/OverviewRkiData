using OverviewRkiData.Controls.Diagram;
using OverviewRkiData.Views.Base;
using OverviewRkiData.Views.Data;
using System.Collections.Generic;
using System.Windows.Controls;

namespace OverviewRkiData.Views.RenderPicture
{
    /// <summary>
    /// Interaction logic for RenderPicturePrint.xaml
    /// </summary>
    public partial class RenderPicturePrint : UserControl
    {
        private readonly RenderPicturePrintViewModel _viewModel;

        public RenderPicturePrint()
        {
            this.InitializeComponent();

            this._viewModel = (RenderPicturePrintViewModel) this.DataContext;
        }
    }

    internal class RenderPicturePrintViewModel : BaseViewModel
    {
        private DistrictItem _districtData;

        public DistrictItem DistrictData
        {
            get => this._districtData;
            set
            {
                this._districtData = value;
                this.OnNotifyPropertyChanged(nameof(this.DistrictData));
            }
        }

        private List<DiagramLevelItem> _countyResults;

        public List<DiagramLevelItem> CountyResults
        {
            get => this._countyResults;
            set
            {
                this._countyResults = value;
                this.OnNotifyPropertyChanged(nameof(this.CountyResults));
            }
        }
    }
}
