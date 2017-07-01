using System;
using System.Drawing;

namespace CardMasterImageClipping.Selection
{
    class RectangleD
    {
        public static readonly RectangleD Empty = new RectangleD(0, 0, 0, 0);

        public double X;
        public double Y;
        public double Width;
        public double Height;

        public RectangleD(double X, double Y, double Width, double Height)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle(
                (int)Math.Round(X),
                (int)Math.Round(Y),
                (int)Math.Round(Width),
                (int)Math.Round(Height));
        }

        public override string ToString()
        {
            return "{X=" + X + "; Y=" + Y + "; Width=" + Width + "; Height=" + Height + "}";
        }
    }
}
