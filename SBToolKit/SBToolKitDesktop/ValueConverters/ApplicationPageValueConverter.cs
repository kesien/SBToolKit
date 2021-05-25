using SBToolKit.WPF.Models;
using SBToolKit.WPF.Pages;
using SBToolKitDesktop.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBToolKit.WPF.ValueConverters
{
    public class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (ApplicationPage)value switch
            {
                ApplicationPage.Login => new LoginPage(),
                ApplicationPage.Cloudlearning => new CloudlearningPage(),
                _ => null
            };

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
