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
            bool found = StringContains(card.Name,filterText) ||
                    StringContains(card.Citation, filterText) ||
                    StringContains(card.Comments, filterText) ||
                    StringContains(card.Attack, filterText) ||
                    StringContains(card.Cost, filterText) ||
                    StringContains(card.Defense, filterText) ||
                    StringContains(card.Element, filterText) ||
                    StringContains(card.Rank, filterText) ||
                    StringContains(card.StringField1, filterText) ||
                    StringContains(card.StringField2, filterText) ||
                    StringContains(card.StringField3, filterText) ||
                    StringContains(card.StringField4, filterText) ||
                    IntEquals(card.IntField1,filterText) ||
                    IntEquals(card.IntField2, filterText) ||
                    IntEquals(card.IntField3, filterText) ||
                    IntEquals(card.IntField4, filterText) ||
                    StringContains(card.Team, filterText);
            return found;
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
