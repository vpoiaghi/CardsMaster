using CardMasterImageBuilder.GraphicsElements;
using System;
using System.Drawing;

namespace CardMasterImageBuilder.Elements.TextFormater
{
    class TElementSpace : TElementText
    {
        private int fixedWidth = 0;

        public TElementSpace() : base(null)
        { }

        public TElementSpace(int fixedWidth) : base(null)
        {
            this.fixedWidth = fixedWidth;
        }

        public override void PrevBoundsChanged(int prevX, int prevY, int prevWidth, int prevHeight, bool globalFontChanged)
        {
            ChangeBound(prevX + prevWidth, prevY, null, prevHeight, globalFontChanged);
        }

        protected override Size GetSize(Graphics g)
        {
            Size s = base.GetSize(g);

            return new Size(fixedWidth, s.Height);
        }

        public override GraphicElement GetGraphicElement(Graphics g)
        {
            return null;
        }

        private int GetWidth()
        {
            return this.fixedWidth == 0 ? this.fixedWidth : this.Elements.TextArea.WordSpaceOffsetX;
        }
    }
}
