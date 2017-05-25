using CardMasterCard.Card;
using CardMasterSkin.GraphicsElements;
using CardMasterSkin.Skins;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace CardMasterSkin.Elements
{
    public class SECircle : SkinElement
    {
        public SECircle(Skin skin, int x, int y, int radius) : base(skin, x, y, radius * 2, radius * 2)
        { }

        public SECircle(Skin skin, int x, int y, int width, int height) : base(skin, x, y, width, height)
        { }

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
