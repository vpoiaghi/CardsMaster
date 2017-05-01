using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.ComponentModel;
using System.Reflection;

namespace CardMasterManager.Utils
{

    public class GeneralListConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = "";
            Boolean firstLine = true;

            IEnumerable list = (IEnumerable)value;

            foreach (CardMasterCard.Card.Power p in list)
            {
                if (firstLine)
                {
                    result += p.ToString();
                    firstLine = false;
                }
                else
                {
                    result += Environment.NewLine + p.ToString();
                }

            }
                


            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        
    }
}
