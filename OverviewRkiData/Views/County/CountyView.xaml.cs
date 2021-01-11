using OverviewRkiData.Commands;
using OverviewRkiData.Components.Data;
using OverviewRkiData.Components.Ui.Anims;
using OverviewRkiData.Components.Ui.EventBus;
using OverviewRkiData.Controls.Diagram;
using OverviewRkiData.Views.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace OverviewRkiData.Views.County
{
    /// <summary>
    /// Interaction logic for CountyView.xaml
    /// </summary>
    public partial class CountyView : UserControl
    {
        private readonly CountyViewModel _viewModel;

        private readonly IList<(TextBlock, Storyboard)> _fadeIn = new List<(TextBlock, Storyboard)>();
        private readonly IList<(TextBlock, Storyboard)> _fadeOut = new List<(TextBlock, Storyboard)>();

        public CountyView()
        {
            this.InitializeComponent();

            this._viewModel = (CountyViewModel)this.DataContext;

            EventBusManager.Register<CountyView, BaseMessage>(this.CountyMessageEvent);
            this.InitializeAnimation();
        }

        private void InitializeAnimation()
        {
            this._fadeOut.Add((this.tbDescription_Name, new Storyboard()));
            this._fadeOut.Add((this.tbValue_Name, new Storyboard()));

            this._fadeOut.Add((this.tbDescription_Deaths, new Storyboard()));
            this._fadeOut.Add((this.tbValue_Deaths, new Storyboard()));

            this._fadeOut.Add((this.tbDescription_WeekIncidence, new Storyboard()));
            this._fadeOut.Add((this.tbValue_WeekIncidence, new Storyboard()));

            for (int index = 0; index < this._fadeOut.Count; index++)
            {
                Animations.SetFade(this._fadeOut[index].Item1, this._fadeOut[index].Item2, 1d, 0d, 0, 0);
            }

            this._fadeIn.Add((this.tbDescription_Name, new Storyboard()));
            this._fadeIn.Add((this.tbValue_Name, new Storyboard()));

            this._fadeIn.Add((this.tbDescription_Deaths, new Storyboard()));
            this._fadeIn.Add((this.tbValue_Deaths, new Storyboard()));

            this._fadeIn.Add((this.tbDescription_WeekIncidence, new Storyboard()));
            this._fadeIn.Add((this.tbValue_WeekIncidence, new Storyboard()));

            for (int index = 0; index < this._fadeIn.Count; index++)
            {
                Animations.SetFade(this._fadeIn[index].Item1, this._fadeIn[index].Item2, 0d, 1d, (index * 100) + 10, 1000);
            }
        }

        private async void CountyMessageEvent(IMessageContainer obj)
        {
            foreach (var item in this._fadeOut)
            {
                item.Item2.Begin();
            }

            if (obj.Content is DistrictItem districtItem)
            {
                foreach (var item in this._fadeIn)
                {
                    item.Item2.Begin();
                }

                this._viewModel.DistrictData = districtItem;

                await Task.Run(() =>
                {
                    var result = HelperExtension.GetCountyResults(districtItem.Name);

                    this._viewModel.CountyResults = result.Select(s =>
                    {
                        var toolTip = $"{s.Date:d} | {s.WeekIncidence:N1} | {s.Deaths}";
                        return new DiagramLevelItem { Value = s.WeekIncidence, ToolTipText = toolTip };
                    }).ToList();

                });
            }
        }
    }
}
