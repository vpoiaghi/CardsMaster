using CardMasterCard.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMasterStat
{
    class MockCards
    {
        public static List<Card> GetSamples()
        {
            List<Card> toReturn = new List<Card>();
            Random rnd = new Random();
            for (int i=0;i<50;i++)
            {
                Card c = CardBuilder.newCard().withName("Card #" + i)
                                    .withCost(rnd.Next(0,10))
                                    .withAttack(rnd.Next(0, 10))
                                    .withDefense(rnd.Next(0, 10))
                                    .build();
                toReturn.Add(c);
            }
            return toReturn;
        }
    }
}
