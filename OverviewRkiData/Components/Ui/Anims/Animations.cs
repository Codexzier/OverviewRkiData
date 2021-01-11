using System.Windows;
using System.Windows.Media.Animation;

namespace OverviewRkiData.Components.Ui.Anims
{
    public static class Animations
    {
        public static TDependencyObject SetMove<TDependencyObject>(
            TDependencyObject control,
            Storyboard storyboard,
            Thickness from,
            Thickness to,
            double beginTime = 0,
            double duration = 100,
            bool autoReverse = false) where TDependencyObject : DependencyObject
        {
            storyboard ??= AnimationsHelper.CreateStoryboard(0, duration, autoReverse);

            var ta = AnimationsHelper.GetThicknessAnimation(from, to, beginTime, duration, autoReverse);

            ta.AccelerationRatio = .2;
            ta.DecelerationRatio = .2;

            storyboard.Children.Add(ta);

            Storyboard.SetTarget(ta, control);
            Storyboard.SetTargetProperty(ta, new PropertyPath("(Canvas.Margin)"));

            return control;
        }

        public static void SetFade<TDependencyObject>(
            TDependencyObject control,
            Storyboard storyboard,
            double from,
            double to,
            double beginTime = 0,
            double duration = 100,
            bool autoReverse = false) where TDependencyObject : DependencyObject
        {
            if (storyboard == null)
            {
                storyboard = AnimationsHelper.CreateStoryboard(0, duration, autoReverse);
            }

            var da = AnimationsHelper.GetDoubleAnimation(from, to, beginTime, duration, autoReverse);

            storyboard.Children.Add(da);

            Storyboard.SetTarget(da, control);
            Storyboard.SetTargetProperty(da, new PropertyPath(UIElement.OpacityProperty));
        }
    }
}
