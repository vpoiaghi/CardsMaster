using System.IO;

namespace CardMasterSkin.Skins
{
    public class Skin
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public DirectoryInfo ResourcesDirectory { get; set; }
        public SkinElements Elements { get; set; }


        protected Skin()
        { }

        public Skin(int width, int height, DirectoryInfo resourcesDirectory)
        {
            this.Width = width;
            this.Height = height;
            this.ResourcesDirectory = resourcesDirectory;
            this.Elements = new SkinElements();
        }

        ~Skin()
        {
            this.ResourcesDirectory = null;
            this.Elements.Clear();
        }

    }
}
