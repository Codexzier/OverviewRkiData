using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace OverviewRkiData.Components.Ui.Anims
{
    public static class AnimationsHelper
    {
        public static DoubleAnimation GetDoubleAnimation(double from, double to, double beginTime, double duration, bool autoReverse)
        {
            var da = new DoubleAnimation
            {
                From = from,
                To = to,
                BeginTime = TimeSpan.FromMilliseconds(beginTime),
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                AutoReverse = autoReverse,
                FillBehavior = FillBehavior.HoldEnd,
                DecelerationRatio = 0.3
            };

            return da;
        }

        public static ThicknessAnimation GetThicknessAnimation(Thickness from, Thickness to, double beginTime, double duration, bool autoReverse)
        {
            var ta = new ThicknessAnimation
            {
                From = from,
                To = to,
                BeginTime = TimeSpan.FromMilliseconds(beginTime),
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                AutoReverse = autoReverse,
                DecelerationRatio = 0.3
            };

            return ta;
        }

        public static Storyboard CreateStoryboard(double begin, double duration, bool autoReverse)
        {
            var sb = new Storyboard
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                BeginTime = TimeSpan.FromMilliseconds(begin),
                FillBehavior = FillBehavior.HoldEnd,
                AutoReverse = autoReverse
            };
            return sb;
        }
    }
}
