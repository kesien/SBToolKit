using System;
using System.Globalization;
using System.Windows;

namespace SBToolKit.WPF.ValueConverters
{
    public class AttachedPropertiesValueConverter : BaseValueConverter<AttachedPropertiesValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Hidden : Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
