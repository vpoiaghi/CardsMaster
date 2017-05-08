using CardMasterCard.Card;
using CardMasterSkin.Skins;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace CardMasterImageBuilder
{
    public class Drawer
    {
        private Card card;
        private Skin skin;

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
            Bitmap img = null;

            if (this.skin != null)
            {
                img = new Bitmap(this.skin.Width, this.skin.Height);
                img.SetResolution(96, 96);

                Graphics g = Graphics.FromImage(img);

                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.CompositingMode = CompositingMode.SourceOver;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                foreach (SkinElement e in this.skin.Elements)
                {
                    e.Draw(g, this.card);
                }

                g.Dispose();
                g = null;

            }

            return img;
        }

    }
}
