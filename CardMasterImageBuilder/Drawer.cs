using CardMasterCard.Card;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;
using System;
using System.Drawing;
using System.IO;

namespace CardMasterImageBuilder
{
    public class Drawer
    {
        private JsonCard card;
        private Skin frontSkin;
        private Skin backSideSkin;

        private Bitmap img = null;
        private Graphics g = null;

        public DrawingQuality Quality { get; set; } = new DrawingQuality();

        public Drawer(JsonCard card, FileInfo skinsFile, String skinName)
        {
            try
            {
                this.card = card;
                this.frontSkin = new SkinFactory().GetSkin(this.card, skinsFile, skinName, SkinFactory.SkinSide.FRONT);
                this.backSideSkin = new SkinFactory().GetSkin(this.card, skinsFile, skinName, SkinFactory.SkinSide.BACK);
            }catch(SkinNotFoundException)
            {
               //DO NOTHING
            }
        }

        public Drawer(JsonCard card, JsonSkinsProject jsonSkinsProject, String skinsFilePath, String skinName)
        {
            try
            {
                this.card = card;
                this.frontSkin = new SkinFactory().GetSkin(this.card, jsonSkinsProject,skinsFilePath, skinName, SkinFactory.SkinSide.FRONT);
                this.backSideSkin = new SkinFactory().GetSkin(this.card, jsonSkinsProject, skinsFilePath, skinName, SkinFactory.SkinSide.BACK);
            }
            catch (SkinNotFoundException)
            {
                //DO NOTHING
            }
        }

        ~Drawer()
        {
            this.frontSkin = null;
            this.card = null;
            this.backSideSkin = null;
        }

        public Image DrawCard()
        {
            return DrawSkin(this.frontSkin);
        }

        public Image DrawBackCard()
        {
            return DrawSkin(this.backSideSkin);
        }

        private Image DrawSkin(Skin skin)
        {
            this.img = null;

            if (skin != null)
            {
                InitBasicImage(skin);

                foreach (SkinElement e in skin.Elements)
                {
                    e.Draw(this.g, this.card);
                }

                g.Dispose();
                g = null;
            }

            return this.img;
        }

        private void InitBasicImage(Skin skin)
        {
            this.img = new Bitmap(skin.Width, skin.Height);
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
