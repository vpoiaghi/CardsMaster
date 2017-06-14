using CardMasterCard.Card;
using CardMasterImageBuilder.Builders;
using CardMasterImageBuilder.SkinElements;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;

namespace CardMasterImageBuilder.Builders.Impl
{
    public class SERoundedRectangleBuilder : AbstractBuilder
    {
        public override string TYPE { get { return "SERoundedRectangle"; } }

        public SERoundedRectangleBuilder(BuilderParameter builderParameter)
        {
            this.builderParameter = builderParameter;
        }

        public override SkinElement Build(JsonSkinItem item, JsonCard card)
        {
            SERoundedRectangle skinElement = new SERoundedRectangle(builderParameter.ResourcesDirectory, item.X, item.Y, item.Width, item.Height, item.Comment, item.Radius.Value);
            skinElement.SetBackground(item.BackgroundColor);
            return ManageShadow(skinElement, item);
        }
    }
}
