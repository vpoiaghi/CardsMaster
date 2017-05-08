using System;

namespace CardMasterCard.Card
{
    public class Texture
    {
        public String Name { get; set; } = null;
        public String Color { get; set; } = null;

        public Texture()
        { }

        public Texture(String name)
        {
            this.Name = name;
        }

        public Texture(String name, String color)
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
