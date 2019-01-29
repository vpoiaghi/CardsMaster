using CardMasterCard.Card;
using CardMasterCommon.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMasterManager.Utils
{
    public class CardMatcher
    {
        private static EnumConverter enumConverter = new EnumConverter();

        public static Boolean IsWarning(Card card)
        {
            if (card.Warning.HasValue)
            {
                return card.Warning.Value;
            }
            else
            {
                return false;
            }
          
        }

        public static Boolean Matches(Card card, String filterText)
        {
            bool found = StrContains(card.Name,filterText) ||
                    StrContains(card.Citation, filterText) ||
                    StrContains(card.Comments, filterText) ||
                    StrContains(card.Attack, filterText) ||
                    StrContains(card.Cost, filterText) ||
                    StrContains(card.Defense, filterText) ||
                    StrContains(card.Element, filterText) ||
                    StrContains(card.Rank, filterText) ||
                    StrContains(card.StringField1, filterText) ||
                    StrContains(card.StringField2, filterText) ||
                    StrContains(card.StringField3, filterText) ||
                    StrContains(card.StringField4, filterText) ||
                    StrContains(card.Kind,filterText) ||
                    StrContains(card.Nature, filterText) ||
                    IntEquals(card.IntField1,filterText) ||
                    IntEquals(card.IntField2, filterText) ||
                    IntEquals(card.IntField3, filterText) ||
                    IntEquals(card.IntField4, filterText) ||
                    PowerContains(card.Powers, filterText) ||
                    StrContains(card.Team, filterText);
            return found;
        }

        private static bool PowerContains(List<JsonPower> powers, string filterText)
        {
            bool found = false;
            foreach (JsonPower p in powers)
            {
                if (StrContains(p.Description, filterText))
                {
                    found = true;
                }
            }
            return found;
        }

        private static bool EnumContains(Enum enumeration, string filterText)
        {
           
            String result = (String)enumConverter.Convert(enumeration, null, null, null);
            return StrContains(result, filterText);
        }

        private static bool StrContains(string s, string search)
        {
            bool result = false;

            if ((s != null) && (search != null))
            {
                result = StringContains(s.ToLower(), search.ToLower());
            }

            return result;
        }

        private static bool IntEquals(int? i, String search)
        {
            int intSearch;
            bool isInt = int.TryParse(search, out intSearch);
            return (i != null && i.Equals(intSearch));
        }
        private static bool StringContains(String s, String search)
        {    
            return ((s != null && s.Contains(search)));
        }
    }
}
