using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace CardMasterImageClipping
{
    public class SmallImage
    {
        public BitmapImage Image { get; set; } = null;
        public Image SourceImage { get; set; } = null;
        public string Text { get; set; } = null;
        public FileInfo File { get; set; } = null;

        public SmallImage(FileInfo imageFile)
        {
            this.Image = new BitmapImage(new Uri(imageFile.FullName));
            this.SourceImage = new Bitmap(imageFile.FullName);
            this.Text = imageFile.Name;
            this.File = imageFile;
        }
    }
}
