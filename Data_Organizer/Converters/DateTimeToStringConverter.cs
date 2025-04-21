using System.Globalization;

namespace Data_Organizer.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dt)
                return dt.ToString("dd.MM.yyyy HH:mm");
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime.TryParse(value?.ToString(), out var dt);
            return dt;
        }
    }
}
