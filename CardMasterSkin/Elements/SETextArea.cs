using CardMasterCard.Card;
using CardMasterSkin.GraphicsElements;
using CardMasterSkin.Skins;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Linq;

namespace CardMasterSkin.Elements
{
    public enum HorizontalAlignment
    {
        Left,
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
        private const char WORDS_SEPARATOR = ' ';

        private string text = null;

        public string TextAttribute = null;
        public HorizontalAlignment TextAlign = HorizontalAlignment.Left;
        public VerticalAlignment TextVerticalAlign = VerticalAlignment.Top;

        public SETextArea(Skin skin, int width, int height) : this(skin, 0, 0, width, height, null)
        { }

        public SETextArea(Skin skin, int width, int height, string text) : this(skin, 0, 0, width, height, text)
        { }

        public SETextArea(Skin skin, int x, int y, int width, int height) : this(skin, x, y, width, height, null)
        { }

        public SETextArea(Skin skin, int x, int y, int width, int height, string text) : base(skin, x, y, width, height)
        {
            this.text = text;
            SetBackground(Color.Black);
        }

        protected override List<GraphicElement> GetGraphicElements(Card card)
        {
            float textFontSize = 13;
            float textEmFontSize = this.graphics.DpiY * textFontSize / 72;
            var textFontFamily = new FontFamily("Bell MT");
            var textFontStyle = FontStyle.Bold;
            var textFont = new Font(textFontFamily, textFontSize, textFontStyle);
            var textFormat = StringFormat.GenericDefault;

            var rowsList = new List<TextRow>();
            int rowTop = 0;

            List<TextElement> textElementsList = null;

            // Découpage en bloc de chaînes (découpage par "retour à la ligne")
            List<string> textList = GetTextList(card);

            // Dessine le contour de la zone max du texte --> A utiliser pour les tests
            // m_graphics.DrawRectangle(Pens.Red, Me.X, Me.Y, Me.Width, Me.Height)

            foreach (string txt in textList)
            {
                // Recherche des symboles à charger.
                // --> Retourne une liste composée de StringElement qui sont soit un mot soit un symbole.
                // Si la chaine ne contient pas de mot à remplacer par un symbole, un string element contenant
                //  l'ensemble de la chaîne sera retourné.
                textElementsList = GetTextElements(txt);

                // Chargement des images (symboles)
                LoadSymbolsImages(textElementsList);

                // Calcul de la taille et de la position en X,Y des TextElement (mots et symboles) composant le texte,
                // et retourne une liste de lignes de texte composées de mots et de symboles.
                rowsList.AddRange(GetRows(textElementsList, textFont, textFormat, rowTop));

                // Calcul de la position verticale de la prochaine ligne
                if (rowsList.Count > 0)
                {
                    rowTop = rowsList.Last<TextRow>().Bottom;
                }

                textElementsList.Clear();
                textElementsList = null;

            }

            ApplyAlignments(rowsList);

            List<GraphicElement> graphicElementsList = GetGraphicElementsList(rowsList, textFont, textEmFontSize, textFormat);

            rowsList.Clear();
            rowsList = null;

            textList.Clear();
            textList = null;

            return graphicElementsList;
        }

        private List<string> GetTextList(Card card)
        {
            var txtList = new List<string>();
            string fullText = null;

            if (! string.IsNullOrEmpty(this.TextAttribute))
            {
                Type cardType = card.GetType();
                PropertyInfo cardProperty = cardType.GetProperty(this.TextAttribute);
                MethodInfo cardGetMethod = cardProperty.GetGetMethod();
                object propertyValue = cardGetMethod.Invoke(card, null);

                if (propertyValue != null)
                {
                    fullText = propertyValue.ToString();
                }
            }
            else
            {
                fullText = this.text;
            }


            if (!string.IsNullOrEmpty(fullText))
            {
                string[] txtArray = fullText.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (string txt in txtArray)
                {
                    txtList.Add(txt);
                }
            }


            return txtList;
        }

        private List<TextElement> GetTextElements(string text)
        {
            var textElementsList = new List<TextElement>();
            TextElement textElement = null;

            char[] separators = { WORDS_SEPARATOR };
            string[] wordsArray = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in wordsArray)
            {
                textElement = new TextElement();
                textElement.Text = word;
                textElementsList.Add(textElement);
            }

            return textElementsList;
        }

        private void LoadSymbolsImages(List<TextElement> textElementsList)
        {
            string searchPattern = null;
            DirectoryInfo symbolsDirectory = this.skin.TexturesDirectory;
            bool symbolsDirExists = (symbolsDirectory != null) && (symbolsDirectory.Exists);

            if (symbolsDirExists)
            {
                foreach (TextElement element in textElementsList)
                {
                    if (element.Text.StartsWith("<<") && element.Text.EndsWith(">>"))
                    {
                        element.Text = element.Text.Substring(2, element.Text.Length - 4);
                        searchPattern = element.Text + ".*";

                        FileInfo[] files = symbolsDirectory.GetFiles(searchPattern, SearchOption.AllDirectories);

                        if (files.Length > 0)
                        {
                            element.Image = Bitmap.FromFile(files[0].FullName);
                        }

                        files = null;
                    }
                }
            }                        
        }

        private List<TextRow> GetRows(List<TextElement> TextElementsList, Font textFont, StringFormat textFormat, int rowTop)
        {
            var rowsList = new List<TextRow>();
            TextRow row = null;

            var elementSize = SizeF.Empty;
            var refSize = SizeF.Empty;
            var initSize = SizeF.Empty;
            float ratio = 0;
            int elementX = 0;
            int rowY = rowTop;

            GraphicsUnit graphicsPageUnit = this.graphics.PageUnit;

            bool isSeparator = false;

            foreach (TextElement textElement in TextElementsList)
            {
                isSeparator = false;

                if (row == null)
                {
                    row = new TextRow();
                    row.Y = rowY;
                    rowsList.Add(row);
                }

                if (textElement.IsText)
                {
                    // Ajout d'un mot à une ligne

                    elementSize = this.graphics.MeasureString(textElement.Text, textFont, this.Width, textFormat);
                    isSeparator = (textElement.Text.Equals(WORDS_SEPARATOR));
                }
                else
                {
                    // Ajout d'un symbole à une ligne

                    if (refSize == SizeF.Empty)
                    {
                        refSize = this.graphics.MeasureString("O", textFont, this.Width, textFormat);
                    }

                    initSize = textElement.Image.GetBounds(ref graphicsPageUnit).Size;
                    ratio = initSize.Height / refSize.Height;
                    elementSize = new SizeF(initSize.Width / ratio, refSize.Height);
                }

                if ((elementX + elementSize.Width) > this.Width)
                {
                    if (isSeparator)
                    {
                        // Si le nouvel élément à ajouter dépasse la zone autorisée et est un séparateur de mots (caractère espace)
                        // alors il est ignoré

                        // Force la création d'une nouvelle ligne si il y a un nouvel élément à ajouter
                        row = null;

                        // Ignore le textElement courant en forçant le Next
                        continue;
                    }

                    rowY = row.Bottom;
                    row = new TextRow();
                    row.Y = rowY;
                    elementX = 0;
                    rowsList.Add(row);
                }

                textElement.Bounds = new Rectangle(elementX, 0, (int)Math.Ceiling(elementSize.Width), (int)Math.Ceiling(elementSize.Height));
                row.AddElement(textElement);
                elementX += textElement.Bounds.Width;
            }

            return rowsList;

        }

        private void ApplyAlignments(List<TextRow> rowsList)
        {
            switch (this.TextAlign)
            {
                case HorizontalAlignment.Right:
                    foreach (TextRow row in rowsList)
                    {
                        row.X = this.Width - row.Width;
                    }
                    break;

                case HorizontalAlignment.Center:
                    foreach (TextRow row in rowsList)
                    {
                        row.X = (int)Math.Round((double)(this.Width - row.Width) / 2);
                    }
                    break;

                case HorizontalAlignment.Left:
                    // rien à faire
                    break;
            }

            switch (this.TextVerticalAlign)
            {
                case VerticalAlignment.Top :
                    // Rien à faire
                    break;

                case VerticalAlignment.Center:
                    // Même traitement que pour VerticalAlignment.Bottom

                case VerticalAlignment.Bottom:
                    int rowsHeight = 0;
                    int y = 0;

                    foreach (TextRow row in rowsList)
                    {
                        rowsHeight += row.Height;
                    }

                    if (TextVerticalAlign == VerticalAlignment.Center)
                    {
                        y = (int)Math.Round((double)(this.Height - rowsHeight) / 2);
                    }
                    else if (TextVerticalAlign == VerticalAlignment.Bottom)
                    {
                        y = this.Height - rowsHeight;
                    }

                    foreach (TextRow row in rowsList)
                    {
                        row.Y += y;
                    }

                    break;
            }
        }

        private List<GraphicElement> GetGraphicElementsList(List<TextRow> rowsList, Font textFont, float textEmFontSize, StringFormat textFormat)
        {
            var graphicElementsList = new List<GraphicElement>();
            GraphicsPath textPath = null;

            Rectangle r;

            foreach (TextRow row in rowsList)
            {
                foreach (TextElement textElement in row.Elements)
                {
                    if (textElement.IsText)
                    {
                        if (textPath == null)
                        {
                            textPath = new GraphicsPath();
                            graphicElementsList.Add(new PathElement(textPath, GetBackground()));
                        }

                        r = new Rectangle(this.X + row.X + textElement.Bounds.X, this.Y + row.Y + textElement.Bounds.Y, textElement.Bounds.Width, textElement.Bounds.Height);
                        textPath.AddString(textElement.Text, textFont.FontFamily, (int)textFont.Style, textEmFontSize, r, textFormat);

                        // Dessine le contour de la zone "au plus près" du texte --> A utiliser pour les tests de positionnement
                        // m_graphics.DrawRectangle(Pens.Black, r)
                    }
                    else
                    {
                        r = new Rectangle(this.X + row.X + textElement.Bounds.X, this.Y + row.Y + textElement.Bounds.Y, textElement.Bounds.Width, textElement.Bounds.Height);
                        graphicElementsList.Add(new ImageElement(textElement.Image, r));
                    }
                }
            }

            return graphicElementsList;
        }

        private class TextElement
        {
            public string Text { get; set; } = "";
            public Image Image { get; set; } = null;
            public Rectangle Bounds { get; set; }
            public bool IsText { get { return (this.Image == null); } }
            public bool IsImage { get { return (this.Image != null); } }
        }

        private class TextRow
        {
            public List<TextElement> Elements { get; set; } = new List<TextElement>();

            public int X { get; set; } = 0;
            public int Y { get; set; } = 0;
            public int Width { get; set; } = 0;
            public int Height { get; set; } = 0;
            public int Bottom { get { return this.Y + this.Height; } }
            public int Right { get { return this.X + this.Width; } }

            public void AddElement(TextElement element)
            {
                this.Elements.Add(element);

                Rectangle elementBounds = element.Bounds;

                if (this.Height < elementBounds.Height)
                {
                    this.Height = elementBounds.Height;
                }

                if (this.Width < elementBounds.Right)
                {
                    this.Width = elementBounds.Right;
                }
            }

        }

    }
}
