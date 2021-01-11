using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace OverviewRkiData.Converters
{
    public class NormalizeTextConverter : IValueConverter
    {
        private readonly IDictionary<string, string> _specializeCharReplaceAlternate = new Dictionary<string, string>
        {
            {"ü", "ue" },
            {"Ü", "Ue" },
            {"ä", "Ae" },
            {"Ä", "Ae" },
            {"ö", "oe" },
            {"Ö", "Oe" }
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && this.TryNormalizeText(str, out var result))
            {
                return result;
            }

            return value;
        }

        private bool TryNormalizeText(string value, out string result)
        {
            result = string.Empty;

            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            if (!this._specializeCharReplaceAlternate.Keys.Any(a => value.Contains(a)))
            {
                return false;
            }

            foreach (var kvp in this._specializeCharReplaceAlternate)
            {
                value = value.Replace(kvp.Key, kvp.Value);
            }
            result = value;

            return true;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
