using System.IO;

namespace CardMasterSkin.Skins
{
    public class Skin
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public DirectoryInfo ImagesDirectory { get; set; }
        public DirectoryInfo TexturesDirectory { get; set; }
        public SkinElements Elements { get; set; }


        protected Skin()
        { }

        public Skin(int width, int height, DirectoryInfo imagesDirectory, DirectoryInfo texturesDirectory)
        {
            this.Width = width;
            this.Height = height;
            this.ImagesDirectory = imagesDirectory;
            this.TexturesDirectory = texturesDirectory;
            this.Elements = new SkinElements();
        }

        ~Skin()
        {
            this.ImagesDirectory = null;
            this.TexturesDirectory = null;
            this.Elements.Clear();
        }

    }
}
