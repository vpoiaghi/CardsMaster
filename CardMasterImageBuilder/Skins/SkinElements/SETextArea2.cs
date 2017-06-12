using CardMasterCard.Card;
using CardMasterImageBuilder.Elements.TextFormater;
using CardMasterImageBuilder.GraphicsElements;
using CardMasterImageBuilder.Skins;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace CardMasterImageBuilder.SkinElements
{
    //public enum HorizontalAlignment
    //{
    //    Left,
    //    LeftWithIcon,
    //    Center,
    //    Right
    //};

    //public enum VerticalAlignment
    //{
    //    Top,
    //    Center,
    //    Bottom
    //};

    public class SETextArea2 : SkinElement
    {
        private const string DEFAULT_FONT_FAMILY = "Bell MT";
        private const float DEFAULT_FONT_SIZE = 14;
        private const FontStyle DEFAULT_FONT_STYLE = FontStyle.Bold;

        private string fullText = null;
        private TElements elements = null;

        public StringFormat TextFormat { get; } = StringFormat.GenericDefault;

        public string TextAttribute = null;
        public HorizontalAlignment TextAlign { get; set; } = HorizontalAlignment.Left;
        public VerticalAlignment TextVerticalAlign { get; set; } = VerticalAlignment.Top;
        public Font TextFont { get; set; } = new Font(DEFAULT_FONT_FAMILY, DEFAULT_FONT_SIZE, DEFAULT_FONT_STYLE);
        public Color FontColor { get; set; } = Color.Black;
        public int WordSpaceOffsetX { get; set; } = 0;
        public int RowSpaceOffsetY { get; set; } = 0;
        public Size LeftIconsSize { get; set; } = new Size(80, 80);

        public SETextArea2(DirectoryInfo resourcesDirectory, int x, int y, int width, int height, string comments, string text) : base(resourcesDirectory, x, y, width, height, comments)
        {
            this.fullText = text;
            this.elements = new TElements(this);

            SetBackground(Color.Transparent);
        }


        protected override List<GraphicElement> GetGraphicElements(JsonCard card)
        {
            List<GraphicElement> graphicElementsList = null;

            string fullText = AttributeParser.Parse(card, this.TextAttribute, this.fullText);

            if (!string.IsNullOrEmpty(fullText))
            {
                float textEmFontSize = this.graphics.DpiY * this.TextFont.Size / 72;

                // Liste des "mots" constituants le texte.
                // Les "mots" peuvent être des mots, des marqueurs ou des références d'image. Ils sont toujours sous la formes de string et n'ont pas encore été interprétés.
                List<string> words = Words.Cut(fullText);

                try
                {
                    // Convertion des mots string en éléments interprétés
                    this.elements.Clear();
                    this.elements.AddRange(words);

                    // Applique les alignements (vertical et horizontal)
                    TextAlignment.Align(this.elements, TextAlign, TextVerticalAlign);

                }
                catch (TextSizeException ex)
                {
                    throw new TextSizeException("Erreur lors de la fabrication de la carte " + card.Name + ". " + ex.Message, ex);
                }

                // Construction des éléments graphics
                graphicElementsList = GetGraphicElementsList();
            }
            else
            {
                graphicElementsList = new List<GraphicElement>();
            }

            return graphicElementsList;
        }

        private List<GraphicElement> GetGraphicElementsList()
        {
            List<GraphicElement> graphicElementsList = new List<GraphicElement>();

            TElement element = null;
            List<TElement>.Enumerator elementsEnumerator = this.elements.GetEnumerator();

            GraphicElement gElement = null;

            while (elementsEnumerator.MoveNext())
            {
                element = elementsEnumerator.Current;

                gElement = element.GetGraphicElement(this.graphics);

                if (gElement != null)
                {
                    graphicElementsList.Add(gElement);
                }

                // Dessine le contour de la zone "au plus près" du texte --> A utiliser pour les tests de positionnement
                drawElementRectangleForTests(element);
            }

            return graphicElementsList;
        }

        private void drawElementRectangleForTests(TElement element)
        {
            Rectangle r = new Rectangle(element.X, element.Y, element.Width, element.Height);
            r.Offset(this.X, this.Y);
            this.graphics.DrawRectangle(Pens.Black, r);
        }

        public bool ReduceFontSize()
        {
            bool canReduceFont = false;
            float fontSize = TextFont.Size - 0.5f;

            if (fontSize > 5)
            {
                this.TextFont = new Font(TextFont.FontFamily, fontSize, TextFont.Style);
                this.elements.ChangeGlobalFont();
                canReduceFont = true;
            }

            return canReduceFont;
        }
    }
}
