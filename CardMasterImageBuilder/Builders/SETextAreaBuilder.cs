using CardMasterCard.Card;
using CardMasterImageBuilder.Builders;
using CardMasterSkin.Elements;
using CardMasterSkin.Skins;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMasterImageBuilder.Converters
{
    public class SETextAreaBuilder : AbstractBuilder
    {
        private SkinsProject _skinsProject;
        private Card _card;
        public SETextAreaBuilder(SkinsProject skinsProject, Card card)
        {
            _skinsProject = skinsProject;
            _card = card;
        }
        public SETextArea Build(Skin skin, JsonSkinItem item)
        {
            // Zone entête
            SETextArea skinElement = new SETextArea(skin, item.X, item.Y, item.Width, item.Height, "<Nom>");
            if(item.Style == "Bold")
            {
                skinElement.TextFont = new Font(item.FontName, item.FontSize, FontStyle.Bold);
            }else if (item.Style == "Bold,Italic" || item.Style =="Italic,Bold")
            {
                skinElement.TextFont = new Font(item.FontName, item.FontSize, FontStyle.Bold | FontStyle.Italic);
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

            skin.Elements.Add(skinElement);

            return skinElement;
        }
    }
}
