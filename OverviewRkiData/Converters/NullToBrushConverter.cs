using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace OverviewRkiData.Converters
{
    public class NullToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    return new SolidColorBrush(Colors.Black);
                }
            }

            return new SolidColorBrush(Colors.Red);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
