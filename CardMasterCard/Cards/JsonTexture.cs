using System;

namespace CardMasterCard.Card
{
    public class JsonTexture
    {
        public String Name { get; set; } = null;
        public String Color { get; set; } = null;

        public JsonTexture()
        { }

        public JsonTexture(String name)
        {
            this.Name = name;
        }

        public JsonTexture(String name, String color)
        {
            this.Name = name;
            this.Color = color;
        }

        public override String ToString()
        {
            return this.Name;
        }

    }
}
