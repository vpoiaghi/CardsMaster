﻿using CardMasterCard.Card;
using CardMasterImageBuilder.Builders;
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

            //SETextArea skinElement = new SETextArea(skin, item.X, item.Y, item.Width, item.Height, item.Comment, "<Nom>");
            SETextArea2 skinElement = new SETextArea2(builderParameter.ResourcesDirectory, item.X, item.Y, item.Width, item.Height, item.Comment, "<Nom>");

            if (item.Style == "Bold")
            {
                skinElement.TextFont = new Font(item.FontName, item.FontSize.Value, FontStyle.Bold);
            }else if (item.Style == "Bold,Italic" || item.Style =="Italic,Bold")
            {
                skinElement.TextFont = new Font(item.FontName, item.FontSize.Value, FontStyle.Bold | FontStyle.Italic);
            }

            if (item.FontColor!=null)
            {
                skinElement.FontColor = ConvertColorFromString(item.FontColor);
            }
            
            skinElement.TextAttribute = item.NameAttribute;
            if (item.VerticalAlign=="Center")
            {
                skinElement.TextVerticalAlign = VerticalAlignment.Center;
            }
            if (item.HorizontalAlign == "Center")
            {
                skinElement.TextAlign = HorizontalAlignment.Center;
            }else if (item.HorizontalAlign=="Left-WithIcon")
            {
                skinElement.TextAlign = HorizontalAlignment.LeftWithIcon;
            }
            if (item.VisibleConditionAttribute != null && item.VisibleConditionAttribute != "")
            {
                skinElement.Visible = IsAttributeNonEmpty(card, item.VisibleConditionAttribute);
            }
            if (item.PowerIconHeight.HasValue && item.PowerIconWidth.HasValue)
            {
                skinElement.LeftIconsSize =  new Size(item.PowerIconHeight.Value, item.PowerIconWidth.Value);
            }

            return ManageShadow(skinElement, item);
        }
    }
}
