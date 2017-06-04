using System;
using System.Drawing;

namespace CardMasterImageBuilder.Converters
{
    public class CustomColorConverter
    {
        private static ColorConverter colorConverter;
        private static CustomColorConverter instance;

        private CustomColorConverter() { }

        public static CustomColorConverter Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomColorConverter();
                    colorConverter = new ColorConverter();
                }
                return instance;
            }
        }

        public Color ConvertFromString(String color)
        {
            return (Color)colorConverter.ConvertFromString(color);
        }
    }
}
