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
        public int? Cost { get; set; }
        public int? Nb { get; set; }
        public int? Attack { get; set; }
        public int? Defense { get; set; }
        public String Citation { get; set; }
        public String Comments { get; set; }
        public JsonTexture Background { get; set; }
        public List<JsonPower> Powers { get; set; }

        public int Ratio
        {
            get{
                return ComputeRatio();
            }
        }

        private int ComputeRatio()
        {
            int ratio = 0;
            ratio += (Attack.Value);
            ratio += (Defense.Value);
            ratio = ratio * Powers.Count;
            return ratio;
        }

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
            Spécial,
            Physique
        }

        public static NatureCard ParseNature(String nature)
        {
            NatureCard.TryParse(nature, out NatureCard toReturn);
            return toReturn;
        }

        public static CardKind ParseKind(String kind)
        {
            CardKind.TryParse(kind, out CardKind toReturn);
            if (kind.Equals("Lieu légendaire"))
                toReturn = CardKind.Lieu;
            return toReturn;
        }

        public static Card ConvertCard(JsonCard sourceCard)
        {
            Card toReturn = CardBuilder.NewCard()
                                .WithName(sourceCard.Name)
                                .WithCost(sourceCard.Cost)
                                .WithAttack(sourceCard.Attack)
                                .WithBackground(sourceCard.Background)
                                .WithNature(sourceCard.Chakra)
                                .WithCitation(sourceCard.Citation)
                                .WithComment(sourceCard.Comments)
                                .WithDefense(sourceCard.Defense)
                                .WithElement(sourceCard.Element)
                                .WithNb(sourceCard.Nb)
                                .WithKind(sourceCard.Kind)
                                .WithPowers(sourceCard.Powers)
                                .WithRank(sourceCard.Rank)
                                .WithTeam(sourceCard.Team)
                                .Build();
            return toReturn;
        }

    }


}
