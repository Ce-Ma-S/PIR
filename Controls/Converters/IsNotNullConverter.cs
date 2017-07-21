using System;
using Windows.UI.Xaml.Data;

namespace Controls.Converters
{
    public class IsNotNullConverter :
        IValueConverter
    {
        public static IsNotNullConverter Instance = new IsNotNullConverter();

        public object Convert(object value, Type targetType, object parameter, string language) =>
            value != null;
        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            throw new NotImplementedException();
    }
}
