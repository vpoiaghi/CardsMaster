using System.Drawing;
using CardMasterSkin.Skins;

namespace CardMasterSkin.GraphicsElements
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
            g.DrawImage(this.image, this.rectangle);
        }

        public override void DrawShadow(Graphics g, SkinShadow shadow)
        { }

    }
}
