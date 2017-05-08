using CardMasterCard.Card;
using CardMasterSkin.GraphicsElements;
using CardMasterSkin.Skins;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace CardMasterSkin.Elements
{
    public class SECircle : SkinElement
    {
        int radius = 0;

        public SECircle(Skin skin, int radius) : this(skin, 0, 0, radius * 2, radius * 2, radius)
        { }

        public SECircle(Skin skin, int x, int y, int width, int height, int radius) : base(skin, x, y, width, height)
        {
            this.radius = radius;
        }

        protected override List<GraphicElement> GetGraphicElements(Card card)
        {
            var graphicElementsList = new List<GraphicElement>();
            var path = new GraphicsPath();

            path.AddEllipse(X, Y, Width, Height);

            graphicElementsList.Add(new PathElement(path, GetBackground()));
            
            path = null;

            return graphicElementsList;
        }

    }
}
