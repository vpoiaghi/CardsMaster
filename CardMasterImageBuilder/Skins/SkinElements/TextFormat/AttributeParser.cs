using CardMasterCard.Card;
using CardMasterImageBuilder.Converters;
using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CardMasterImageBuilder.Elements.TextFormat
{
    class AttributeParser
    {
        //@Team@ :  @Nature@ (@Element@)”;
        public static String Parse(CardMasterCard.Card.Card card, String format,String defaultText)
        {
            String fullText= "";
            if (!string.IsNullOrEmpty(format))
            {
                if(!format.Contains("@"))
                {
                    fullText = ExtractSingleAttribute(card, format);
                }else
                {
                    fullText = ExtractCompoundedAttribute(card, format);
                }
            }
            else
            {
                fullText = defaultText;
            }
            return fullText;

        }

        private static string ExtractCompoundedAttribute(Card card, string format)
        {

            MatchCollection result = Regex.Matches(format, "(@[^@]+@)");
            foreach (var r in result)
             {
               
                String token = r.ToString();
                String attribute = token.Replace("@", "");
                format = format.Replace(token, ExtractSingleAttribute(card,attribute));
            }
            if (format.Contains("%"))
            {
                String resultOptionalS = Regex.Matches(format, "(%[^%]+%)")[0].ToString();
                MatchCollection resultOptionalAttribute = Regex.Matches(resultOptionalS, @"[a-zA-Z]+");
                if (resultOptionalAttribute.Count > 0)
                {
                    String attributeOptionalS = resultOptionalAttribute[0].ToString();
                    int count = CCount(format, attributeOptionalS);
                    if (count > 1)
                    {
                        format = format.Replace(resultOptionalS, "");
                    }
                }
                else
                {
                    format = format.Replace(resultOptionalS, "");
                }
                format = format.Replace("%", "");
            }
            return format;
        }

        private static int CCount(String haystack, String needle)
        {
            return haystack.Split(new[] { needle }, StringSplitOptions.None).Length - 1;
        }

        private static string ExtractSingleAttribute(Card card, string format)
        {
            String fullText = "";
            Type cardType = card.GetType();
            PropertyInfo cardProperty = cardType.GetProperty(format);
            MethodInfo cardGetMethod = cardProperty.GetGetMethod();
            object propertyValue = cardGetMethod.Invoke(card, null);

            if (propertyValue != null)
            {
                var converter = new PowersListConverter();
                fullText = (string)converter.Convert(propertyValue, typeof(string), null, System.Globalization.CultureInfo.CurrentCulture);
            }
            return fullText;
        }

    }
}
