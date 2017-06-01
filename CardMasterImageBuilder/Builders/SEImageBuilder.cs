using CardMasterCard.Card;
using CardMasterImageBuilder.Builders;
using CardMasterSkin.Elements;
using CardMasterSkin.Skins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMasterImageBuilder.Converters
{
    public class SEImageBuilder : AbstractBuilder
    {
        private SkinsProject _skinsProject;
        private Card _card;

        public SEImageBuilder(SkinsProject skinsProject, Card card)
        {
            _skinsProject = skinsProject;
            _card = card;
        }

        public SEImage Build(Skin skin, JsonSkinItem item)
        {
            SEImage skinElement;
            if (item.Background == null)
            {
                skinElement = new SEImage(skin, item.X, item.Y, item.Width, item.Height);
                skinElement.NameAttribute = item.NameAttribute;
            }else
            {
                skinElement = new SEImage(skin, item.X, item.Y, item.Width, item.Height,item.Background);
            }
            if (item.VisibleConditionAttribute!= null && item.VisibleConditionAttribute!="")
            {
                skinElement.Visible = IsAttributeNonEmpty(_card,item.VisibleConditionAttribute);
            }
            if (item.BorderColor!=null)
            {
                if(item.BorderColor=="DYNAMIC")
                {
                    skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(_skinsProject,_card), _skinsProject.BorderWidth);
                }
                else
                {
                    skinElement.Border = new SkinElementBorder(ConvertColorFromString(item.BorderColor), _skinsProject.BorderWidth);
                }
               
            }
            return skinElement;
        }
    }
}
