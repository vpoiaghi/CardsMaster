using System;
using CardMasterCard.Card;
using CardMasterImageBuilder.Builders;
using CardMasterImageBuilder.SkinElements;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;

namespace CardMasterImageBuilder.Builders.Impl
{
    public class SECurvedRectangleBuilder : AbstractBuilder
    {
        
        public SECurvedRectangleBuilder(BuilderParameter builderParameter)
        {
            this.builderParameter = builderParameter;
        }

        public override string TYPE { get { return "SECurvedRectangle";}}

        public override SkinElement Build(JsonSkinItem item, JsonCard card)
        {
            JsonSkinsProject skinsProject = builderParameter.JsonSkinsProject;

            // Zone entête
            SECurvedRectangle skinElement = new SECurvedRectangle(builderParameter.ResourcesDirectory, item.X, item.Y, item.Width, item.Height, item.Comment, item.CurveSize.Value);
            if(item.Background=="DYNAMIC-RARETE")
            {
                skinElement.SetBackground(GetMatchingRarityColor(skinsProject,card));
            }
            else
            {
                skinElement.SetBackground(item.Background);
            }
           
            if (item.BorderColor == "DYNAMIC")
            {
                skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(skinsProject,card), skinsProject.BorderWidth.Value);
            }
            else
            {
                skinElement.Border = new SkinElementBorder(ConvertColorFromString(item.BorderColor), skinsProject.BorderWidth.Value);
            }
   
            return ManageShadow(skinElement, item);
        }
    }
}
