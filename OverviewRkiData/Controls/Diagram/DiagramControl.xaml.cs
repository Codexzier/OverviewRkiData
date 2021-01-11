using OverviewRkiData.Components.Ui.Anims;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace OverviewRkiData.Controls.Diagram
{
    public partial class DiagramControl : UserControl
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

        private static void SetValueToRects(DiagramControl control)
        {
            if (control.RkiCountyData == null)
            {
                return;
            }

            control.SimpleDiagram.Children.Clear();

            var heightScale = control.ActualHeight / 200d;

            control.OneHundred.Margin = new Thickness(0, 0, 0, 100 / control.Scale * heightScale);

            var widthPerResult = control.ActualWidth / control.RkiCountyData.Count;

            int delay = 1;
            foreach (var item in control.RkiCountyData)
            {
                var heightValue = item.Value / control.Scale * heightScale;

                var rect = new System.Windows.Shapes.Rectangle
                {
                    Fill = new SolidColorBrush(Color.FromArgb(255, 138, 187, 219)),
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Width = widthPerResult,
                    Height = heightValue,
                    ToolTip = item.ToolTipText,
                    Margin = new Thickness(0, 0, 0, heightValue * -1)
                };

                var storyboard = new Storyboard();
                Animations.SetMove(rect,
                    storyboard,
                    new Thickness(0, 0, 0, rect.Height * -1),
                    new Thickness(0),
                    delay * 20,
                    100);
                control.SimpleDiagram.Children.Add(rect);
                storyboard.Begin();
                delay++;
            }
        }

        public DiagramControl() => this.InitializeComponent();

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e) => SetValueToRects(this);
    }
}
