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
                                    .withNature(getRandomType(rnd))
                                    .build();
                toReturn.Add(c);
            }
            return toReturn;
        }

        private static Card.NatureCard getRandomType(Random rnd)
        {
            int i = rnd.Next(0, 8);
            switch (i){
                case 0:
                    return Card.NatureCard.Eau;
                case 1:
                    return Card.NatureCard.Equipement;
                case 2:
                    return Card.NatureCard.Feu;
                case 3:
                    return Card.NatureCard.Foudre;
                case 4:
                    return Card.NatureCard.Physique;
                case 5:
                    return Card.NatureCard.Special;
                case 6:
                    return Card.NatureCard.Terre;
                case 7:
                    return Card.NatureCard.Vent;
                default:
                    return Card.NatureCard.Eau;


            }
        }
    }
}
