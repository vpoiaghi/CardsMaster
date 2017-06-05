using CardMasterImageBuilder.GraphicsElements;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CardMasterImageBuilder.Elements.TextFormater
{
    class TElementText : TElement
    {
        private string Word { get; set; }
        private FontStyle? fontStyle = null;
        private float? fontSize = null;
        private FontFamily fontFamily = null;


        public TElementText(string word)
        {
            this.Word = word;
        }

        public FontStyle? FontStyle
        {
            get
            {
                return this.fontStyle;
            }
            set
            {
                this.fontStyle = value;
                this.ResizeBounds(true);
            }
        }

        public float? FontSize
        {
            get
            {
                return this.fontSize;
            }
            set
            {
                this.fontSize = value;
                this.ResizeBounds(true);
            }
        }

        public FontFamily FontFamily
        {
            get
            {
                return this.fontFamily;
            }
            set
            {
                this.fontFamily = value;
                this.ResizeBounds(true);
            }
        }

        protected override Size GetSize(Graphics g)
        {
            if (g != null)
            {
                Font textFont = GetFont(g);
                int maxWidth = this.Elements.TextArea.Width;
                StringFormat textFormat = this.Elements.TextArea.TextFormat;

                return g.MeasureString(this.Word, textFont, maxWidth, textFormat).ToSize();
            }
            else
            {
                return Size.Empty;
            }
        }

        public override GraphicElement GetGraphicElement(Graphics g)
        {
            Font textFont = GetFont(g);
            StringFormat textFormat = this.Elements.TextArea.TextFormat;
            float textEmFontSize = g.DpiY * textFont.Size / 72;

            PointF origin = new PointF(this.Elements.TextArea.X + this.X, this.Elements.TextArea.Y + this.Y);

            GraphicsPath textPath = new GraphicsPath();
            textPath.AddString(this.Word, textFont.FontFamily, (int)textFont.Style, textEmFontSize, origin, textFormat);

            return new PathElement(textPath, new SolidBrush(Color.Black));
        }

        private Font GetFont(Graphics g)
        {
            FontFamily family = this.fontFamily == null ? globalFont.FontFamily : this.fontFamily;
            float size = this.fontSize == null ? globalFont.Size : this.fontSize.Value;

            FontStyle style;

            if (this.fontStyle == null)
            {
                style = globalFont.Style;
            }
            else
            {
                style = this.fontStyle.Value;
            }

            //GraphicsUnit graphicsPageUnit = g.PageUnit;

            return new Font(family, size, style);
        }
    }
}
