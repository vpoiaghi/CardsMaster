using CardMasterCard.Card;
using CardMasterSkin.GraphicsElements;
using CardMasterSkin.Skins;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace CardMasterSkin.Elements
{
    public class SEImage : SkinElement
    {
        public string ImageName { get; set; } = null;
        public string NameAttribute { get; set; } = null;

        public SEImage(Skin skin, int x, int y, int width, int height) : base(skin, x, y, width, height)
        { }

        public SEImage(Skin skin, int x, int y, int width, int height, string imageName) : base(skin, x, y, width, height)
        {
            this.ImageName = imageName;
        }

        protected override List<GraphicElement> GetGraphicElements(Card card)
        {
            var graphicElementsList = new List<GraphicElement>();
            string fileFullname = GetFileFullname(card);

            if (fileFullname != null)
            {
                Image img = Bitmap.FromFile(fileFullname, true);
                graphicElementsList.Add(new ImageElement(img, new Rectangle(this.X, this.Y, this.Width, this.Height)));
            }

            return graphicElementsList;

        }

        private string GetFileFullname(Card card)
        {
            string fileFullname = null;
            DirectoryInfo rootDirectory = this.skin.ResourcesDirectory;

            if ((rootDirectory != null) && (rootDirectory.Exists))
            {
                string fileName = null;

                if (!string.IsNullOrEmpty(this.NameAttribute))
                {
                    Type cardType = card.GetType();
                    PropertyInfo cardProperty = cardType.GetProperty(this.NameAttribute);
                    MethodInfo cardGetMethod = cardProperty.GetGetMethod();
                    object propertyValue = cardGetMethod.Invoke(card, null);

                    if (propertyValue != null)
                    {
                        fileName = propertyValue.ToString();
                    }

                }
                else
                {
                    fileName = ImageName;
                }

                if (fileName != null)
                {
                    fileName = fileName.Replace(":", "-").Replace("\"", "'");
                    FileInfo[] files = rootDirectory.GetFiles(fileName + ".*", SearchOption.AllDirectories);

                    if (files.Length > 0)
                    {
                        fileFullname = files[0].FullName;
                    }

                    files = null;
                }

            }

            return fileFullname;

        }

    }
}
