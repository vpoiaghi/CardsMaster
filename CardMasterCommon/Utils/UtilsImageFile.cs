using System;
using System.Drawing;
using System.IO;

namespace CardMasterCommon.Utils
{
    public static class UtilsImageFile
    {

        public static Image LoadImage(string imageFilePath)
        {
            return LoadImage(new FileInfo(imageFilePath));
        }

        public static Image LoadImage(FileInfo imageFile)
        {
            Image img = null;

            if ((imageFile != null) && (imageFile.Exists))
            {
                FileStream fs = new FileStream(imageFile.FullName, FileMode.Open, FileAccess.Read);
                img = Image.FromStream(fs);
                fs.Close();
                fs.Dispose();

                float dpiX = img.HorizontalResolution;
                float dpiY = img.VerticalResolution;

                img = new Bitmap(img);
                ((Bitmap)img).SetResolution(dpiX, dpiY);
            }

            return img;
        }

        public static bool SaveImage(Image image, string imageFilePath)
        {
            return SaveImage(image, new FileInfo(imageFilePath));
        }

        public static bool SaveImage(Image image, FileInfo imageFile)
        {
            bool savedWithSuccess = false;

            if ((imageFile != null) && (image != null))
            {
                try
                {
                    DirectoryInfo parentDir = imageFile.Directory;

                    if (!parentDir.Exists)
                    {
                        parentDir.Create();
                    }

                    if (imageFile.Extension == ".png")
                    {
                        image.Save(imageFile.FullName, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    else if (imageFile.Extension == ".jpg")
                    {
                        image.Save(imageFile.FullName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }

                    savedWithSuccess = true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Une erreur s'est prosuite lors de l'enregistrement de l'image \n" + imageFile.FullName, ex);
                }
            }

            return savedWithSuccess;
    
        }
    }
}
