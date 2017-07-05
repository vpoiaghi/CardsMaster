using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CardMasterStat
{
    public class ConvertToBrush : IValueConverter
    {
        static Random random = new Random(DateTime.Now.Millisecond);
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        { 
            Color color = Color.FromArgb(255, (byte)random.Next(255), (byte)random.Next(255), (byte)random.Next(255));
            return new SolidColorBrush(color);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
