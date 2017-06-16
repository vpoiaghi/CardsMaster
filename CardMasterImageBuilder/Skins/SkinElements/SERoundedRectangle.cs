using CardMasterCard.Card;
using CardMasterImageBuilder.GraphicsElements;
using CardMasterImageBuilder.Skins;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;

//
//         270
//          |
//          |
// 180 -----+----- 0
//          |
//          |
//          0
//

namespace CardMasterImageBuilder.SkinElements
{
    public class SERoundedRectangle : SkinElement
    {
        public int CornerRadius { get; set; } = 0;

        public SERoundedRectangle(DirectoryInfo resourcesDirectory, int x, int y, int width, int height, string comments, int cornerRadius) : base(resourcesDirectory, x, y, width, height, comments)
        {
            this.CornerRadius = cornerRadius;
        }
        protected override List<GraphicElement> GetGraphicElements(JsonCard card)
        {
            var graphicElementsList = new List<GraphicElement>();
            var path = new GraphicsPath();

            int x = this.X;
            int y = this.Y;
            int w = Width;
            int h = Height;
            int b = this.CornerRadius;
            int r = 2 * b;

            // début de la forme
            path.StartFigure();

            // Coin haut/gauche
            path.AddArc(x, y, r, r, 180, 90);
            // Côté haut
            path.AddLine(x + b, y, w - b, y);
            // Coin haut/droit
            path.AddArc(x + w - r, y, r, r, 270, 90);
            // Côté droit
            path.AddLine(x + w, y + b, x + w, y + h - b);
            // Coin bas/droit
            path.AddArc(x + w - r, y + h - r, r, r, 0, 90);
            // Côté bas
            path.AddLine(x + w - b, y + h, x + b, y + h);
            // Coin bas/gauche
            path.AddArc(x, y + h - r, r, r, 90, 90);
            // Côté gauche
            // Généré par la fermeture de la forme

            // Fermeture de la forme
            path.CloseFigure();

            graphicElementsList.Add(new PathElement(path, GetBackground(path)));

            path = null;

            return graphicElementsList;

        }

    }
}
