using CardMasterSkin.Skins;
using System.Drawing;

namespace CardMasterSkin.GraphicsElements
{
    public abstract class GraphicElement
    {
        public abstract void DrawShadow(Graphics g, SkinShadow shadow);
        public abstract void Draw(Graphics g);

    }
}
