using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace CardMasterImageBuilder.GraphicsElements
{
    public class PathElement : GraphicElement
    {
        private GraphicsPath path = null;
        private Brush background = null;
        private bool withFontBorder;

        public PathElement(GraphicsPath path, Brush background)
        {
            this.path = path;
            this.background = background;
            this.withFontBorder = false;
        }

        public PathElement(GraphicsPath path, Brush background, bool withFontBorder)
        {
            this.path = path;
            this.background = background;
            this.withFontBorder = withFontBorder;
        }

        ~PathElement()
        {
            this.path = null;
            this.background = null;
        }

        public override void Draw(Graphics g)
        {
            g.FillPath(this.background, this.path);
            if (withFontBorder)
            {
                g.DrawPath(Pens.Black, this.path);
            }
        }

        public override void DrawShadow(Graphics g, SkinElementShadow shadow)
        {
            double radianAngle = shadow.Angle * Math.PI / 180;

            float transactionX = (float)Math.Cos(radianAngle) * shadow.Size;
            float transactionY = (float)Math.Sin(radianAngle) * shadow.Size;

            var translateMatrix = new Matrix();
            translateMatrix.Translate(transactionX, transactionY);

            var shadowPath = (GraphicsPath)this.path.Clone();
            shadowPath.Transform(translateMatrix);

            var b = new SolidBrush(Color.FromArgb(120, 0, 0, 0));

            g.FillPath(b, shadowPath);
        }

        public override void DrawBorder(Graphics g, SkinElementBorder border)
        {
            g.DrawPath(new Pen(border.BorderColor, border.BorderWidth), this.path);
        }

    }
}
