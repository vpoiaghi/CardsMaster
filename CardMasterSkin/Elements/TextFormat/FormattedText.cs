using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace CardMasterSkin.Elements.TextFormat
{
    class FormattedText
    {
        private const int SPACE_BETWEEN_ICON_AND_TEXT = 5;

        private string text = null;
        private List<TextSection> sectionsList = null;

        internal FormattedText(string text)
        {
            this.text = text;
            this.sectionsList = GetSections(text);
        }

        public List<TextSection> Sections { get { return this.sectionsList; } }

        private List<TextSection> GetSections(string text)
        {
            var sectionsList = new List<TextSection>();

            if (!string.IsNullOrEmpty(text))
            {
                string[] txtArray = text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (string txt in txtArray)
                {
                    sectionsList.Add(new TextSection(txt));
                }
            }


            return sectionsList;
        }

        internal void Format(Graphics g, DirectoryInfo SymbolsDirectory, HorizontalAlignment HAlignment, VerticalAlignment VAlignment, Font TextFont, StringFormat TextFormat, int MaxWidth, int MaxHeight, int wordSpaceOffsetX, int rowSpaceOffsetY)
        {
            LoadSymbols(SymbolsDirectory);

            int hAlignLeftWithIconOffsetX = GetLeftWithIconOffsetX(g, HAlignment, TextFont, TextFormat, MaxWidth);

            int previousSectionBottom = 0;

            foreach (TextSection section in this.sectionsList)
            { 
                section.Format(g, TextFont, TextFormat, MaxWidth, hAlignLeftWithIconOffsetX, wordSpaceOffsetX, rowSpaceOffsetY);
                section.Bounds = new Rectangle(section.Bounds.X, previousSectionBottom, section.Bounds.Width, section.Bounds.Height);
                previousSectionBottom = section.Bounds.Bottom + rowSpaceOffsetY;
            }
        }

        private void LoadSymbols(DirectoryInfo SymbolsDirectory)
        {
            foreach(TextSection section in this.sectionsList)
            {
                section.LoadSymbols(SymbolsDirectory);
            }
        }

        private int GetLeftWithIconOffsetX(Graphics g, HorizontalAlignment HAlignment, Font TextFont, StringFormat TextFormat, int MaxWidth)
        {
            int offsetX = 0;

            if (HAlignment == HorizontalAlignment.LeftWithIcon)
            {
                TextElement element = null;
                int w = 0;

                foreach (TextSection section in sectionsList)
                {
                    element = section.GetTextElementAt(0);

                    if(element.IsImage)
                    {
                        w = (int)element.GetSize(g, TextFont, TextFormat, MaxWidth).Width;

                        if (w > offsetX)
                        {
                            offsetX = w;
                        }
                    }
                }

                if (offsetX > 0)
                {
                    offsetX += SPACE_BETWEEN_ICON_AND_TEXT;
                }
            }

            return offsetX;
        }
    }
}
