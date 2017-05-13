using System;
using System.Collections.Generic;

namespace CardMasterCard.Card
{
    public class Card
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
        public Texture Background { get; set; }
        public List<Power> Powers { get; set; }

        public Card() : this(null)
        {
        }

        public Card(String name)
        {
            this.Name = name;
            this.Powers = new List<Power>();
        }

        ~Card()
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
