using CardMasterImageBuilder.GraphicsElements;
using CardMasterImageBuilder.SkinElements;
using System;
using System.Collections.Generic;

namespace CardMasterImageBuilder.Elements.TextFormater
{
    abstract class TextAlignment
    {
        public static void Align(TElements elements, HorizontalAlignment hAlignment, VerticalAlignment vAlignment)
        {
            int areaWidth = elements.TextArea.Width;
            int areaHeight = elements.TextArea.Height;

            int y = 0;
            int rowWidth = 0;
            List<int> rowWidthList = new List<int>();
            int rowIndex = 0;
            int textHeight = 0;

            List<TElement>.Enumerator elementsEnum = elements.GetEnumerator();
            TElement element = null;

            while (elementsEnum.MoveNext())
            {
                element = elementsEnum.Current;

                if (! (element is TElementReturn))
                {
                    if (element.Bottom > textHeight)
                    {
                        textHeight = element.Bottom;
                    }

                    if (y == element.Y)
                    {
                        rowWidth += element.Width;
                    }
                    else
                    {
                        y = element.Y;
                        rowWidthList.Add(rowWidth);
                        rowWidth = element.Width;
                    }
                }
                else
                {
                    y = element.Y;
                    rowWidthList.Add(rowWidth);
                    rowWidth = 0;
                }
            }
            rowWidthList.Add(rowWidth);

            

            elementsEnum = elements.GetEnumerator();
            rowIndex = 0;
            y = 0;

            int offsetY = GetVerticalOffset(areaHeight, textHeight, vAlignment);
            int offsetX = GetHorizontalOffset(areaWidth, rowWidthList[rowIndex++], hAlignment);

            while (elementsEnum.MoveNext())
            {
                element = elementsEnum.Current;

                if (y != element.Y)
                {
                    y = element.Y;
                    offsetX = GetHorizontalOffset(areaWidth, rowWidthList[rowIndex++], hAlignment);
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
