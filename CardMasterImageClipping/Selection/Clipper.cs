using System;
using System.Drawing;
using System.Windows;

namespace CardMasterImageClipping.Selection
{
    class SelectRectangle
    {
        public System.Windows.Controls.TextBlock txt { get; set; } = null;

        System.Windows.Controls.Image component = null;
        private double componentWidth = 0;
        private double componentHeight = 0;

        private double imageOnComponentWidth = 0;
        private double imageOnComponentHeight = 0;

        private double targetWidth = 0;
        private double targetHeight = 0;
        private double targetRatioW = 0;
        private double targetRatioH = 0;
        private int targetResolution;

        private double offsetX = 0;
        private double offsetY = 0;

        private double anchorX = -1;
        private double anchorY = -1;

        private Image realImage = null;

        public SelectRectangle(System.Windows.Controls.Image component, Image image, double targetWidth, double targetHeight, int targetResolution)
        {
            imageOnComponentWidth = component.ActualWidth;
            imageOnComponentHeight = component.ActualHeight;

            System.Windows.Point location = component.TranslatePoint(new System.Windows.Point(0, 0), (UIElement)component.Parent);
            offsetX = location.X;
            offsetY = location.Y;

            this.component = component;
            componentWidth = imageOnComponentWidth + 2 * offsetX;
            componentHeight = imageOnComponentHeight + 2 * offsetY;


            this.targetWidth = targetWidth;
            this.targetHeight = targetHeight;
            this.targetResolution = targetResolution;

            targetRatioW = targetWidth / targetHeight;
            targetRatioH = targetHeight / targetWidth;

            realImage = image;
        }

        public ImageSelection StartSelection(double mouseX, double mouseY)
        {
            this.anchorX = mouseX;
            this.anchorY = mouseY;

            return GetSelection(mouseX, mouseY);
        }

        public ImageSelection GetSelection(double mouseX, double mouseY)
        {

            // Calcul le rectangle de sélection brut.
            RectangleD rectangleOnComponent = GetRectangleOnComponent(mouseX, mouseY);

            // Calcul un nouveau rectangle respectant le ratio largeur/hauteur cible.
            RectangleD resizedRectangleOnComponent = GetResizedRectangleOnComponent(rectangleOnComponent);

            // Positionne le rectangle dans l'espace de l'image visible dans le composant.
            RectangleD rectangleOnComponentImage = GetRectangleOnComponentImage(resizedRectangleOnComponent);

            // Redimensionnement du rectangle au format de l'image réelle
            RectangleD rectangleOnRealImage = GetRectangleOnRealImage(rectangleOnComponentImage);

            // Découpe la sélection dans l'image visible dans le composant, et la passe en résolution cible.
            Image img = CutImagePart(rectangleOnRealImage);

            txt.TextAlignment = TextAlignment.Left;
            txt.Text = mouseX + "; " + mouseY + "\n"
                + rectangleOnComponent.ToString() + "\n"
                + resizedRectangleOnComponent.ToString() + "\n"
                + rectangleOnComponentImage.ToString() + "\n"
                + rectangleOnRealImage.ToString();

            return new ImageSelection(resizedRectangleOnComponent.GetRectangle(), img);
        }

        private RectangleD GetRectangleOnComponent(double mouseX, double mouseY)
        {
            double x = mouseX < offsetX ? offsetX : mouseX >= componentWidth - offsetX ? componentWidth-(offsetX+1) : mouseX;
            double y = mouseY < offsetY ? offsetY : mouseY >= componentHeight - offsetY ? componentHeight-(offsetY+1) : mouseY;

            double w = Math.Abs(x - anchorX);
            double h = Math.Abs(y - anchorY);

            double leftX = Math.Min(x, anchorX);
            double topY = Math.Min(y, anchorY);

            return new RectangleD(leftX, topY, w, h);
        }

        private RectangleD GetResizedRectangleOnComponent(RectangleD rectangleOnComponent)
        {
            RectangleD result = RectangleD.Empty;

            if ((rectangleOnComponent.Height * targetRatioW) <= rectangleOnComponent.Width)
            {
                result.Height = rectangleOnComponent.Height;
                result.Width = rectangleOnComponent.Height * targetRatioW;
                result.X = rectangleOnComponent.X < anchorX ? anchorX - result.Width : rectangleOnComponent.X;
                result.Y = rectangleOnComponent.Y;
            }
            else
            {
                result.Height = rectangleOnComponent.Width * targetRatioH;
                result.Width = rectangleOnComponent.Width;
                result.X = rectangleOnComponent.X;
                result.Y = rectangleOnComponent.Y < anchorY ? anchorY - result.Height : rectangleOnComponent.Y;
            }

            return result;
        }

        private RectangleD GetRectangleOnComponentImage(RectangleD rectangleOnComponent)
        {
            return new RectangleD(
                rectangleOnComponent.X - offsetX,
                rectangleOnComponent.Y - offsetY,
                rectangleOnComponent.Width,
                rectangleOnComponent.Height);
        }

        private RectangleD GetRectangleOnRealImage(RectangleD rectangleOnComponent)
        {
            return new RectangleD(
                rectangleOnComponent.X * realImage.Width / imageOnComponentWidth,
                rectangleOnComponent.Y * realImage.Height / imageOnComponentHeight,
                rectangleOnComponent.Width * realImage.Width / imageOnComponentWidth,
                rectangleOnComponent.Height * realImage.Height / imageOnComponentHeight);
        }

        private Image CutImagePart(RectangleD rectangleOnComponentImage)
        {
            Bitmap result = null;

            if (rectangleOnComponentImage.Width > 0 && rectangleOnComponentImage.Height > 0)
            {
                System.Drawing.Bitmap srcImage = (Bitmap)realImage;

                if (rectangleOnComponentImage.Width > 0 && rectangleOnComponentImage.Height > 0)
                {
                    result = new Bitmap((int)targetWidth, (int)targetHeight);
                    result.SetResolution(targetResolution, targetResolution);

                    Graphics g = Graphics.FromImage(result);

                    try
                    {
                        g.Clear(System.Drawing.Color.Black);
                        g.DrawImage(srcImage.Clone(rectangleOnComponentImage.GetRectangle(), srcImage.PixelFormat), 0, 0, (int)targetWidth, (int)targetHeight);
                    }
                    catch(Exception)
                    { }

                    g.Dispose();
                    g = null;
                }
            }

            return result;
        }
    }
}