using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMasterManager.Utils
{
    public class CardMatcher
    {
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
                    IntEquals(card.IntField1,filterText) ||
                    IntEquals(card.IntField2, filterText) ||
                    IntEquals(card.IntField3, filterText) ||
                    IntEquals(card.IntField4, filterText) ||
                    StrContains(card.Team, filterText);
            return found;
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
