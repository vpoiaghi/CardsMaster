using CardMasterCard.Card;
using CardMasterImageBuilder.GraphicsElements;
using CardMasterImageBuilder.Skins;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace CardMasterImageBuilder.SkinElements
{
    public class SECircle : SkinElement
    {
        public SECircle(Skin skin, int x, int y, string comments, int radius) : base(skin, x, y, radius * 2, radius * 2, comments)
        { }

        public SECircle(Skin skin, int x, int y, int width, int height, string comments) : base(skin, x, y, width, height, comments)
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
