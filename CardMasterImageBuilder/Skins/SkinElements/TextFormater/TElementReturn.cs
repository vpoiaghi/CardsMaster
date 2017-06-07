using CardMasterImageBuilder.GraphicsElements;
using System;
using System.Drawing;

namespace CardMasterImageBuilder.Elements.TextFormater
{
    class TElementReturn : TElement
    {
        public TElementReturn()
        { }

        public override void PrevBoundsChanged(int prevX, int prevY, int prevWidth, int prevHeight, bool globalFontChanged)
        {
            ChangeBound(0, GetMaxBottomOnRow(), null, null, globalFontChanged);
        }

        protected override Size GetSize(Graphics g)
        {
            return new Size(0, 0);
        }

        public override GraphicElement GetGraphicElement(Graphics g)
        {
            return null;
        }

    }
}
