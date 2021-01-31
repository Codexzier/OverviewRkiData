using OverviewRkiData.Components.Ui.Anims;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;

namespace OverviewRkiData.Controls.Diagram
{
    public partial class DiagramControl 
    {
        public double Scale
        {
            get => (double)this.GetValue(ScaleProperty);
            set => this.SetValue(ScaleProperty, value);
        }

        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.RegisterAttached(
                "Scale",
                typeof(double),
                typeof(DiagramControl),
                new PropertyMetadata(2.5, UpdateDiagram));

        public List<DiagramLevelItem> RkiCountyData
        {
            get => (List<DiagramLevelItem>)this.GetValue(RkiCountyDataProperty);
            set => this.SetValue(RkiCountyDataProperty, value);
        }

        public static readonly DependencyProperty RkiCountyDataProperty =
            DependencyProperty.RegisterAttached("RkiCountyData",
                typeof(List<DiagramLevelItem>),
                typeof(DiagramControl),
                new PropertyMetadata(new List<DiagramLevelItem>(), UpdateDiagram));

        private static void UpdateDiagram(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DiagramControl control)
            {
                SetValueToRects(control);
            }
        }

        // TODO: Not sure, but I have an idea.
        private readonly IList<BarItem> _barItems = new List<BarItem>();

        private static void SetValueToRects(DiagramControl control)
        {
            if (control.RkiCountyData == null)
            {
                return;
            }

            control._barItems.Clear();
            control.SimpleDiagram.Children.Clear();

            var heightScale = control.ActualHeight / 200d;

            control.OneHundred.Margin = new Thickness(0, 0, 0, 100 / control.Scale * heightScale);
            control.OneHundredText.Margin = new Thickness(0, 0, 0, 100 / control.Scale * heightScale);

            var widthPerResult = control.ActualWidth / control.RkiCountyData.Count;

            int delay = 1;
            foreach (var item in control.RkiCountyData)
            {
                var heightValue = item.Value / control.Scale * heightScale;

                var barItem = new BarItem(widthPerResult, heightValue, item.ToolTipText, item.Value);

                var storyboard = new Storyboard();
                Animations.SetMove(barItem.Bar,
                    storyboard,
                    new Thickness(0, 0, 0, barItem.Bar.Height * -1),
                    new Thickness(0),
                    delay * 20);
                
                control.SimpleDiagram.Children.Add(barItem.Bar);
                storyboard.Begin();
                delay++;

                control._barItems.Add(barItem);
            }
        }

        public DiagramControl() => this.InitializeComponent();

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e) => SetValueToRects(this);
    }
}
