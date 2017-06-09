using CardMasterImageBuilder.SkinElements;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace CardMasterImageBuilder.Elements.TextFormater
{
    class TElements
    {
        private Dictionary<string, bool> markers = new Dictionary<string, bool>()
        {
            { "b", false },
            { "u", false },
            { "i", false }
        };

        private SETextArea2 owner = null;
        private TElement firstElement = null;
        private TElement lastElement = null;

        public TElements(SETextArea2 owner)
        {
            this.owner = owner;
        }

        public SETextArea2 TextArea { get { return this.owner; } }

        public void Clear()
        {
            firstElement = null;
            lastElement = null;
        }

        public void Add(string word)
        {
            AddItem(word);
        }

        public void AddRange(List<string> words)
        {
            foreach (string word in words)
            {
                AddItem(word);
            }


            int maxHeight = this.owner.Height;
            int oldAllTextheight = TextAlignment.GetTextHeight(this);
            int newAllTextheight = 0;

            bool canReduceFont = (oldAllTextheight > maxHeight);

            while (canReduceFont)
            {
                // return true if min font size is not reached
                canReduceFont = this.TextArea.ReduceFontSize();

                // get new all text height
                newAllTextheight = TextAlignment.GetTextHeight(this);

                // test if can reduce one more time
                canReduceFont = canReduceFont && (newAllTextheight > maxHeight) && (oldAllTextheight > newAllTextheight);

                // save new height
                oldAllTextheight = newAllTextheight;
            }

            if (oldAllTextheight > maxHeight)
            {
                throw new TextSizeException("La taille du texte est trop grande pour le composant " + owner.Comments);
            }

        }

        public void ChangeGlobalFont()
        {
            this.firstElement.GlobalFont = this.TextArea.TextFont;
        }

        private void AddItem(string word)
        {
            if (! ((this.lastElement == null) || (this.lastElement is TElementReturn)))
            {
                AddSpace();
            }

            if (word.StartsWith("<") && word.EndsWith(">"))
            {
                AddMarker(word);
            }
            else
            {
                if ((TextArea.TextAlign == HorizontalAlignment.LeftWithIcon) && ((this.lastElement == null) || (this.lastElement is TElementReturn)))
                {
                    AddSpace(this.TextArea.LeftIconsSize.Width);
                }

                AddWord(word);
            }
        }

        private void AddWord(string word)
        {
            AddText(new TElementText(word));
        }

        private void AddSpace()
        {
            AddText(new TElementSpace());
        }

        private void AddSpace(int width)
        {
            AddText(new TElementSpace(width));
        }

        private void AddText(TElementText element)
        {
            if (this.markers["b"])
            {
                ApplyFontStyle(element, FontStyle.Bold);
            }

            if (this.markers["i"])
            {
                ApplyFontStyle(element, FontStyle.Italic);
            }

            if (this.markers["u"])
            {
                ApplyFontStyle(element, FontStyle.Underline);
            }

            AddElement(element);
        }

        private void ApplyFontStyle(TElementText element, FontStyle style)
        {
            if (element.FontStyle == null)
            {
                element.FontStyle = style;
            }
            else
            {
                element.FontStyle |= style;
            }
        }

        private void AddMarker(string word)
        {
            // Suppression des balises < et >
            word = word.Substring(1, word.Length - 2);

            if (word.StartsWith("<") && word.EndsWith(">"))
            {
                string fileName = word.Substring(1, word.Length - 2);
                AddImage(fileName);
            }
            else
            {
                TElement element = null;

                if (word.Equals("br/"))
                {
                    element = new TElementReturn();
                    AddElement(element);
                }
                else
                {
                    if (word.StartsWith("/"))
                    {
                        markers[word.Substring(1)] = false;
                    }
                    else
                    {
                        markers[word] = true;
                    }
                }
            }
        }

        private void AddImage(string fileName)
        {
            TElementImage element = new TElementImage(owner.GetResourceImage(fileName));

            if ((TextArea.TextAlign == HorizontalAlignment.LeftWithIcon) && ((this.lastElement == null) || (this.lastElement is TElementReturn)))
            {
                element.ImageSize = this.TextArea.LeftIconsSize;
            }

            AddElement(element);
        }

        private void AddElement(TElement element)
        {
            element.Elements = this;
            element.GlobalFont = this.TextArea.TextFont;

            if (this.firstElement == null)
            {
                this.firstElement = element;
            }
            else
            {
                this.lastElement.NextElement = element;
            }

            this.lastElement = element;
        }

        public List<TElement>.Enumerator GetEnumerator()
        {
            List<TElement> lst = new List<TElement>();

            TElement element = this.firstElement;

            while (element != null)
            {
                lst.Add(element);
                element = element.NextElement;
            }

            return lst.GetEnumerator();
        }
    }
}
