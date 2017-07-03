using CardMasterCard.Card;
using CardMasterCommon.Utils;
using CardMasterImageBuilder.GraphicsElements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace CardMasterImageBuilder.Skins
{
    public abstract class SkinElement
    {
        private static Dictionary<String, Brush> imgTexturesMap = new Dictionary<String, Brush>();

        private enum TextureTypes : int { Color, GradientColor, Image };

        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;
        public string Comments { get; set; } = "";
        public SkinElementShadow Shadow { get; set; } = null;
        public SkinElementBorder Border { get; set; } = null;
        public bool Visible { get; set; } = true;
        protected Color color1;
        protected Color color2;
        protected int? externalCardBorderthickness;
        protected string imageName = null;
        private TextureTypes type;
        protected DirectoryInfo resourcesDirectory = null;
        private ColorConverter colorConverter = new ColorConverter();

        protected Graphics graphics = null;
        public Graphics Graphic { get { return this.graphics; } }

        protected abstract List<GraphicElement> GetGraphicElements(JsonCard card);

        protected SkinElement(DirectoryInfo resourcesDirectory, int x, int y, int width, int height, string comments)
        {
            this.resourcesDirectory = resourcesDirectory;

            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;

            this.Comments = comments;

            SetBackground(Color.Black);
        }

        ~SkinElement()
        {
            this.Shadow = null;
            this.resourcesDirectory = null;
            this.graphics = null;
        }

        public void Draw(Graphics g, JsonCard card)
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

        public void SetBackground(Color color1, Color color2, int? externalBorderThickness)
        {
            this.color1 = color1;
            this.color2 = color2;
            this.imageName = null;
            this.type = TextureTypes.GradientColor;
            this.externalCardBorderthickness = externalBorderThickness;
        }

        public void SetBackground(String imageName)
        {
            if (imageName.StartsWith("#"))
            {
                this.color1 = (Color)colorConverter.ConvertFromString(imageName);
                this.imageName = null;
                this.type = TextureTypes.Color;
            }
            else
            {
                this.imageName = imageName;
                this.type = TextureTypes.Image;
            }
        }

        public void SetBackground(String imageName, String imageName2, int? externalBorderThickness)
        {
            if (imageName!=null && imageName.StartsWith("#"))
            {
                this.color1 = (Color)colorConverter.ConvertFromString(imageName);
                this.type = TextureTypes.Color;
                if (imageName2!=null && imageName2 != "")
                {
                    this.color2 = (Color)colorConverter.ConvertFromString(imageName2);
                    this.externalCardBorderthickness = externalBorderThickness;
                    this.type = TextureTypes.GradientColor;
                }       
                this.imageName = null;
               
            }
            else
            {
                this.imageName = imageName;
                if (imageName2 !=null && imageName2 != null && imageName2 != "")
                {
                    this.imageName = imageName2;
                }
                this.type = TextureTypes.Image;
            }
        }

        public Brush GetBackground()
        {
            return GetBackground(null);
        }

        public Brush GetBackground(GraphicsPath path)
        {
            Brush bkg = null;

            switch (this.type)
            {
                case TextureTypes.Color:
                    bkg = new SolidBrush(this.color1);
                    break;

                case TextureTypes.GradientColor:
                    PathGradientBrush pgb;
                    if (path == null)
                    {
                        pgb = new PathGradientBrush(new Point[] {
                        new Point(1,1),
                        new Point(Width, 1),
                        new Point (Width,Height),
                        new Point(1, Height)},
                            WrapMode.Tile);
                    }
                    else
                    {
                        pgb = new PathGradientBrush(path);
                    }

                    pgb.CenterColor = this.color2;
                    pgb.SurroundColors = new Color[] { this.color1 };


                    int b = externalCardBorderthickness.Value; // LE PB EST LA !!! IL FAUT TROUVER UN MOYEN DE CONNAITRE ICI LA TAILLE DE LA BORDURE...


                    float scaleW = (float)(this.Width - 2 * b) / (float)this.Width;
                    float scaleH = (float)(this.Height - 2 * b) / (float)this.Height;
                    pgb.FocusScales = new PointF(scaleW, scaleH);

                    bkg = pgb;

                    break;

                case TextureTypes.Image:
                    if ((resourcesDirectory != null) && (resourcesDirectory.Exists))
                    {
                        FileInfo[] files = resourcesDirectory.GetFiles(this.imageName + ".*", SearchOption.AllDirectories);

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
                                Image img = UtilsImageFile.LoadImage(fileName);
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

        public DirectoryInfo GetResourcessDirectory()
        {
            DirectoryInfo myresourcesDirectory = resourcesDirectory;

            if (!((myresourcesDirectory != null) && (myresourcesDirectory.Exists)))
            {
                throw new DirectoryNotFoundException();
            }

            return myresourcesDirectory;
        }

        public FileInfo GetResourceFile(string fileName)
        {
            FileInfo file = null;
            string searchPattern = fileName + ".*";

            FileInfo[] files = GetResourcessDirectory().GetFiles(searchPattern, SearchOption.AllDirectories);

            if (files.Length > 0)
            {
                file = files[0];
            }

            files = null;

            return file;
        }

        public Image GetResourceImage(string fileName)
        {
            Image img = null;

            FileInfo file = GetResourceFile(fileName);

            if (file != null && file.Exists)
            {
                img = UtilsImageFile.LoadImage(file.FullName);
            }

            return img;
        }

    }
}
