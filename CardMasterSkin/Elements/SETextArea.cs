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
using CardMasterSkin.Converters;

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

        private string text = null;

        public string TextAttribute = null;
        public HorizontalAlignment TextAlign = HorizontalAlignment.Left;
        public VerticalAlignment TextVerticalAlign = VerticalAlignment.Top;
        public Font TextFont { get; set; } = new Font(DEFAULT_FONT_FAMILY, DEFAULT_FONT_SIZE, DEFAULT_FONT_STYLE);

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
            float textEmFontSize = this.graphics.DpiY * this.TextFont.Size / 72;
            StringFormat textFormat = StringFormat.GenericDefault;

            // Liste des paragraphes constituant le texte
            List<TextSection> sectionsList = new List<TextSection>();
            TextSection section = null;

            int rowTop = 0;

            List<TextElement> textElementsList = null;

            // Découpage en bloc de chaînes (découpage par "retour à la ligne")
            List<string> textList = GetTextList(card);

            // Dessine le contour de la zone max du texte --> A utiliser pour les tests
            // this.graphics.DrawRectangle(Pens.Red, Me.X, Me.Y, Me.Width, Me.Height)

            foreach (string txt in textList)
            {
                section = new TextSection();
                sectionsList.Add(section);

                // Recherche des symboles à charger.
                // --> Retourne une liste composée de StringElement qui sont soit un mot soit un symbole.
                // Si la chaine ne contient pas de mot à remplacer par un symbole, un string element contenant
                //  l'ensemble de la chaîne sera retourné.
                textElementsList = GetTextElements(txt);

                // Chargement des images (symboles)
                LoadSymbolsImages(textElementsList);

                // Calcul de la taille et de la position en X,Y des TextElement (mots et symboles) composant le texte,
                // et retourne une liste de lignes de texte composées de mots et de symboles.
                section.AddRange(GetRows(textElementsList, this.TextFont, textFormat, rowTop));

                // Calcul de la position verticale de la prochaine ligne
                if (section.Count > 0)
                {
                    rowTop = section.Last<TextRow>().Bottom;
                }

                textElementsList.Clear();
                textElementsList = null;

            }

            ApplyAlignments(sectionsList);

            List<GraphicElement> graphicElementsList = GetGraphicElementsList(sectionsList, this.TextFont, textEmFontSize, textFormat);

            sectionsList.Clear();
            sectionsList = null;

            textList.Clear();
            textList = null;

            return graphicElementsList;
        }

        private List<string> GetTextList(Card card)
        {
            var txtList = new List<string>();
            string fullText = null;


            fullText = AttributeParser.Parse(card, this.TextAttribute,this.text);

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

        private void ApplyAlignments(List<TextSection> sectionsList)
        {
            switch (this.TextAlign)
            {
                case HorizontalAlignment.Left:
                    // rien à faire
                    break;
                case HorizontalAlignment.LeftWithIcon:
                    ApplyHorizontalAlignementLeftWithIcon(sectionsList);
                    break;
                case HorizontalAlignment.Center:
                    ApplyHorizontalAlignementCenter(sectionsList);
                    break;
                case HorizontalAlignment.Right:
                    ApplyHorizontalAlignementRight(sectionsList);
                    break;
            }

            switch (this.TextVerticalAlign)
            {
                case VerticalAlignment.Top :
                    // Rien à faire
                    break;
                case VerticalAlignment.Center:
                    ApplyVerticalAlignementCenter(sectionsList);
                    break;
                case VerticalAlignment.Bottom:
                    ApplyVerticalAlignementBottom(sectionsList);
                    break;
            }
        }

        private void ApplyHorizontalAlignementLeftWithIcon(List<TextSection> sectionsList)
        {
            TextElement element = null;
            int iconWidth = 0;
            int tmpIconWidth = 0;
            int iconsCount = 0;

            // Recherche de la plus grande taille d'icon de début de paragraphe
            foreach (TextSection section in sectionsList)
            {
                try
                {
                    element = section.First<TextRow>().Elements.First<TextElement>();

                    if (element.IsImage)
                    {
                        tmpIconWidth = element.Bounds.Width;
                        if (tmpIconWidth > iconWidth)
                        {
                            iconWidth = tmpIconWidth;
                        }
                        iconsCount++;
                    }
                }
                catch (NullReferenceException)
                { }
            }

            if (iconsCount > 0)
            {
                // Rajout d'un espace
                if (iconWidth > 0)
                {
                    iconWidth += 5;
                }

                foreach (TextSection section in sectionsList)
                {
                    int rowIndex = 0;
                    int elementIndex = 0;
                    int offsetX = 0;

                    foreach (TextRow row in section)
                    {
                        if (rowIndex == 0)
                        {
                            if (row.Elements.Count > 1)
                            {
                                offsetX = iconWidth - row.Elements.ElementAt<TextElement>(1).Bounds.X;

                                elementIndex = 1;
                                while (elementIndex < row.Elements.Count)
                                {
                                    Rectangle r = row.Elements.ElementAt<TextElement>(elementIndex).Bounds;
                                    r.X += offsetX;
                                    row.Elements.ElementAt<TextElement>(elementIndex).Bounds = r;
                                    elementIndex++;
                                }
                            }
                        }
                        else
                        {
                            row.X = iconWidth;
                        }
                        rowIndex++;
                    }
                }
            }
        }

        private void ApplyHorizontalAlignementCenter(List<TextSection> sectionsList)
        {
            foreach (TextSection section in sectionsList)
            {
                foreach (TextRow row in section)
                {
                    row.X = (int)Math.Round((double)(this.Width - row.Width) / 2);
                }
            }
        }

        private void ApplyHorizontalAlignementRight(List<TextSection> sectionsList)
        {
            foreach (TextSection section in sectionsList)
            {
                foreach (TextRow row in section)
                {
                    row.X = this.Width - row.Width;
                }
            }
        }

        private void ApplyVerticalAlignementCenter(List<TextSection> sectionsList)
        {
            foreach (TextSection section in sectionsList)
            {
                int rowsHeight = 0;
                int y = 0;

                foreach (TextRow row in section)
                {
                    rowsHeight += row.Height;
                }

                y = (int)Math.Round((double)(this.Height - rowsHeight) / 2);

                foreach (TextRow row in section)
                {
                    row.Y += y;
                }

            }
        }


        private void ApplyVerticalAlignementBottom(List<TextSection> sectionsList)
        {
            foreach (TextSection section in sectionsList)
            {
                int rowsHeight = 0;
                int y = 0;

                foreach (TextRow row in section)
                {
                    rowsHeight += row.Height;
                }

                y = this.Height - rowsHeight;

                foreach (TextRow row in section)
                {
                    row.Y += y;
                }

            }
        }

        private List<GraphicElement> GetGraphicElementsList(List<TextSection> sectionsList, Font textFont, float textEmFontSize, StringFormat textFormat)
        {
            var graphicElementsList = new List<GraphicElement>();
            GraphicsPath textPath = null;

            Rectangle r;

            foreach (TextSection section in sectionsList)
            {
                foreach (TextRow row in section)
                {
                    foreach (TextElement textElement in row.Elements)
                    {
                        r = new Rectangle(this.X + row.X + textElement.Bounds.X, this.Y + row.Y + textElement.Bounds.Y, textElement.Bounds.Width, textElement.Bounds.Height);

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

        private class TextSection : List<TextRow>
        { }

    }
}
