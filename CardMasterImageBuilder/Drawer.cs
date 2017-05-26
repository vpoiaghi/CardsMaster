using CardMasterCard.Card;
using CardMasterSkin.Elements;
using CardMasterSkin.Skins;
using System;
using System.Drawing;
using System.IO;

namespace CardMasterImageBuilder
{
    public class Drawer
    {
        private Card card;
        private Skin skin;

        private Bitmap img = null;
        private Graphics g = null;

        public DrawingQuality Quality { get; set; } = new DrawingQuality();

        public Drawer(Card card, FileInfo skinsFile, String skinName)
        {
            this.card = card;
            this.skin = SkinFactory.GetSkin(this.card, skinsFile, skinName);
        }

        ~Drawer()
        {
            this.skin = null;
            this.card = null;
        }

        public Image DrawCard()
        {
            this.img = null;

            if (this.skin != null)
            {
                InitBasicImage();

                foreach (SkinElement e in this.skin.Elements)
                {
                    e.Draw(g, this.card);
                }

                g.Dispose();
                g = null;
            }

            return this.img;
        }

        public Image DrawCardBackground()
        {
            this.img = null;

            if (this.skin != null)
            {
                InitBasicImage();

                // Bordure
                SkinElement skinElement = new SERoundedRectangle(skin, 0, 0, this.skin.Width, this.skin.Height, 28);
                skinElement.SetBackground(Color.Black);
                skinElement.Draw(g, this.card);

                // Image
                skinElement = new SEImage(skin, 28, 28, 688, 982);
                ((SEImage)skinElement).NameAttribute = "BackSide";
                ((SEImage)skinElement).ResourceType = ResourceTypes.Image;
                skinElement.Draw(g, this.card);

                g.Dispose();
                g = null;
            }

            return this.img;

        }

        private void InitBasicImage()
        {
            this.img = new Bitmap(this.skin.Width, this.skin.Height);
            this.img.SetResolution(300, 300);

            this.g = Graphics.FromImage(img);

            this.g.SmoothingMode = this.Quality.SmoothingMode;
            this.g.TextRenderingHint = this.Quality.TextRenderingHint;
            this.g.CompositingQuality = this.Quality.CompositingQuality;
            this.g.CompositingMode = this.Quality.CompositingMode;
            this.g.InterpolationMode = this.Quality.InterpolationMode;
            this.g.PixelOffsetMode = this.Quality.PixelOffsetMode;
        }
    }
}
