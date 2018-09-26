using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Data;

namespace CardMasterCommon.Converters
{

    public abstract class ListConverter<T> : IValueConverter
    {
        protected abstract T GetItem(string value);

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (IsList(value))
            {
                return ListToString((IEnumerable)value);
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((value == null) || (IsList(value)))
            {
                return value;
            }
            else
            {
                return StringToList(value.ToString());
            }
        }

        private bool IsList(object value)
        {
            bool isList = false;

            if (value != null)
            {
                IEnumerable<Type> interfaceTypes = ((System.Reflection.TypeInfo)(value.GetType())).ImplementedInterfaces;
                Type iListType = typeof(IList);

                foreach (Type interfaceType in interfaceTypes)
                {
                    isList |= (interfaceType == iListType);
                }
            }

            return isList;
        }

        private string ListToString(IEnumerable enumerableValue)
        {
            string strResult = "";
            bool firstLine = true;

            foreach (T item in enumerableValue)
            {
                if (firstLine)
                {
                    strResult += item.ToString();
                    firstLine = false;

                }
                else
                {
                    strResult += Environment.NewLine + item.ToString();
                }

            }

      

            return strResult;
        }

        private List<T> StringToList(string value)
        {
            var result = new List<T>();

            string[] stringParts = value.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in stringParts)
            {
                result.Add(GetItem(s));
            }

            return result;
        }
    }
}
