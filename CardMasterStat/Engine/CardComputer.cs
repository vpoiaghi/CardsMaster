
using CardMasterStat.Engine;
using System;
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

        public SortedDictionary<int, int> GetRepartitionByCost()
        {
            SortedDictionary<int, int> toReturn = new SortedDictionary<int, int>();
            for (int i = 0; i < 10; i++)
            {
                toReturn.Add(i, this.m_listCards.Where(c => c.Cost == i).Count());
            }
            return toReturn;
        }

        public SortedDictionary<int, int> GetRepartitionByAttack()
        {
            SortedDictionary<int, int> toReturn = new SortedDictionary<int, int>();
            for (int i = 0; i < 10; i++)
            {
                toReturn.Add(i, this.m_listCards.Where(c => c.Attack == i).Count());
            }
            return toReturn;
        }

        public SortedDictionary<int, int> GetRepartitionByDefense()
        {
            SortedDictionary<int, int> toReturn = new SortedDictionary<int, int>();
            for (int i = 0; i < 10; i++)
            {
                toReturn.Add(i, this.m_listCards.Where(c => c.Defense == i).Count());
            }
            return toReturn;
        }
        public List<CustomDot> GetRepartitionByRatio()
        {
           List<CustomDot> toReturn = new List<CustomDot>();
            foreach (Card c in m_listCards)
            {
                if (c.Cost.HasValue)
                {
                    CustomDot k = new CustomDot(c.Name,c.Cost.Value, c.Ratio);
                    toReturn.Add(k);
                }
                
            }
            return toReturn;
        }


        public SortedDictionary<String, int> GetRepartitionByNature()
        {
            SortedDictionary<String, int> toReturn = new SortedDictionary<String, int>();
            foreach (Card.CardKind kind in Enum.GetValues(typeof(Card.CardKind)))
            {
                if (kind.Equals(Card.CardKind.Ninja))
                {
                    foreach (Card.NatureCard nature in Enum.GetValues(typeof(Card.NatureCard)))
                    {
                        toReturn.Add(kind.ToString() + " " + nature.ToString(), this.m_listCards.Where(c => c.Kind == kind && c.Nature == nature).Count());
                    }
                        
                }
                else
                {
                    toReturn.Add(kind.ToString(), this.m_listCards.Where(c => c.Kind == kind).Count());
                }
               
            }
            return toReturn;
        }

        public int GetTotalCount()
        {
            return this.m_listCards.Count;
        }


    }
}
