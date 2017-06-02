using CardMasterCard.Card;
using CardMasterImageBuilder.SkinFactories;
using CardMasterImageBuilder.Skins;
using System;
using System.Drawing;
using System.IO;

namespace CardMasterImageBuilder
{
    public class Drawer
    {
        private Card card;
        private Skin skin;
        private Skin backSideSkin;

        private Bitmap img = null;
        private Graphics g = null;

        public DrawingQuality Quality { get; set; } = new DrawingQuality();

        public Drawer(Card card, FileInfo skinsFile, String skinName)
        {
            this.card = card;
            this.skin = (new FrontSideSkinFactory()).GetSkin(this.card, skinsFile, skinName);
            this.backSideSkin = (new BackSideFactory()).GetSkin(this.card, skinsFile, skinName);
        }

        ~Drawer()
        {
            this.skin = null;
            this.card = null;
        }

        public Image DrawCard()
        {
            return DrawSkin(this.skin);
        }

        public Image DrawBackSideSkin()
        {
            return DrawSkin(this.backSideSkin);
        }

        public Image DrawSkin(Skin skin)
        {
            this.img = null;

            if (skin != null)
            {
                InitBasicImage();

                foreach (SkinElement e in skin.Elements)
                {
                    e.Draw(g, this.card);
                }

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
