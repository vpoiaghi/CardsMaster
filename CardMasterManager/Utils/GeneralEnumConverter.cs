using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Data;

namespace CardMasterManager.Utils
{

    public class GeneralEnumConverter : IValueConverter
    {
        private Type enumType = null;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value.GetType().IsEnum)
            {
                this.enumType = value.GetType();
                return this.getDescription(value);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value.GetType().IsEnum)
                return value;
            else
                return searchItemByName(value.ToString());

        }

        private string getDescription(object enumValue)
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return enumValue.ToString();

        }

        private object searchItemByName(string enumName)
        {
            Array enumValues = Enum.GetValues(enumType);

            foreach(object enumValue in enumValues)
                if (enumValue.ToString().Equals(enumName))
                    return enumValue;

            return null;
        }
    }
}
