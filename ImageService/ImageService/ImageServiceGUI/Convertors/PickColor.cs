using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using ImageServiceInfrastructure.Enums;

namespace ImageServiceGUI
{
    class PickColor : IValueConverter
    {
        /// <summary>
        /// Convert function in order to set the backround color of the messageType
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
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
            else // if type is MessageTypeEnum.Info
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
