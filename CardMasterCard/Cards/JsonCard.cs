using System;
using System.Collections.Generic;

namespace CardMasterCard.Card
{
    public class JsonCard
    {
        public String Kind { get; set; }
        public String Name  { get; set; }
        public String Rank  { get; set; }
        public String Team  { get; set; }
        public String Chakra  { get; set; }
        public String Element  { get; set; }
        public String Cost  { get; set; }
        public String Attack  { get; set; }
        public String Defense  { get; set; }
        public String Citation  { get; set; }
        public String Comments  { get; set; }
        public int Nb { get; set; }
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

        public JsonTexture Background { get; set; }
        public List<JsonPower> Powers { get; set; }

        public JsonCard() : this(null)
        {
        }

        public JsonCard(String name)
        {
            this.Name = name;
            this.Powers = new List<JsonPower>();
        }

        ~JsonCard()
        {
            this.Powers.Clear();
            this.Powers = null;
            this.Background = null;
        }

        public override String ToString()
        {
            return this.Name;
        }

    }
}
