using CardMasterCard.Card;
using CardMasterImageBuilder.Builders;
using CardMasterImageBuilder.SkinElements;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;

namespace CardMasterImageBuilder.Builders.Impl
{
    public class SEImageBuilder : AbstractBuilder
    {
        public override string TYPE { get { return "SEImage"; } }

        public override SkinElement Build(BuilderParameter builderParameter)
        {
            JsonSkinItem item = builderParameter.JsonSkinItem;
            JsonSkinsProject skinsProject = builderParameter.JsonSkinsProject;
            JsonCard card = builderParameter.JsonCard;
            SEImage skinElement;
            if (item.Background == null)
            {
                skinElement = new SEImage(builderParameter.ResourcesDirectory, item.X, item.Y, item.Width, item.Height, item.Comment);
                skinElement.NameAttribute = item.NameAttribute;
            }else
            {
                skinElement = new SEImage(builderParameter.ResourcesDirectory, item.X, item.Y, item.Width, item.Height, item.Comment, item.Background);
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

            return ManageShadow(skinElement, item);
        }
    }
}
