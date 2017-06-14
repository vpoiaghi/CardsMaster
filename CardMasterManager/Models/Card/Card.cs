using CardMasterCard.Card;
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
        public JsonTexture Background { get; set; }
        public List<JsonPower> Powers { get; set; }
        public String BackSide { get; set; }
        public String BackSkinName { get; set; }
        public String FrontSkinName { get; set; }
        //Custo
        public String StringField1 { get; set; }
        public String StringField2 { get; set; }
        public String StringField3 { get; set; }
        public String StringField4 { get; set; }

        public int? IntField1 { get; set; }
        public int? IntField2 { get; set; }
        public int? IntField3 { get; set; }
        public int? IntField4 { get; set; }

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
              

        public static JsonCard ConvertToMasterCard(Card card)
        {
            JsonCard toReturn = new JsonCard();
            toReturn.Attack = card.Attack;
            toReturn.Background = card.Background;
            toReturn.Chakra = card.Nature.ToString();
            toReturn.Citation = card.Citation;
            toReturn.Comments = card.Comments;
            toReturn.Cost = card.Cost;
            toReturn.Defense = card.Defense;
            toReturn.Element = card.Element;
            toReturn.Kind = card.Kind.ToString();
            toReturn.Name = card.Name;
            toReturn.Nb = card.Nb;
            toReturn.Powers = new List<JsonPower>();
            toReturn.Powers.AddRange(card.Powers);

            toReturn.Rank = card.Rank;
            toReturn.Team = card.Team;
            toReturn.BackSide = card.BackSide;
            toReturn.FrontSkinName = card.FrontSkinName;
            toReturn.BackSkinName = card.BackSkinName;
            //custo
            toReturn.StringField1 = card.StringField1;
            toReturn.StringField2 = card.StringField2;
            toReturn.StringField3 = card.StringField3;
            toReturn.StringField4 = card.StringField4;
            toReturn.IntField1 = card.IntField1;
            toReturn.IntField2 = card.IntField2;
            toReturn.IntField3 = card.IntField3;
            toReturn.IntField4 = card.IntField4;

            return toReturn;

        }

        public static Card ConvertCard(JsonCard card)
        {
            Card toReturn = new Card();
            toReturn.Attack = card.Attack.ToString();
            toReturn.Background = card.Background;
            toReturn.Nature = parseNature(card.Chakra);
            toReturn.Citation = card.Citation;
            toReturn.Comments = card.Comments;
            toReturn.Cost = card.Cost.ToString();
            toReturn.Defense = card.Defense.ToString();
            toReturn.Element = card.Element;
            toReturn.Kind = parseKind(card.Kind);
            toReturn.Name = card.Name;
            toReturn.Nb = card.Nb;
            toReturn.Powers = new List<JsonPower>();
            toReturn.Powers.AddRange(card.Powers);

            toReturn.Rank = card.Rank;
            toReturn.Team = card.Team;
            toReturn.BackSide = card.BackSide;
            toReturn.FrontSkinName = card.FrontSkinName;
            toReturn.BackSkinName = card.BackSkinName;
            //custo
            toReturn.StringField1 = card.StringField1;
            toReturn.StringField2 = card.StringField2;
            toReturn.StringField3 = card.StringField3;
            toReturn.StringField4 = card.StringField4;
            toReturn.IntField1 = card.IntField1;
            toReturn.IntField2 = card.IntField2;
            toReturn.IntField3 = card.IntField3;
            toReturn.IntField4 = card.IntField4;

            return toReturn;
        }

       


        

    }


}
