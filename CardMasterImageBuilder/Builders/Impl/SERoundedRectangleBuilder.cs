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
       
        public override SkinElement Build(JsonSkinsProject skinsProject, JsonCard card,Skin skin, JsonSkinItem item)
        {
            SERoundedRectangle skinElement = new SERoundedRectangle(skin, item.X, item.Y, item.Width, item.Height, item.Comment, item.Radius.Value);
            skinElement.SetBackground(item.BackgroundColor);
            return ManageShadow(skinElement, item);
        }
    }
}
