using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Media;

namespace OverviewRkiData.Converters
{
    public class RegexResultToBrushConverter : IValueConverter
    {
        private static string PATTERN_HEX_COLOR = "^((0x){0,1}|#{0,1})([0-9A-F]{8}|[0-9A-F]{6})$";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                if (IsValid(str))
                {
                    return new SolidColorBrush(Colors.Black);
                }
            }

            return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        public static bool IsValid(string enter)
        {
            if (string.IsNullOrEmpty(enter))
            {
                return false;
            }

            if (!enter.StartsWith("#"))
            {
                return false;
            }

            return Regex.IsMatch(enter.ToUpper(), PATTERN_HEX_COLOR);
        }
    }
}
