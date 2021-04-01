using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using TennisScoreBoard.EF;
using TennisScoreBoard.EF.Model;

namespace TennisScoreBoard.App.Converters
{
    public class TennisPlayerToFullNameStringConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var player = (TennisPlayer) value;
            if (player != null)
            {
                return $"{player.FirstName} {player.LastName}";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
