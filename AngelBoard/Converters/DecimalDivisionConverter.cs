using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace AngelBoard.Converters
{
    public class DecimalDivisionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is decimal m)
            {
                int.TryParse((string)parameter, out int multiplier);

                return (m / multiplier).ToString();
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            int.TryParse((string)parameter, out int multiplier);

            if (value != null)
            {
                if (decimal.TryParse(value.ToString(), out decimal m))
                {
                    return m * multiplier;
                }
            }
            return 0m;
        }
    }
}
