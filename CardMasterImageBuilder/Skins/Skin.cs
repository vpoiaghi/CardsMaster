using System.IO;

namespace CardMasterImageBuilder.Skins
{
    public class Skin
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public SkinElements Elements { get; set; }


        protected Skin()
        { }

        public Skin(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.Elements = new SkinElements();
        }

        ~Skin()
        {
            this.Elements.Clear();
        }

    }
}
