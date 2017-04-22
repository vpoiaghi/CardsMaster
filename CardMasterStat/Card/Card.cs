﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMasterStat
{
    public class Card
    {
        public String Kind { get; set; }
        public String Name { get; set; }
        public String Rank { get; set; }
        public String Team { get; set; }
        public NatureCard Nature { get; set; }
        public String Element { get; set; }
        public int Cost { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public String Citation { get; set; }
        public String Comments { get; set; }
        public CardMasterCard.Card.Texture Background { get; set; }
        public List<CardMasterCard.Card.Power> Powers { get; set; }

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
            Zone
        }

        public static Card.NatureCard parseNature(String nature)
        {
            NatureCard toReturn;
            NatureCard.TryParse(nature,out toReturn);
            if (nature.Equals("Lieu légendaire"))
                toReturn = NatureCard.Zone;
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
                                .withTeam(sourceCard.Team)
                                .build();
            return toReturn;
        }

    }


}
