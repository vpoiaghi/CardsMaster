using CardMasterCard.Card;
using CardMasterImageBuilder.SkinElements;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;
using System.Drawing;

namespace CardMasterImageBuilder.Builders.Impl
{
    public class SETextAreaBuilder : AbstractBuilder
    {
        public override string TYPE { get { return "SETextArea"; } }

        public SETextAreaBuilder(BuilderParameter builderParameter)
        {
            this.builderParameter = builderParameter;
        }

        public override SkinElement Build(JsonSkinItem item, JsonCard card)
        {

            SETextArea skinElement = new SETextArea(builderParameter.ResourcesDirectory, item.X, item.Y, item.Width, item.Height, item.Comment, "<Texte>");

            skinElement.TextFont = new Font(item.FontName, item.FontSize.Value, GetStyle(item.Style));
            skinElement.TextAttribute = item.NameAttribute;
            skinElement.TextAlign = GetHorizontalAlignment(item.HorizontalAlign);
            skinElement.TextVerticalAlign = GetVerticalAlignment(item.VerticalAlign);

            if (! string.IsNullOrEmpty(item.VisibleConditionAttribute))
            {
                skinElement.Visible = IsAttributeNonEmpty(card, item.VisibleConditionAttribute);
            }

            if (item.FontColor != null)
            {
                skinElement.FontColor = ConvertColorFromString(item.FontColor);
            }

            if (item.WithFontBorder.HasValue)
            {
                skinElement.WithFontBorder = item.WithFontBorder.Value;
            }

            if (item.PowerIconHeight.HasValue && item.PowerIconWidth.HasValue)
            {
                skinElement.LeftIconsSize = new Size(item.PowerIconHeight.Value, item.PowerIconWidth.Value);
            }

            return ManageShadow(skinElement, item);


            //if (item.Style == "Bold")
            //{
            //    skinElement.TextFont = new Font(item.FontName, item.FontSize.Value, FontStyle.Bold);
            //}else if(item.Style=="Italic")
            //{
            //    skinElement.TextFont = new Font(item.FontName, item.FontSize.Value, FontStyle.Italic);
            //}
            //else if (item.Style == "Bold,Italic" || item.Style =="Italic,Bold")
            //{
            //    skinElement.TextFont = new Font(item.FontName, item.FontSize.Value, FontStyle.Bold | FontStyle.Italic);
            //}

            //if (item.FontColor!=null)
            //{
            //    skinElement.FontColor = ConvertColorFromString(item.FontColor);
            //}

            //skinElement.TextAttribute = item.NameAttribute;

            //if (item.VerticalAlign=="Center")
            //{
            //    skinElement.TextVerticalAlign = VerticalAlignment.Center;
            //}
            //if (item.HorizontalAlign == "Center")
            //{
            //    skinElement.TextAlign = HorizontalAlignment.Center;
            //}else if (item.HorizontalAlign=="Left-WithIcon")
            //{
            //    skinElement.TextAlign = HorizontalAlignment.LeftWithIcon;
            //}
            //if (item.VisibleConditionAttribute != null && item.VisibleConditionAttribute != "")
            //{
            //    skinElement.Visible = IsAttributeNonEmpty(card, item.VisibleConditionAttribute);
            //}
            //if (item.PowerIconHeight.HasValue && item.PowerIconWidth.HasValue)
            //{
            //    skinElement.LeftIconsSize =  new Size(item.PowerIconHeight.Value, item.PowerIconWidth.Value);
            //}
            //if (item.WithFontBorder.HasValue && item.WithFontBorder.Value == true)
            //{
            //    skinElement.WithFontBorder = true;
            //}else
            //{
            //    skinElement.WithFontBorder = false;
            //}

            //return ManageShadow(skinElement, item);
        }

        private FontStyle GetStyle(string itemFontStyle)
        {
            FontStyle style = FontStyle.Regular;

            itemFontStyle = itemFontStyle.ToLower();

            if (itemFontStyle.Contains("bold"))
            {
                style |= FontStyle.Bold;
            }

            if (itemFontStyle.Contains("italic"))
            {
                style |= FontStyle.Italic;
            }

            if (itemFontStyle.Contains("underline"))
            {
                style |= FontStyle.Underline;
            }

            if (itemFontStyle.Contains("strikeout"))
            {
                style |= FontStyle.Strikeout;
            }

            return style;
        }

        private VerticalAlignment GetVerticalAlignment(string itemVAlignement)
        {
            VerticalAlignment vAlignment = VerticalAlignment.Top;

            if (itemVAlignement != null)
            {
                switch (itemVAlignement.Trim().ToLower())
                {
                    case "top":
                        vAlignment = VerticalAlignment.Top;
                        break;
                    case "center":
                        vAlignment = VerticalAlignment.Center;
                        break;
                    case "bottom":
                        vAlignment = VerticalAlignment.Bottom;
                        break;
                }
            }

            return vAlignment;
        }

        private HorizontalAlignment GetHorizontalAlignment(string itemHAlignement)
        {
            HorizontalAlignment hAlignment = HorizontalAlignment.Left;

            if (itemHAlignement != null)
            {
                switch (itemHAlignement.Trim().ToLower())
                {
                    case "left":
                        hAlignment = HorizontalAlignment.Left;
                        break;
                    case "left-withicon":
                        hAlignment = HorizontalAlignment.LeftWithIcon;
                        break;
                    case "center":
                        hAlignment = HorizontalAlignment.Center;
                        break;
                    case "right":
                        hAlignment = HorizontalAlignment.Right;
                        break;
                }
            }

            return hAlignment;
        }

    }
}
