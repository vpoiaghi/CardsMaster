using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace CardMasterSkin.Elements.TextFormat
{
    class TextSection
    {
        private const char WORDS_SEPARATOR = ' ';

        private string text = null;
        private List<TextElement> textElementsList = null;
        private List<TextRow> rowsList = null;

        public Rectangle Bounds { get; set; } = Rectangle.Empty;

        public TextSection(string text)
        {
            this.text = text;
            textElementsList = GetTextElementsList(this.text);
        }

        private List<TextElement> GetTextElementsList(string text)
        {
            List<TextElement> textElementsList = new List<TextElement>();
            TextElement textElement = null;

            char[] separators = { WORDS_SEPARATOR };
            string[] wordsArray = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in wordsArray)
            {
                textElement = new TextElement(word);
                textElementsList.Add(textElement);
            }

            return textElementsList;
        }

        public List<TextRow> Rows { get { return rowsList; } }

        internal void Format(Graphics g, Font TextFont, StringFormat TextFormat, int MaxWidth, int hAlignLeftWithIconOffsetX, int wordSpaceOffsetX, int rowSpaceOffsetY)
        {
            TextRow row = null;
            rowsList = new List<TextRow>();

            SizeF elementSize = SizeF.Empty;

            int rowIndex = -1;
            int elementIndex = 0;
            int elementX = 0;
            int elementWidth = 0;
            int rowY = 0;
            bool prevElementIsImage = false;

            GraphicsUnit graphicsPageUnit = g.PageUnit;

            foreach (TextElement textElement in this.textElementsList)
            {
                if (row == null)
                {
                    rowIndex++;
                    row = new TextRow();
                    row.Y = rowY;
                    rowsList.Add(row);
                    prevElementIsImage = false;
                }

                elementSize = textElement.GetSize(g, TextFont, TextFormat, MaxWidth);
                elementWidth = (int)Math.Ceiling(elementSize.Width);
                elementX = GetElementX(elementX, rowIndex, elementIndex, textElement, hAlignLeftWithIconOffsetX, prevElementIsImage);

                if (! CanAddElementOnRow(rowIndex, MaxWidth, elementIndex, elementX, elementWidth, hAlignLeftWithIconOffsetX))
                {
                    if (textElement.Text.Equals(WORDS_SEPARATOR))
                    {
                        // Si le nouvel élément à ajouter dépasse la zone autorisée et est un séparateur de mots (caractère espace)
                        // alors il est ignoré

                        // Force la création d'une nouvelle ligne si il y a un nouvel élément à ajouter
                        row = null;
                        rowY = row.Bottom + rowSpaceOffsetY;

                        // Ignore le textElement courant en forçant le Next
                        continue;
                    }

                    rowIndex++;
                    rowY = row.Bottom + rowSpaceOffsetY;
                    row = new TextRow();
                    row.Y = rowY;
                    rowsList.Add(row);
                    prevElementIsImage = false;

                    elementIndex = 0;
                    elementX = GetElementX(0, rowIndex, elementIndex, textElement, hAlignLeftWithIconOffsetX, prevElementIsImage);
                }

                textElement.Bounds = new Rectangle(elementX, 0, elementWidth, (int)Math.Ceiling(elementSize.Height));
                row.AddElement(textElement);
                elementX += textElement.Bounds.Width + wordSpaceOffsetX;
                elementIndex++;
                prevElementIsImage = textElement.IsImage;
            }

            int h = rowsList.Count > 0 ? rowsList.Last().Bottom : 0;
            this.Bounds = new Rectangle(0, 0, MaxWidth, h);
        }

        internal void LoadSymbols(DirectoryInfo SymbolsDirectory)
        {
            foreach (TextElement element in this.textElementsList)
            {
                element.LoadSymbols(SymbolsDirectory);
            }

        }

        internal TextElement GetTextElementAt(int index)
        {
            if ((index < 0) || (index >= this.textElementsList.Count))
            {
                throw new IndexOutOfRangeException();
            }

            return this.textElementsList[index];
        }

        /**
         * Détermine si un élément (défini par son indexation, sa position X et sa largeur) peut être ajouté à une ligne (définie par son indexation
         * et sa largeur maximale), en prenant en compte le cas potentiel de l'alignement de type LeftWithIcon.
         **/
        private bool CanAddElementOnRow(int rowIndex, int rowMaxWidth, int elementIndex, int elementX, int elementWidth, int hAlignLeftWithIconOffsetX)
        {
            Boolean canAdd = true;

            if ((elementX + elementWidth) > rowMaxWidth)
            {
                // Si l'élément à ajouter à la ligne dépasse à droite
                // alors - a priori - l'ajout à la ligne courante n'est pas autorisé, il est nécessaire de créer une nouvelle ligne.
                canAdd = false;

                // Il faut prendre en compte le cas où un seul mot est plus large que la zone de texte.
                // Dans ce cas on écrit le mot sur la ligne courante même si il dépasse.

                if (rowIndex == 0)
                {
                    // Si la ligne courante est la première de son paragraphe

                    if ((hAlignLeftWithIconOffsetX == 0) && (elementIndex == 0))
                    {
                        // Si l'alignement LeftWithIcon ne doit pas être appliqué (le mode d'alignement à appliquer n'est pas 
                        // LeftWithIcon ou l'offset x à appliquer a été calculé et égal à zéro),
                        // et si l'élément courant est le premier de sa ligne
                        // alors un renvoi à la ligne n'est pas autorisé, car on est ici en présence d'un mot plus large que la zone de texte
                        // l'élément doit être ajouté à la ligne courante même si il dépasse.
                        canAdd = true;
                    }
                    else if ((hAlignLeftWithIconOffsetX > 0) && (elementIndex == 1))
                    {
                        // Si l'alignement LeftWithIcon doit être appliqué (l'offset x à appliquer ayant été calculé et étant > 0),
                        // et si l'élément courant est le deuxième de sa ligne (1er élément après l'élément icone de paragraphe),
                        // alors un renvoi à la ligne n'est pas autorisé, car on est ici en présence d'un mot plus large que la zone de texte
                        // l'élément doit être ajouté à la ligne courante même si il dépasse.
                        canAdd = true;
                    }
                }
                else
                {
                    // Si la ligne courante n'est pas la première de son paragraphe

                    if (elementIndex == 0)
                    {
                        // Si l'alignement LeftWithIcon ne doit pas être appliqué (le mode d'alignement à appliquer n'est pas 
                        // LeftWithIcon ou l'offset x à appliquer a été calculé et égal à zéro),
                        // et si l'élément courant est le premier de sa ligne
                        // alors un renvoi à la ligne n'est pas autorisé, car on est ici en présence d'un mot plus large que la zone de texte
                        // l'élément doit être ajouté à la ligne courante même si il dépasse.
                        canAdd = true;
                    }
                }
            }

            return canAdd;
        }

        private int GetElementX(int elementX, int rowIndex, int elementIndex, TextElement textElement, int hAlignLeftWithIconOffsetX, bool prevElementIsImage)
        {
            int result = elementX;

            if (hAlignLeftWithIconOffsetX > 0)
            {
                // Si l'alignement LeftWithIcon doit être appliqué (l'offset x à appliquer ayant été calculé et étant > 0),
                // et si la ligne courante est la première du paragraphe,
                // et si l'élément courant est le deuxième de sa ligne (1er élément après l'élément icone de paragraphe),
                // alors appliquer l'offset à la position element x

                if (rowIndex == 0)
                {
                    // Si la ligne courante est la première du paragraphe

                    if ((elementIndex == 0) && (textElement.IsText))
                    {
                        result = hAlignLeftWithIconOffsetX;
                    }
                    else if ((elementIndex == 1) && (prevElementIsImage))
                    {
                        // Si l'élément courant est le deuxième de sa ligne (1er élément après l'élément icone de paragraphe),
                        // alors appliquer l'offset à la position element x
                        result = hAlignLeftWithIconOffsetX;
                    }
                }
                else if ((rowIndex > 0) && (elementIndex == 0))
                {
                    // Si la ligne courante n'est pas la première du paragraphe,
                    // et si l'élément courant est le premier de sa ligne,
                    // alors appliquer l'offset à la position element x
                    result = hAlignLeftWithIconOffsetX;
                }
            }

            return result;
        }
    }
}
