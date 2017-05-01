using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.ComponentModel;
using System.Reflection;
using System.Windows;

namespace CardMasterManager.Utils
{

    public class GeneralListConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string r = ListToString((IEnumerable)value);

            if (r == null || r.Length==0)
            {
                MessageBox.Show("vide");
            }

            return r;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            MessageBox.Show("ok");
            return null;

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
