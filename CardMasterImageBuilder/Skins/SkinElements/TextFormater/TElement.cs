using CardMasterImageBuilder.GraphicsElements;
using CardMasterImageBuilder.SkinElements;
using System;
using System.Drawing;

namespace CardMasterImageBuilder.Elements.TextFormater
{
    abstract class TElement
    {
        private int x = 0;
        private int y = 0;
        private int width = 0;
        private int height = 0;

        private TElements elements = null;
        private TElement prevElement = null;
        private TElement nextElement = null;

        protected Font globalFont = null;

        protected abstract Size GetSize(Graphics g);
        public abstract GraphicElement GetGraphicElement(Graphics g);

        ~TElement()
        {
            prevElement = null;
            nextElement = null;
        }

        public TElements Elements
        {
            get
            {
                return this.elements;
            }
            set
            {
                this.elements = value;
                ResizeBounds(false);
            }
        }

        public Font GlobalFont
        {
            get
            {
                return this.globalFont;
            }
            set
            {
                this.globalFont = value;
                ResizeBounds(true);
            }
        }


        public int X { get { return this.x; } set { this.x = value; } }
        public int Y { get { return this.y; } set { this.y = value; } }
        public int Width { get { return this.width; } }
        public int Height { get { return this.height; } }
        public int Right { get { return this.x + this.width; } }
        public int Bottom { get { return this.y + this.height; } }

        public TElement PrevElement
        {
            get { return this.prevElement;  }
            set
            {
                if (value == null)
                {
                    this.prevElement = null;
                }
                else if (value != this.prevElement)
                {
                    this.prevElement = value;
                    value.NextElement = this;

                    ChangeBound(value.Right, value.Y, null, null, false);
                }
            }
        }

        public TElement NextElement
        {
            get { return this.nextElement; }
            set
            {
                if (value == null)
                {
                    this.nextElement = null;
                }
                else if (value != this.nextElement)
                {
                    this.nextElement = value;
                    value.PrevElement = this;

                    ApplyBoundsChangeToNextElement(false);
                }

            }
        }

        public virtual void PrevBoundsChanged(int prevX, int prevY, int prevWidth, int prevHeight, bool globalFontChanged)
        {
            int? newX = prevX + prevWidth;
            int? newY = prevY;
            int? newWidth = null;
            int? newHeight = null;

            if (globalFontChanged)
            {
                this.globalFont = this.elements.TextArea.TextFont;
                Size newSize = GetSize(GetGraphics());
                newWidth = newSize.Width;
                newHeight = newSize.Height;
            }

            ChangeBound(newX, newY, newWidth, newHeight, globalFontChanged);
        }

        protected void ResizeBounds(bool fontChanged)
        {
            if ((this.elements != null) && (this.globalFont != null))
            {
                Size newSize = GetSize(GetGraphics());
                ChangeBound(null, null, newSize.Width, newSize.Height, fontChanged);
            }
        }

        protected void ChangeBound(int? newX, int? newY, int? newWidth, int? newHeight, bool globalFontChanged)
        {
            bool somethingChanged = globalFontChanged;

            int tmpX = newX == null ? this.x : newX.Value;
            int tmpWidth = newWidth == null ? this.width : newWidth.Value;

            if ((tmpX + tmpWidth) > this.elements.TextArea.Width)
            {
                newX = this.Elements.TextArea.TextAlign == HorizontalAlignment.LeftWithIcon ? this.elements.TextArea.LeftIconsSize.Width : 0;
                newY = newY + this.height;
            }

            int tmpY = newY == null ? this.y : newY.Value;
            int tmpHeight = newHeight == null ? this.height : newHeight.Value;

            if ((tmpY + tmpHeight) > this.elements.TextArea.Height)
            {
                this.elements.TextArea.ReduceFontSize();
            }
            else
            {
                if ((newX != null) && (this.x != newX.Value))
                {
                    this.x = newX.Value;
                    somethingChanged = true;
                }

                if ((newY != null) && (this.y != newY.Value))
                {
                    this.y = newY.Value;
                    somethingChanged = true;
                }

                if ((newWidth != null) && (this.width != newWidth.Value))
                {
                    this.width = newWidth.Value;
                    somethingChanged = true;
                }

                if ((newHeight != null) && (this.height != newHeight.Value))
                {
                    this.height = newHeight.Value;
                    somethingChanged = true;
                }

                if (somethingChanged)
                {
                    ApplyBoundsChangeToNextElement(globalFontChanged);
                }
            }
        }

        private void ApplyBoundsChangeToNextElement(bool globalFontChanged)
        {
            if (this.NextElement != null)
            {
                this.NextElement.PrevBoundsChanged(this.x, this.y, this.width, this.height, globalFontChanged);
            }
        }

        private Graphics GetGraphics()
        {
            Graphics g = null;

            try
            {
                g = this.Elements.TextArea.Graphic;
            }
            catch (NullReferenceException)
            { }

            return g;
        }

        protected int GetMaxHeightOnRow()
        {
            int prevMax = GetPrevMaxHeightOnRow();
            int nextMax = 0;

            if (!(this is TElementReturn))
            {
                nextMax = GetNextMaxHeightOnRow();
            }
            

            return Math.Max(prevMax, nextMax);
        }
        
        public int GetPrevMaxHeightOnRow()
        {
            int max = this.height;

            if ((this.prevElement != null) && (! (this.prevElement is TElementReturn)) && (this.prevElement.Y == this.Y))
            {
                max = Math.Max(this.prevElement.GetPrevMaxHeightOnRow(), this.height);
            }

            return max;
        }

        public int GetNextMaxHeightOnRow()
        {
            int max = this.height;

            if ((this.nextElement != null) && (!(this.nextElement is TElementReturn)) && (this.nextElement.Y == this.Y))
            {
                max = Math.Max(this.nextElement.GetPrevMaxHeightOnRow(), this.height);
            }

            return max;
        }

    }
}