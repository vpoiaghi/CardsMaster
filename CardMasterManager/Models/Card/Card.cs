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

    public class Card : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private CardKind _kind;
        private String _name;
        private String _rank;
        private String _team;
        private NatureCard _nature;
        private String _element;
        private String _cost;
        private String _attack;
        private String _defense;
        private String _citation;
        private String _comments;
        private int _nb;
        private JsonTexture _background;
        private List<JsonPower> _powers;
        private String _backside;
        private String _backskinname;
        private String _frontskinname;
        private String _stringfield1;
        private String _stringfield2;
        private String _stringfield3;
        private String _stringfield4;
        private int? _intfield1;
        private int? _intfield2;
        private int? _intfield3;
        private int? _intfield4;

        public CardKind Kind { get { return _kind; } set { _kind = value; RaisePropertyChanged("Kind"); } }
        public String Name { get { return _name; } set { _name = value; RaisePropertyChanged("Name"); } }
        public String Rank { get { return _rank; } set { _rank = value; RaisePropertyChanged("Rank"); } }
        public String Team { get { return _team; } set { _team = value; RaisePropertyChanged("Team"); } }
        public NatureCard Nature { get { return _nature; } set { _nature = value; RaisePropertyChanged("Nature"); } }
        public String Element { get { return _element; } set { _element = value; RaisePropertyChanged("Element"); } }
        public String Cost { get { return _cost; } set { _cost = value; RaisePropertyChanged("Cost"); } }
        public String Attack { get { return _attack; } set { _attack = value; RaisePropertyChanged("Attack"); } }
        public String Defense { get { return _defense; } set { _defense = value; RaisePropertyChanged("Defense"); } }
        public String Citation { get { return _citation; } set { _citation = value; RaisePropertyChanged("Citation"); } }
        public String Comments { get { return _comments; } set { _comments = value; RaisePropertyChanged("Comments"); } }
        public int Nb { get { return _nb; } set { _nb = value; RaisePropertyChanged("Nb"); } }
        public JsonTexture Background { get { return _background; } set { _background = value; RaisePropertyChanged("Background"); } }
        public List<JsonPower> Powers { get { return _powers; } set { _powers = value; RaisePropertyChanged("Powers"); } }
        public String BackSide { get { return _backside; } set { _backside = value; RaisePropertyChanged("BackSide"); } }
        public String BackSkinName { get { return _backskinname; } set { _backskinname = value; RaisePropertyChanged("BackSkinName"); } }
        public String FrontSkinName { get { return _frontskinname; } set { _frontskinname = value; RaisePropertyChanged("FrontSkinName"); } }

        //Custo
        public String StringField1 { get { return _stringfield1; } set { _stringfield1 = value; RaisePropertyChanged("StringField1"); } }
        public String StringField2 { get { return _stringfield2; } set { _stringfield2 = value; RaisePropertyChanged("StringField2"); } }
        public String StringField3 { get { return _stringfield3; } set { _stringfield3 = value; RaisePropertyChanged("StringField3"); } }
        public String StringField4 { get { return _stringfield4; } set { _stringfield4 = value; RaisePropertyChanged("StringField4"); } }

        public int? IntField1 { get { return _intfield1; } set { _intfield1 = value; RaisePropertyChanged("IntField1"); } }
        public int? IntField2 { get { return _intfield2; } set { _intfield2 = value; RaisePropertyChanged("IntField2"); } }
        public int? IntField3 { get { return _intfield3; } set { _intfield3 = value; RaisePropertyChanged("IntField3"); } }
        public int? IntField4 { get { return _intfield4; } set { _intfield4 = value; RaisePropertyChanged("IntField4"); } }
        

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
