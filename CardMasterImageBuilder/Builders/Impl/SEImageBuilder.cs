using CardMasterCard.Card;
using CardMasterImageBuilder.Builders;
using CardMasterImageBuilder.SkinElements;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;

namespace CardMasterImageBuilder.Builders.Impl
{
    public class SEImageBuilder : AbstractBuilder, IBuilder
    {
        public string TYPE { get { return "SEImage"; } }
        public SEImageBuilder()
        {  
        }

        protected override SkinElement Initialize(JsonSkinsProject skinsProject, JsonCard card,Skin skin, JsonSkinItem item)
        {
            SEImage skinElement;
            if (item.Background == null)
            {
                skinElement = new SEImage(skin, item.X, item.Y, item.Width, item.Height, item.Comment);
                skinElement.NameAttribute = item.NameAttribute;
            }else
            {
                skinElement = new SEImage(skin, item.X, item.Y, item.Width, item.Height, item.Comment, item.Background);
            }
            if (item.VisibleConditionAttribute!= null && item.VisibleConditionAttribute!="")
            {
                skinElement.Visible = IsAttributeNonEmpty(card, item.VisibleConditionAttribute);
            }
            if (item.BorderColor!=null)
            {
                if(item.BorderColor=="DYNAMIC")
                {
                    skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(skinsProject,card), skinsProject.BorderWidth.Value);
                }
                else
                {
                    skinElement.Border = new SkinElementBorder(ConvertColorFromString(item.BorderColor), skinsProject.BorderWidth.Value);
                }
               
            }
            return skinElement;
        }
    }
}
