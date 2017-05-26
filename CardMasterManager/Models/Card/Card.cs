using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CardMasterManager
{
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
        Physique,
        Equipement,
        Ninjutsu,
        Environnement,
        [Description("Lieu légendaire")] Zone
    }

    public class Card
    {
        public CardKind Kind { get; set; }
        public String Name { get; set; }
        public String Rank { get; set; }
        public String Team { get; set; }
        public NatureCard Nature { get; set; }
        public String Element { get; set; }
        public String Cost { get; set; }
        public String Attack { get; set; }
        public String Defense { get; set; }
        public String Citation { get; set; }
        public String Comments { get; set; }
        public int Nb { get; set; }
        public CardMasterCard.Card.Texture Background { get; set; }
        public List<CardMasterCard.Card.Power> Powers { get; set; }
        public String BackSide { get; set; }

        public static NatureCard parseNature(String nature)
        {
            NatureCard toReturn;
            NatureCard.TryParse(nature, out toReturn);
            if (nature.Equals("Lieu légendaire"))
                toReturn = NatureCard.Zone;
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

        public static CardMasterCard.Card.Card ConvertToMasterCard(Card card)
        {
            CardMasterCard.Card.Card toReturn = new CardMasterCard.Card.Card();
            toReturn.Attack = card.Attack.ToString();
            toReturn.Background = card.Background;
            toReturn.Chakra = card.Nature.ToString();
            toReturn.Citation = card.Citation;
            toReturn.Comments = card.Comments;
            toReturn.Cost = card.Cost.ToString();
            toReturn.Defense = card.Defense.ToString();
            toReturn.Element = card.Element;
            toReturn.Kind = card.Kind.ToString();
            toReturn.Name = card.Name;
            toReturn.Nb = card.Nb;
            toReturn.Powers = new List<CardMasterCard.Card.Power>();
            toReturn.Powers.AddRange(card.Powers);

            toReturn.Rank = card.Rank;
            toReturn.Team = card.Team;
            toReturn.BackSide = card.BackSide;
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
                                .withKind(sourceCard.Kind)
                                .withPowers(sourceCard.Powers)
                                .withRank(sourceCard.Rank)
                                .withNb(sourceCard.Nb)
                                .withTeam(sourceCard.Team)
                                .withPowers(sourceCard.Powers)
                                .withBackSide(sourceCard.BackSide)
                                .build();
            return toReturn;
        }

    }


}
