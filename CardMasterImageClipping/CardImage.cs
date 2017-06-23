using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace CardMasterImageClipping
{
    public class CardImage
    {
        public BitmapImage Image { get; set; } = null;
        public Image SourceImage { get; set; } = null;
        public string Text { get; set; } = null;
        public FileInfo File { get; set; } = null;

        public CardImage(FileInfo imageFile)
        {
            this.Text = imageFile.Name;
            this.File = imageFile;

            this.Image = new BitmapImage(new Uri(imageFile.FullName));
            this.SourceImage = GetImage(imageFile.FullName);
        }

        private Image GetImage(string fileFullname)
        {
            Bitmap img = new Bitmap(fileFullname);
            img.SetResolution(300, 300);

            return img;
        }
    }
}
