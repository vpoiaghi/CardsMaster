using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace CardMasterSkin.Elements.TextFormat
{
    class TextFormatter
    {
        private FormattedText formattedText = null;

        public TextFormatter(string text)
        {
            this.formattedText = new FormattedText(text);
        }

        public FormattedText format(Graphics g, DirectoryInfo SymbolsDirectory, HorizontalAlignment HAlignment, VerticalAlignment VAlignment, Font TextFont, StringFormat TextFormat, 
            int MaxWidth, int MaxHeight, int wordSpaceOffsetX, int rowSpaceOffsetY)
        {
            if (!SymbolsDirectory.Exists)
            {
                throw new DirectoryNotFoundException();
            }
            if (TextFont == null)
            {
                throw new ArgumentNullException("FontText");
            }
            if (TextFormat == null)
            {
                throw new ArgumentNullException("TextFormat");
            }
            if (MaxWidth <= 0)
            {
                throw new ArgumentException("Le paramètre MaxWith ne peut être inférieur ou égal à zéro.", "MaxWidth");
            }

            this.formattedText.Format(g, SymbolsDirectory, HAlignment, VAlignment, TextFont, TextFormat, MaxWidth, MaxHeight, wordSpaceOffsetX, rowSpaceOffsetY);

            ApplyAlignments(this.formattedText.Sections, HAlignment, VAlignment, MaxWidth, MaxHeight);

            return this.formattedText;
        }

        private void ApplyAlignments(List<TextSection> sectionsList, HorizontalAlignment hAlignment, VerticalAlignment vAlignment, int MaxWidth, int MaxHeight)
        {
            switch (hAlignment)
            {
                case HorizontalAlignment.Left:
                    // rien à faire
                    break;
                case HorizontalAlignment.LeftWithIcon:
                    // rien à faire (déjà pris en compte en amont)
                    break;
                case HorizontalAlignment.Center:
                    ApplyHorizontalAlignementCenter(sectionsList, MaxWidth);
                    break;
                case HorizontalAlignment.Right:
                    ApplyHorizontalAlignementRight(sectionsList, MaxWidth);
                    break;
            }

            switch (vAlignment)
            {
                case VerticalAlignment.Top:
                    // Rien à faire
                    break;
                case VerticalAlignment.Center:
                    ApplyVerticalAlignementCenter(sectionsList, MaxHeight);
                    break;
                case VerticalAlignment.Bottom:
                    ApplyVerticalAlignementBottom(sectionsList, MaxHeight);
                    break;
            }
        }

        private void ApplyHorizontalAlignementCenter(List<TextSection> sectionsList, int MaxWidth)
        {
            foreach (TextSection section in sectionsList)
            {
                foreach (TextRow row in section.Rows)
                {
                    row.X = (int)Math.Round((double)(MaxWidth - row.Width) / 2);
                }
            }
        }

        private void ApplyHorizontalAlignementRight(List<TextSection> sectionsList, int MaxWidth)
        {
            foreach (TextSection section in sectionsList)
            {
                foreach (TextRow row in section.Rows)
                {
                    row.X = MaxWidth - row.Width;
                }
            }
        }

        private void ApplyVerticalAlignementCenter(List<TextSection> sectionsList, int MaxHeight)
        {
            foreach (TextSection section in sectionsList)
            {
                int rowsHeight = 0;
                int y = 0;

                foreach (TextRow row in section.Rows)
                {
                    rowsHeight += row.Height;
                }

                y = (int)Math.Round((double)(MaxHeight - rowsHeight) / 2);

                foreach (TextRow row in section.Rows)
                {
                    row.Y += y;
                }

            }
        }


        private void ApplyVerticalAlignementBottom(List<TextSection> sectionsList, int MaxHeight)
        {
            foreach (TextSection section in sectionsList)
            {
                int rowsHeight = 0;
                int y = 0;

                foreach (TextRow row in section.Rows)
                {
                    rowsHeight += row.Height;
                }

                y = MaxHeight - rowsHeight;

                foreach (TextRow row in section.Rows)
                {
                    row.Y += y;
                }

            }
        }
    }
}
