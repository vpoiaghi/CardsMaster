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
        public String BackSkinName { get; set; }
        public String FrontSkinName { get; set; }

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
            toReturn.FrontSkinName = card.FrontSkinName;
            toReturn.BackSkinName = card.BackSkinName;
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
                                .withBackSkinName(sourceCard.BackSkinName)
                                .withFrontSkinName(sourceCard.FrontSkinName)
                                .build();
            return toReturn;
        }

        private class CardBuilder
        {
            private Card m_card;

            private CardBuilder()
            {
                m_card = new Card();
            }

            public static CardBuilder newCard()
            {
                return new CardBuilder();
            }

            public Card build()
            {
                return this.m_card;
            }

            public CardBuilder withName(String name)
            {
                this.m_card.Name = name;
                return this;
            }

            public CardBuilder withKind(String kind)
            {
                this.m_card.Kind = Card.parseKind(kind);
                return this;
            }

            public CardBuilder withRank(String rank)
            {
                this.m_card.Rank = rank;
                return this;
            }

            public CardBuilder withNb(int nb)
            {
                this.m_card.Nb = nb;
                return this;
            }

            public CardBuilder withTeam(String team)
            {
                this.m_card.Team = team;
                return this;
            }

            public CardBuilder withNature(string nature)
            {
                this.m_card.Nature = Card.parseNature(nature);
                return this;
            }

            public CardBuilder withElement(String element)
            {
                this.m_card.Element = element;
                return this;
            }


            public CardBuilder withCost(String cost)
            {

                this.m_card.Cost = cost;
                return this;
            }

            private int GetIntValue(String value)
            {
                int intCost = -1;
                try
                {
                    intCost = int.Parse(value);
                }
                catch (FormatException e)
                {
                    Console.Write(e.Message);
                    intCost = -1;
                }
                return intCost;
            }

            public CardBuilder withAttack(String attack)
            {
                this.m_card.Attack = attack;
                return this;
            }

            public CardBuilder withDefense(String defense)
            {
                this.m_card.Defense = defense;
                return this;
            }


            public CardBuilder withCitation(String citation)
            {
                this.m_card.Citation = citation;
                return this;
            }

            public CardBuilder withComment(String comments)
            {
                this.m_card.Comments = comments;
                return this;
            }

            public CardBuilder withBackSkinName(String skinName)
            {
                this.m_card.BackSkinName = skinName;
                return this;
            }

            public CardBuilder withFrontSkinName(String skinName)
            {
                this.m_card.FrontSkinName = skinName;
                return this;
            }

            public CardBuilder withBackground(CardMasterCard.Card.Texture texture)
            {
                this.m_card.Background = texture;
                return this;
            }

            public CardBuilder withPowers(List<CardMasterCard.Card.Power> powers)
            {
                this.m_card.Powers = powers;
                return this;
            }

            public CardBuilder withPower(CardMasterCard.Card.Power power)
            {
                this.m_card.Powers.Add(power);
                return this;
            }

            public CardBuilder withBackSide(String backSide)
            {
                this.m_card.BackSide = backSide;
                return this;
            }


        }

    }


}
