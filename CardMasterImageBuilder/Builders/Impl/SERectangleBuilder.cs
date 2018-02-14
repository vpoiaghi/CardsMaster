using CardMasterCard.Card;
using CardMasterImageBuilder.Builders;
using CardMasterImageBuilder.SkinElements;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;
using System;

namespace CardMasterImageBuilder.Builders.Impl
{
    public class SERectangleBuilder : AbstractBuilder
    {
        public override string TYPE { get { return "SERectangle"; } }

        public SERectangleBuilder(BuilderParameter builderParameter)
        {
            this.builderParameter = builderParameter;
        }

        public override SkinElement Build(JsonSkinItem item, JsonCard card)
        {
            JsonSkinsProject skinsProject = builderParameter.JsonSkinsProject;
            SERectangle skinElement = new SERectangle(builderParameter.ResourcesDirectory, item.X, item.Y, item.Width, item.Height, item.Comment);
            if(item.Background == "DYNAMIC")
            {
                skinElement.SetBackground(GetMatchingBackground(skinsProject,card));
            }
            else
            {
                skinElement.SetBackground(item.Background);
            }

            if (!String.IsNullOrEmpty(item.BorderColor))
            {
                skinElement.Border = GetBorderColor(skinsProject, card, item); 
            }
        
            return ManageShadow(skinElement, item);
        }
    }
}
