using CardMasterImageBuilder.GraphicsElements;
using CardMasterImageBuilder.SkinElements;
using System;
using System.Collections.Generic;

namespace CardMasterImageBuilder.Elements.TextFormater
{
    abstract class TextAlignment
    {
        public static int GetTextHeight(TElements elements)
        {
            int textHeight = 0;

            List<TElement>.Enumerator elementsEnum = elements.GetEnumerator();
            TElement element = null;

            while (elementsEnum.MoveNext())
            {
                element = elementsEnum.Current;

                if (!(element is TElementReturn))
                {
                    if (element.Bottom > textHeight)
                    {
                        textHeight = element.Bottom;
                    }
                }
            }

            return textHeight;
        }

        public static List<int> GetTextLinesWidth(TElements elements)
        {
            List<int> lineWidthList = new List<int>();

            int y = 0;
            int lineWidth = 0;

            List<TElement>.Enumerator elementsEnum = elements.GetEnumerator();
            TElement element = null;

            while (elementsEnum.MoveNext())
            {
                element = elementsEnum.Current;

                if (!(element is TElementReturn))
                {
                    if (y == element.Y)
                    {
                        lineWidth += element.Width;
                    }
                    else
                    {
                        y = element.Y;
                        lineWidthList.Add(lineWidth);
                        lineWidth = element.Width;
                    }
                }
                else
                {
                    y = element.Y;
                    lineWidthList.Add(lineWidth);
                    lineWidth = 0;
                }
            }

            lineWidthList.Add(lineWidth);

            return lineWidthList;
        }

        public static void Align(TElements elements, HorizontalAlignment hAlignment, VerticalAlignment vAlignment)
        {
            int areaWidth = elements.TextArea.Width;
            int areaHeight = elements.TextArea.Height;
            int lineIndex = 0;
            int y = 0;

            int textHeight = GetTextHeight(elements);
            List<int> lineWidthList = GetTextLinesWidth(elements);

            List<TElement>.Enumerator elementsEnum = elements.GetEnumerator();
            TElement element = null;

            int offsetY = GetVerticalOffset(areaHeight, textHeight, vAlignment);
            int offsetX = GetHorizontalOffset(areaWidth, lineWidthList[lineIndex++], hAlignment);

            while (elementsEnum.MoveNext())
            {
                element = elementsEnum.Current;

                if (y != element.Y)
                {
                    y = element.Y;
                    offsetX = GetHorizontalOffset(areaWidth, lineWidthList[lineIndex++], hAlignment);
                }

                element.Y += offsetY;
                element.X += offsetX;
            }
        }

        private static int GetHorizontalOffset(int areaWidth, int textWidth, HorizontalAlignment hAlignment)
        {
            int offsetX = 0;

            switch (hAlignment)
            {
                case HorizontalAlignment.Center:
                    offsetX = (int)(Math.Floor((areaWidth - textWidth) / 2d));
                    break;
                case HorizontalAlignment.Right:
                    offsetX = areaWidth - textWidth;
                    break;
            }

            return offsetX;
        }

        private static int GetVerticalOffset(int areaHeight, int textHeight, VerticalAlignment vAlignment)
        {
            int offsetY = 0;

            switch (vAlignment)
            {
                case VerticalAlignment.Center:
                    offsetY = (int)(Math.Floor((areaHeight - textHeight) / 2d));
                    break;
                case VerticalAlignment.Bottom:
                    offsetY = areaHeight - textHeight;
                    break;
            }

            return offsetY;
        }

    }
}
