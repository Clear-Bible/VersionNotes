using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VersionNotes.Conversions
{
    public class WidthSubtraction : MarkupExtension, IValueConverter
    {
        private static WidthSubtraction _instance;

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var width = System.Convert.ToInt16(value) - System.Convert.ToInt16(parameter);
            //if (width < 0)
            //{
            //    width = 0;
            //}

            return width;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new WidthSubtraction());
        }
    }
}
