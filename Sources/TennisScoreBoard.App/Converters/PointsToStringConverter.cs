using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using TennisScoreBoard.ScoreManager.Common;

namespace TennisScoreBoard.App.Converters
{
    public class PointsToStringConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;    
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var succeed = Enum.TryParse<POINTS>(value.ToString(), out var points);
            var retVal = string.Empty;
            if (succeed)
            {
                switch (points)
                {
                    case POINTS.Love:
                        retVal = "00";
                        break;
                    case POINTS.Fifteen:
                        retVal = "15";
                        break;
                    case POINTS.Thirty:
                        retVal = "30";
                        break;
                    case POINTS.Fourty:
                        retVal = "40";
                        break;
                    case POINTS.Advantage:
                        retVal = "A";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
