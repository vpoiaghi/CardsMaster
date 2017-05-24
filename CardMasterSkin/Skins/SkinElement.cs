using CardMasterCard.Card;
using CardMasterSkin.GraphicsElements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace CardMasterSkin.Skins
{
    public abstract class SkinElement
    {
        private static Dictionary<String, Brush> imgTexturesMap = new Dictionary<String, Brush>();

        private enum TextureTypes : int { Color, GradientColor, Image };

        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;
        public SkinElementShadow Shadow { get; set; } = null;
        public SkinElementBorder Border { get; set; } = null;
        public bool Visible { get; set; } = true;
        protected Color color1;
        protected Color color2;
        protected string imageName = null;
        private TextureTypes type;
        protected Skin skin = null;


        protected Graphics graphics = null;

        protected abstract List<GraphicElement> GetGraphicElements(Card card);

        protected SkinElement(Skin skin, int width, int height) : this(skin, 0, 0, width, height)
        { }

        protected SkinElement(Skin skin, int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.skin = skin;

            SetBackground(Color.Black);
        }

        ~SkinElement()
        {
            this.Shadow = null;
            this.skin = null;
            this.graphics = null;
        }

        public void Draw(Graphics g, Card card)
        {
            this.graphics = g;

            List<GraphicElement> graphicElements = GetGraphicElements(card);

            foreach(GraphicElement graphicElement in graphicElements)
            {
              
                    if (this.Shadow != null)
                    {
                        graphicElement.DrawShadow(g, this.Shadow);
                    }

                    graphicElement.Draw(g);

                    if (this.Border != null)
                    {
                        graphicElement.DrawBorder(g, this.Border);
                    }
               
            }

            graphicElements.Clear();
            graphicElements = null;

            this.graphics = null;
        }

        public void SetBackground(Color color)
        {
            this.color1 = color;
            this.imageName = null;
            this.type = TextureTypes.Color;
        }

        public void SetBackground(Color color1, Color color2)
        {
            this.color1 = color1;
            this.color2 = color2;
            this.imageName = null;
            this.type = TextureTypes.GradientColor;
        }

        public void SetBackground(String imageName)
        {
            this.imageName = imageName;
            this.type = TextureTypes.Image;
        }

        public Brush GetBackground()
        {
            Brush bkg = null;

            switch (this.type)
            {
                case TextureTypes.Color:
                    bkg = new SolidBrush(this.color1);
                    break;

                case TextureTypes.GradientColor:
                    bkg = new LinearGradientBrush(new Point(X, 1), new Point(Height, 1), this.color1, this.color2);
                    break;

                case TextureTypes.Image:
                    if ((this.skin.TexturesDirectory != null) && (this.skin.TexturesDirectory.Exists))
                    {
                        FileInfo[] files = this.skin.TexturesDirectory.GetFiles(this.imageName + ".*", SearchOption.AllDirectories);

                        if (files.Length > 0)
                        {
                            string fileName = files[0].FullName;

                            try
                            {
                                bkg = imgTexturesMap[fileName];
                            }
                            catch (KeyNotFoundException)
                            {
                                bkg = null;
                            }

                            if (bkg == null)
                            {
                                Image img = new Bitmap(fileName);
                                //bkg = new TextureBrush(img, WrapMode.Clamp);
                                bkg = new TextureBrush(img, WrapMode.TileFlipXY);
                                img.Dispose();
                                img = null;

                                imgTexturesMap[fileName] = bkg;

                            }
                        }

                        files = null;
                    }
                    break;
            }

            if (bkg == null)
            {
                bkg = new SolidBrush(Color.Black);
            }

            return bkg;
        }

    }
}
