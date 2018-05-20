using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using ImageServiceInfrastructure.Enums;

namespace ImageServiceGUI
{
    class PickColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(System.Windows.Media.Brush))
                throw new InvalidOperationException("Convert to brush");

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
                return Brushes.LawnGreen;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}
