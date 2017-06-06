using CardMasterCard.Card;
using CardMasterImageBuilder.Builders;
using CardMasterImageBuilder.SkinElements;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;

namespace CardMasterImageBuilder.Builders
{
    public class SEImageBuilder : AbstractBuilder
    {
        private JsonSkinsProject _skinsProject;
        private Card _card;

        public SEImageBuilder(JsonSkinsProject skinsProject, Card card)
        {
            _skinsProject = skinsProject;
            _card = card;
        }

        protected override SkinElement Initialize(Skin skin, JsonSkin jsonSkin, JsonSkinItem item)
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
                    skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(_skinsProject,_card), _skinsProject.BorderWidth.Value);
                }
                else
                {
                    skinElement.Border = new SkinElementBorder(ConvertColorFromString(item.BorderColor), _skinsProject.BorderWidth.Value);
                }
               
            }
            return skinElement;
        }
    }
}
