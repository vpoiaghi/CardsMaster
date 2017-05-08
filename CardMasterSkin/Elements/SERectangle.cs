using CardMasterCard.Card;
using CardMasterSkin.GraphicsElements;
using CardMasterSkin.Skins;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CardMasterSkin.Elements
{
    public class SERectangle : SkinElement
    {
        public SERectangle(Skin skin, int width, int height) : this(skin, 0, 0, width, height)
        { }

        public SERectangle(Skin skin, int x, int y, int width, int height) : base(skin, x, y, width, height)
        { }

        protected override List<GraphicElement> GetGraphicElements(Card card)
        {
            var graphicElementsList = new List<GraphicElement>();
            var path = new GraphicsPath();

            path.AddRectangle(new Rectangle(this.X, this.Y, this.Width, this.Height));

            graphicElementsList.Add(new PathElement(path, GetBackground()));

            path = null;

            return graphicElementsList;

        }

    }
}
