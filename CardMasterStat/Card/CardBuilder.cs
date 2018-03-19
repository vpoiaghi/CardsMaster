using CardMasterCard.Card;
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

        public static CardBuilder NewCard()
        {
            return new CardBuilder();
        }

        public Card Build()
        {
            return this.m_card;
        }

        public CardBuilder WithName(String name)
        {
            this.m_card.Name = name;
            return this;
        }

        public CardBuilder WithKind(String kind)
        {
            this.m_card.Kind = Card.ParseKind(kind);
            return this;
        }

        public CardBuilder WithRank(String rank)
        {
            this.m_card.Rank = rank;
            return this;
        }

        public CardBuilder WithTeam(String team)
        {
            this.m_card.Team = team;
            return this;
        }

        public CardBuilder WithNature(Card.NatureCard nature)
        {
            this.m_card.Nature = nature;
            return this;
        }

        public CardBuilder WithNature(String nature)
        {
            this.m_card.Nature = Card.ParseNature(nature);
            return this;
        }

        public CardBuilder WithElement(String element)
        {
            this.m_card.Element = element;
            return this;
        }

        public CardBuilder WithCost(int cost)
        {
            this.m_card.Cost = cost;
            return this;
        }

        public CardBuilder WithCost(String cost)
        {
          
            this.m_card.Cost = GetIntValue(cost);
            return this;
        }

        private int GetIntValue(String value)
        {
            int intCost=0;
            try
            {
                intCost = int.Parse(value);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
               
            }
            return intCost;
        }
        public CardBuilder WithAttack(int attack)
        {
            this.m_card.Attack = attack;
            return this;
        }

        public CardBuilder WithAttack(String attack)
        {
            this.m_card.Attack = GetIntValue(attack);
            return this;
        }

        public CardBuilder WithDefense(int defense)
        {
            this.m_card.Defense = defense;
            return this;
        }

        public CardBuilder WithDefense(String defense)
        {
            this.m_card.Defense = GetIntValue(defense);
            return this;
        }


        public CardBuilder WithCitation(String citation)
        {
            this.m_card.Citation = citation;
            return this;
        }
        public CardBuilder WithNb(int? nb)
        {
            this.m_card.Nb = nb;
            return this;
        }

        public CardBuilder WithComment(String comments)
        {
            this.m_card.Comments = comments;
            return this;
        }

        public CardBuilder WithBackground(JsonTexture texture)
        {
            this.m_card.Background = texture;
            return this;
        }

        public CardBuilder WithPowers(List<JsonPower> power)
        {
            this.m_card.Powers = power;
            return this;
        }

        public CardBuilder WithPowerJson(JsonPower power)
        {
            this.m_card.Powers.Add(power);
            return this;
        }
    }
}
