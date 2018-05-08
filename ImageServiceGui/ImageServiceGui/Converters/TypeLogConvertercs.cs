using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageServiceGui.Converters
{
    public class TypeLogConvertercs: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("Must convert to a brush!");

            if (value is string) {
                string type = (string)value;

                switch (type)
                {
                    case "INFO":
                        return Brushes.Green;
                    case "WARRNING":
                        return Brushes.Yellow;
                    case "FAIL":
                        return Brushes.Red;
                   //Wrong string means no color
                    default:
                        return Brushes.Transparent;
                        
                }
            }

            throw new InvalidOperationException("You need to give a string");

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
