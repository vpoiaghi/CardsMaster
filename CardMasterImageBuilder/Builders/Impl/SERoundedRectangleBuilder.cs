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
       
        public override SkinElement Build(BuilderParameter builderParameter)
        {
            JsonSkinItem item = builderParameter.JsonSkinItem;
            SERoundedRectangle skinElement = new SERoundedRectangle(builderParameter.ResourcesDirectory, item.X, item.Y, item.Width, item.Height, item.Comment, item.Radius.Value);
            skinElement.SetBackground(item.BackgroundColor);
            return ManageShadow(skinElement, item);
        }
    }
}
