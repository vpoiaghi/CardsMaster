using System.Collections.Generic;
using System.Drawing;

namespace CardMasterSkin.Elements.TextFormat
{
    class TextRow
    {
        public List<TextElement> Elements { get; set; } = new List<TextElement>();

        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;
        public int Bottom { get { return this.Y + this.Height; } }
        public int Right { get { return this.X + this.Width; } }

        public void AddElement(TextElement element)
        {
            this.Elements.Add(element);

            Rectangle elementBounds = element.Bounds;

            if (this.Height < elementBounds.Height)
            {
                this.Height = elementBounds.Height;
            }

            if (this.Width < elementBounds.Right)
            {
                this.Width = elementBounds.Right;
            }
        }
    }
}
