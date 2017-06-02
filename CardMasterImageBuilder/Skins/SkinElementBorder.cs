using System.Drawing;

namespace CardMasterImageBuilder.Skins
{
    public class SkinElementBorder
    {
        public SkinElementBorder(Color color, int width)
        {
            this.BorderColor = color;
            this.BorderWidth = width;
        }

        public Color BorderColor { get; set; } = Color.Empty;
        public int BorderWidth { get; set; } = 0;

    }
}
