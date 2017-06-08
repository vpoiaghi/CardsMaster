using CardMasterCard.Card;
using CardMasterImageBuilder.GraphicsElements;
using CardMasterImageBuilder.Skins;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CardMasterImageBuilder.SkinElements
{
    public class SERectangle : SkinElement
    {
        public SERectangle(Skin skin, int x, int y, int width, int height, string comments) : base(skin, x, y, width, height, comments)
        { }

        protected override List<GraphicElement> GetGraphicElements(JsonCard card)
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
