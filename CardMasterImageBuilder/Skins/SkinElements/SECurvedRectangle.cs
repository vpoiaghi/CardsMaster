using CardMasterCard.Card;
using CardMasterImageBuilder.GraphicsElements;
using CardMasterImageBuilder.Skins;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace CardMasterImageBuilder.SkinElements
{
    public class SECurvedRectangle : SkinElement
    {
        private int CurveSize { get; set; } = 0;

        public SECurvedRectangle(Skin skin, int x, int y, int width, int height, string comments, int curveSize) : base(skin, x, y, width, height, comments)
        {
            this.CurveSize = curveSize;
        }

        protected override List<GraphicElement> GetGraphicElements(JsonCard card)
        {
            var graphicElementsList = new List<GraphicElement>();
            var path = new GraphicsPath();

            int w = Width;
            int h = Height;
            int c = this.CurveSize;
            int d = 2 * c;

            // Début de la forme
            path.StartFigure();

            // Côté gauche
            path.AddArc(X, Y, d, h, 90, 180);
            // Côté haut
            path.AddLine(X + c, Y, X + w - c, Y);
            // Côté droit
            path.AddArc(X + w - d, Y, d, h, 270, 180);
            // Côté bas
            // Est généré par la fermeture de la forme

            // Fermeture de la forme
            path.CloseFigure();

            graphicElementsList.Add(new PathElement(path, GetBackground()));

            path = null;

            return graphicElementsList;

        }

    }
}
