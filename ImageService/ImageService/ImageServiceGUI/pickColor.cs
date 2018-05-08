using ImageServiceInfrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ImageServiceGUI
{
    public class pickColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("Convert to brush");
            int type = (int)value;
            if(type == (int)MessageTypeEnum.FAIL)
            {
                return Brushes.Red;
            } else if ( type == (int)MessageTypeEnum.WARNING)
            {
                return Brushes.Yellow;
            } else
            {
                return Brushes.Green;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("Convert to brush");
            int type = (int)value;
            if (type == (int)MessageTypeEnum.FAIL)
            {
                return Brushes.Red;
            }
            else if (type == (int)MessageTypeEnum.WARNING)
            {
                return Brushes.Yellow;
            }
            else
            {
                return Brushes.Green;
            }
        }
    }
}
