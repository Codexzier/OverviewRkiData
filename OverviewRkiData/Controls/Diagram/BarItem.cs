using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OverviewRkiData.Controls.Diagram
{
    public class BarItem
    {
        public Rectangle Bar { get; }

        public BarItem(double widthPerResult,
            double heightValue,
            string toolTipText,
            double itemValue,
            bool setHighlightMark,
            int setColor,
            bool forAnimationSetup)
        {
            var barColorNormal = new SolidColorBrush(this.SetUpValueIfOverHundred(itemValue, setColor));
            var barColorHighlighted = new SolidColorBrush(Color.FromArgb(255, 160, 200, 219));
            
            this.Bar = new Rectangle
            {
                Fill = barColorNormal,
                VerticalAlignment = VerticalAlignment.Bottom,
                Width = widthPerResult,
                Height = heightValue,
                ToolTip = toolTipText,
                Margin = new Thickness(0, 0, 0, forAnimationSetup ? heightValue * -1 : 0)
            };

            if (setHighlightMark)
            {
                this.Bar.StrokeThickness = 3;
                this.Bar.Stroke = new SolidColorBrush(Color.FromArgb(255, 200, 250, 219));
            }

            this.Bar.MouseEnter += (e, r) =>
            {
                if (!(e is Rectangle subRect))
                {
                    return;
                }

                subRect.Fill = barColorHighlighted;
            };

            this.Bar.MouseLeave += (e, r) =>
            {
                if (!(e is Rectangle subRect))
                {
                    return;
                }

                subRect.Fill = barColorNormal;
            };
        }

        private Color SetUpValueIfOverHundred(double value, int setColor)
        {
            if (value < 100)
            {
                return ColorSetup(colorNr: setColor);
            }

            var red = value - 100 + 138;
            if (red >= 256)
            {
                red = 255;
            }

            return ColorSetup((byte)red, setColor);

            static Color ColorSetup(byte red = 138, int colorNr = 0)
            {
                return colorNr switch
                {
                    1 => Color.FromArgb(255, 31, 240, 127),
                    2 => Color.FromArgb(255, 255, 242, 0),
                    _ => Color.FromArgb(255, red, 187, 219)
                };
            }
        }
    }
}