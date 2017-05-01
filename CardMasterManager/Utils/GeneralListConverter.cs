using CardMasterCard.Utils;
using System;
using System.Diagnostics;
using System.Collections;
using System.Windows.Data;

namespace CardMasterManager.Utils
{

    public class GeneralListConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Debug.WriteLine("=== back");
            return value;
        }

        public static string ListToString(IEnumerable list)
        {

            string result = "";
            Boolean firstLine = true;

            foreach (Object o in list)
            {
                if (firstLine)
                {
                    result += o.ToString();
                    firstLine = false;
                }
                else
                {
                    result += Environment.NewLine + o.ToString();
                }

            }

            return result;

        }

    }
}
