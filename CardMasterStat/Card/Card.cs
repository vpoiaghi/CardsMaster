using CardMasterCard.Card;
using System;
using System.Collections.Generic;

namespace CardMasterStat
{
    public class Card
    {
        public CardKind Kind { get; set; }
        public String Name { get; set; }
        public String Rank { get; set; }
        public String Team { get; set; }
        public NatureCard Nature { get; set; }
        public String Element { get; set; }
        public int Cost { get; set; }
        public string Nb { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public String Citation { get; set; }
        public String Comments { get; set; }
        public Texture Background { get; set; }
        public List<Power> Powers { get; set; }

        public enum CardKind
        {
            Environnement,
            Equipement,
            Lieu,
            Ninja,
            Ninjutsu,
            Quête  
        }

        public enum NatureCard

        {
            Feu,
            Eau,
            Vent,
            Foudre,
            Terre,
            Special,
            Physique
        }

        public static NatureCard parseNature(String nature)
        {
            NatureCard toReturn;
            NatureCard.TryParse(nature,out toReturn);
            return toReturn;
        }

        public static CardKind parseKind(String kind)
        {
            CardKind toReturn;
            CardKind.TryParse(kind, out toReturn);
            if (kind.Equals("Lieu légendaire"))
                toReturn = CardKind.Lieu;
            return toReturn;
        }

        public static Card ConvertCard(CardMasterCard.Card.Card sourceCard)
        {
            Card toReturn = CardBuilder.newCard()
                                .withName(sourceCard.Name)
                                .withCost(sourceCard.Cost)
                                .withAttack(sourceCard.Attack)
                                .withBackground(sourceCard.Background)
                                .withNature(sourceCard.Chakra)
                                .withCitation(sourceCard.Citation)
                                .withComment(sourceCard.Comments)
                                .withDefense(sourceCard.Defense)
                                .withElement(sourceCard.Element)
                                .withNb(sourceCard.Nb)
                                .withKind(sourceCard.Kind)
                                .withPowers(sourceCard.Powers)
                                .withRank(sourceCard.Rank)
                                .withTeam(sourceCard.Team)
                                .build();
            return toReturn;
        }

    }


}
