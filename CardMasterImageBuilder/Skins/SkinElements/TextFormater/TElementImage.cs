using CardMasterImageBuilder.GraphicsElements;
using System.Drawing;

namespace CardMasterImageBuilder.Elements.TextFormater
{
    class TElementImage : TElement
    {
        Image Image { get; set; } = null;
        public Size ImageSize { get; set; } = Size.Empty;

        public TElementImage(Image image)
        {
            this.Image = image;
        }

        protected override Size GetSize(Graphics g)
        {
            return ImageSize != Size.Empty ? this.ImageSize : this.Image.Size;
        }

        public override GraphicElement GetGraphicElement(Graphics g)
        {
            Point origin = new Point(this.Elements.TextArea.X + this.X, this.Elements.TextArea.Y + this.Y);
            Size size = new Size(this.Width, this.Height);

            return new ImageElement(this.Image, new Rectangle(origin, size));
        }
    }
}
