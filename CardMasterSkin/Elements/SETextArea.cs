using CardMasterCard.Card;
using CardMasterSkin.Elements.TextFormat;
using CardMasterSkin.GraphicsElements;
using CardMasterSkin.Skins;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace CardMasterSkin.Elements
{
    public enum HorizontalAlignment
    {
        Left,
        LeftWithIcon,
        Center,
        Right
    };

    public enum VerticalAlignment
    {
        Top,
        Center,
        Bottom
    };

    public class SETextArea : SkinElement
    {
        private const string DEFAULT_FONT_FAMILY = "Bell MT";
        private const float DEFAULT_FONT_SIZE = 14;
        private const FontStyle DEFAULT_FONT_STYLE = FontStyle.Bold;

        private const char WORDS_SEPARATOR = ' ';

        private string fullText = null;

        public string TextAttribute = null;
        public HorizontalAlignment TextAlign { get; set; } = HorizontalAlignment.Left;
        public VerticalAlignment TextVerticalAlign { get; set; } = VerticalAlignment.Top;
        public Font TextFont { get; set; } = new Font(DEFAULT_FONT_FAMILY, DEFAULT_FONT_SIZE, DEFAULT_FONT_STYLE);
        public int WordSpaceOffsetX { get; set; } = 0;

        public SETextArea(Skin skin, int x, int y, int width, int height, string text) : base(skin, x, y, width, height)
        {
            this.fullText = text;
            SetBackground(Color.Black);
        }

        protected override List<GraphicElement> GetGraphicElements(Card card)
        {
            List<GraphicElement> graphicElementsList = null;

            string fullText = AttributeParser.Parse(card, this.TextAttribute, this.fullText);

            if (!string.IsNullOrEmpty(fullText))
            {
                float textEmFontSize = this.graphics.DpiY * this.TextFont.Size / 72;
                StringFormat textFormat = StringFormat.GenericDefault;

                TextFormatter formatter = new TextFormatter(fullText);
                FormattedText formattedText = formatter.format(this.graphics, GetSymbolsDirectory(), this.TextAlign, this.TextVerticalAlign, this.TextFont, textFormat, this.Width, this.Height, this.WordSpaceOffsetX);
                
                graphicElementsList = GetGraphicElementsList(formattedText, this.TextFont, textEmFontSize, textFormat);

                formatter = null;
                formattedText = null;

            }
            else
            {
                graphicElementsList = new List<GraphicElement>();
            }

            return graphicElementsList;
        }

        private DirectoryInfo GetSymbolsDirectory()
        {
            DirectoryInfo symbolsDirectory = this.skin.TexturesDirectory;
            bool symbolsDirExists = (symbolsDirectory != null) && (symbolsDirectory.Exists);

            if (!symbolsDirExists)
            {
                throw new DirectoryNotFoundException();
            }

            return symbolsDirectory;
        }

        private List<GraphicElement> GetGraphicElementsList(FormattedText formattedText, Font textFont, float textEmFontSize, StringFormat textFormat)
        {
            var graphicElementsList = new List<GraphicElement>();
            GraphicsPath textPath = null;

            Rectangle r;

            List<TextSection> sectionsList = formattedText.Sections;

            foreach (TextSection section in sectionsList)
            {
                foreach (TextRow row in section.Rows)
                {
                    foreach (TextElement textElement in row.Elements)
                    {
                        r = new Rectangle(this.X + row.X + textElement.Bounds.X, this.Y + section.Bounds.Y + row.Y + textElement.Bounds.Y, textElement.Bounds.Width, textElement.Bounds.Height);

                        if (textElement.IsText)
                        {
                            if (textPath == null)
                            {
                                textPath = new GraphicsPath();
                                graphicElementsList.Add(new PathElement(textPath, GetBackground()));
                            }

                            textPath.AddString(textElement.Text, textFont.FontFamily, (int)textFont.Style, textEmFontSize, r, textFormat);

                            // Dessine le contour de la zone "au plus près" du texte --> A utiliser pour les tests de positionnement
                            //this.graphics.DrawRectangle(Pens.Black, r);
                        }
                        else
                        {
                            graphicElementsList.Add(new ImageElement(textElement.Image, r));
                        }
                    }
                }
            }

            return graphicElementsList;
        }
    }
}
