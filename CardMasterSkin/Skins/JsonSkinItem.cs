using System;

namespace CardMasterSkin.Skins
{
    public class JsonSkinItem
    {
        public String Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int? Radius { get; set; }
        public String BackgroundColor { get; set; }
        public String NameAttribute { get; set; }
        public String Background { get; set; }
        public String Comment { get; set; }
        public int? CurveSize { get; set; }
        public String BorderColor { get; set; }
        public int? BorderWidth { get; set; }
        public String FontName { get; set; }
        public int? FontSize { get; set; }
        public String Style { get; set; }
        public String VerticalAlign { get; set; }
        public String HorizontalAlign { get; set; }
        public String VisibleConditionAttribute { get; set; }
        public int? shadowSize { get; set; }
        public int? shadowAngle { get; set; }
        public int? PowerIconWidth;
        public int? PowerIconHeight;
    }
}
