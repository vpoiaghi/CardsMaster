using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace CardMasterManager.Converters
{
    public class DrawingImageToImageSourceConverter : System.Windows.Data.IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BitmapImage ImgSource = null;

            Image image = value as Image;

            if (image != null)
            {
                ImgSource = new BitmapImage();
                ImgSource.BeginInit();

                ImgSource.StreamSource = new MemoryStream(ConvertImageToByteArray(image));

                ImgSource.EndInit();
            }

            return ImgSource;
        }

        /// <summary>
        /// Convert a System.Drawing.Image to a byte array
        /// </summary>
        /// <param name="img">The System.Drawing.Image to convert in a byte array</param>
        /// <returns>A byte array</returns>
        private byte[] ConvertImageToByteArray(Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            return ms.ToArray();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }


}
