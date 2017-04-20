
using System;
using System.Collections.Generic;

namespace CardMasterStat
{
    class CardBuilder
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
            this.m_card.Kind = kind;
            return this;
        }

        public CardBuilder withRank(String rank)
        {
            this.m_card.Rank = rank;
            return this;
        }

        public CardBuilder withTeam(String team)
        {
            this.m_card.Team = team;
            return this;
        }

        public CardBuilder withNature(Card.NatureCard nature)
        {
            this.m_card.Nature = nature;
            return this;
        }

        public CardBuilder withNature(String nature)
        {
            this.m_card.Nature = Card.parseNature(nature);
            return this;
        }

        public CardBuilder withElement(String element)
        {
            this.m_card.Element = element;
            return this;
        }

        public CardBuilder withCost(int cost)
        {
            this.m_card.Cost = cost;
            return this;
        }

        public CardBuilder withCost(String cost)
        {
          
            this.m_card.Cost = GetIntValue(cost);
            return this;
        }

        private int GetIntValue(String value)
        {
            int intCost=-1;
            try
            {
                intCost = int.Parse(value);
            }
            catch (FormatException e)
            {
                intCost = -1;
            }
            return intCost;
        }
        public CardBuilder withAttack(int attack)
        {
            this.m_card.Attack = attack;
            return this;
        }

        public CardBuilder withAttack(String attack)
        {
            this.m_card.Attack = GetIntValue(attack);
            return this;
        }

        public CardBuilder withDefense(int defense)
        {
            this.m_card.Defense = defense;
            return this;
        }

        public CardBuilder withDefense(String defense)
        {
            this.m_card.Defense = GetIntValue(defense);
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

        public CardBuilder withBackground(CardMasterCard.Card.Texture texture)
        {
            this.m_card.Background = texture;
            return this;
        }

        public CardBuilder withPowers(List<CardMasterCard.Card.Power> power)
        {
            this.m_card.Powers = power;
            return this;
        }

        public CardBuilder withPower(CardMasterCard.Card.Power power)
        {
            this.m_card.Powers.Add(power);
            return this;
        }
    }
}
