using CardMasterSkin.Skins;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CardMasterSkin.GraphicsElements
{
    public class PathElement : GraphicElement
    {
        private GraphicsPath path = null;
        private Brush background = null;

        public PathElement(GraphicsPath path, Brush background)
        {
            this.path = path;
            this.background = background;
        }

        ~PathElement()
        {
            this.path = null;
            this.background = null;
        }

        public override void Draw(Graphics g)
        {
            g.FillPath(this.background, this.path);
        }

        public override void DrawShadow(Graphics g, SkinShadow shadow)
        {
            if (shadow != null)
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
        }

    }
}
