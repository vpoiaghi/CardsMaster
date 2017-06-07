using CardMasterCard.Card;
using CardMasterImageBuilder.Builders;
using CardMasterImageBuilder.SkinElements;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;
using System.Drawing;

namespace CardMasterImageBuilder.Builders
{
    public class SETextAreaBuilder : AbstractBuilder
    {
        private JsonSkinsProject _skinsProject;
        private Card _card;
        public SETextAreaBuilder(JsonSkinsProject skinsProject, Card card)
        {
            _skinsProject = skinsProject;
            _card = card;
        }
        protected override SkinElement Initialize(Skin skin, JsonSkinItem item)
        {
            //SETextArea skinElement = new SETextArea(skin, item.X, item.Y, item.Width, item.Height, "<Nom>");
            SETextArea2 skinElement = new SETextArea2(skin, item.X, item.Y, item.Width, item.Height, "<Nom>", item.PowerIconHeight, item.PowerIconWidth);
            if (item.Style == "Bold")
            {
                skinElement.TextFont = new Font(item.FontName, item.FontSize.Value, FontStyle.Bold);
            }else if (item.Style == "Bold,Italic" || item.Style =="Italic,Bold")
            {
                skinElement.TextFont = new Font(item.FontName, item.FontSize.Value, FontStyle.Bold | FontStyle.Italic);
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
                skinElement.Visible = IsAttributeNonEmpty(_card, item.VisibleConditionAttribute);
            }

            return skinElement;
        }
    }
}
