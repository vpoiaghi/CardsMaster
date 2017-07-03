using CardMasterCommon.Utils;
using CardMasterManager.Converters;
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

            this.SourceImage = UtilsImageFile.LoadImage(imageFile.FullName);
            this.Image = ImageToBitmapImage(this.SourceImage);
        }

        private BitmapImage ImageToBitmapImage(Image img)
        {
            DrawingImageToImageSourceConverter conv = new DrawingImageToImageSourceConverter();
            return (BitmapImage)conv.Convert(img, null, null, null);
        }
    }
}
