using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;
using System.Drawing;

namespace CardMasterImageBuilder.GraphicsElements
{
    public abstract class GraphicElement
    {
        public abstract void DrawShadow(Graphics g, SkinElementShadow shadow);
        public abstract void Draw(Graphics g);
        public abstract void DrawBorder(Graphics g, SkinElementBorder border);

    }
}
