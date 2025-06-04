using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VoorraadbeheerSysteemProject.Wpf.Converters
{
    internal class DifferenceToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is decimal decimalValue)
            {
                if(decimalValue > 0)
                    return "green"; // Positive values
                else if (decimalValue < 0)
                    return "red"; // Negative values
            }
            return "black"; // Default color for zero or non-decimal values
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
