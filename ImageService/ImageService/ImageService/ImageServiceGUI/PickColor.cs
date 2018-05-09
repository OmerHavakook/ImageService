using ImageServiceInfrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageServiceGUI
{
    class PickColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(System.Windows.Media.Brush))
                throw new InvalidOperationException("Convert to brush");
            
            MessageTypeEnum type = (MessageTypeEnum)value;
            if(type == MessageTypeEnum.FAIL)
            {
                return Brushes.Red;
            } else if ( type == MessageTypeEnum.WARNING)
            {
                return Brushes.Yellow;
            } else
            {
                return Brushes.Green;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MessageTypeEnum type = (MessageTypeEnum)value;

            if (type == MessageTypeEnum.FAIL)
            {
                return Brushes.Red;
            }
            else if (type == MessageTypeEnum.WARNING)
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
