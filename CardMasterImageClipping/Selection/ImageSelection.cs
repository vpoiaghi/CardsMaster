using CardMasterManager.Converters;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace CardMasterImageClipping.Selection
{
    class ImageSelection
    {
        public Rectangle SelRectangle { get; } = Rectangle.Empty;
        public Image SelImage { get; } = null;
        public BitmapImage SelBitmapImage { get; } = null;

        public ImageSelection(Rectangle selRectangle, Image selImage)
        {
            SelRectangle = selRectangle;
            SelImage = selImage;
            SelBitmapImage = ImageToBitmapImage(selImage);
        }

        private BitmapImage ImageToBitmapImage(Image img)
        {
            DrawingImageToImageSourceConverter conv = new DrawingImageToImageSourceConverter();
            return (BitmapImage)conv.Convert(img, null, null, null);
        }

    }
}
