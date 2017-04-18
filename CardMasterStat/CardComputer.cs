using CardMasterCard.Card;
using System.Collections.Generic;
using System.Linq;

namespace CardMasterStat
{
    class CardComputer
    {
        private List<Card> m_listCards;

        public CardComputer(List<Card> list)
        {
            this.m_listCards = list;
        }

        public SortedDictionary<int,int> GetRepartitionByCost()
        {
            SortedDictionary<int, int> toReturn = new SortedDictionary<int, int>();
            for (int i = 0; i < 10; i++)
            {
                toReturn.Add(i,m_listCards.Where(c => c.Cost == i).Count());

            }

            return toReturn;
        }
    }
}
