using CardMasterCard.Card;
using CardMasterImageBuilder.Builders;
using CardMasterImageBuilder.SkinElements;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;

namespace CardMasterImageBuilder.Builders.Impl
{
    public class SERectangleBuilder : AbstractBuilder, IBuilder
    {
        public string TYPE { get { return "SERectangle"; } }
        public SERectangleBuilder()
        {
         
        }
        protected override SkinElement Initialize(JsonSkinsProject skinsProject, JsonCard card,Skin skin, JsonSkinItem item)
        {
            SERectangle skinElement = new SERectangle(skin, item.X, item.Y, item.Width, item.Height, item.Comment);
            if(item.Background == "DYNAMIC")
            {
                skinElement.SetBackground(GetMatchingBackground(skinsProject,card));
            }
            else
            {
                skinElement.SetBackground(item.Background);
            }

            if (item.BorderColor != null)
            {
                if (item.BorderColor == "DYNAMIC")
                {
                    skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(skinsProject, card), skinsProject.BorderWidth.Value);
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
