using System.Drawing;
using CardMasterSkin.Skins;
using System;
using System.Drawing.Drawing2D;
using CardMasterImageBuilder.Skins;

namespace CardMasterImageBuilder.GraphicsElements
{
    public class ImageElement : GraphicElement
    {
        private Image image;
        private Rectangle rectangle;

        public ImageElement(Image image, Rectangle rectangle)
        {
            this.image = image;
            this.rectangle = rectangle;
        }

        ~ImageElement()
        {
            this.image = null;
        }

        public override void Draw(Graphics g)
        {
            //Pourquoi une image peut être nulle ?? je sais pas
            if(this.image!=null) g.DrawImage(this.image, this.rectangle);
        }

        public override void DrawShadow(Graphics g, SkinElementShadow shadow)
        {
            double radianAngle = shadow.Angle * Math.PI / 180;

            float transactionX = (float)Math.Cos(radianAngle) * shadow.Size;
            float transactionY = (float)Math.Sin(radianAngle) * shadow.Size;

            var translateMatrix = new Matrix();
            translateMatrix.Translate(transactionX, transactionY);

            var shadowPath = new GraphicsPath();
            shadowPath.AddRectangle(new Rectangle(this.rectangle.Location, this.rectangle.Size));
            shadowPath.Transform(translateMatrix);

            var b = new SolidBrush(Color.FromArgb(120, 0, 0, 0));

            g.FillPath(b, shadowPath);
        }

        public override void DrawBorder(Graphics g, SkinElementBorder border)
        {
            g.DrawRectangle(new Pen(border.BorderColor, border.BorderWidth), this.rectangle);
        }

    }
}
