using CardMasterCard.Card;
using CardMasterImageBuilder.Builders;
using CardMasterImageBuilder.SkinElements;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;

namespace CardMasterImageBuilder.Builders
{
    public class SERoundedRectangleBuilder : AbstractBuilder
    {
        private JsonSkinsProject _skinsProject;
        private Card _card;

        public SERoundedRectangleBuilder(JsonSkinsProject skinsProject, Card card)
        {
            _skinsProject = skinsProject;
            _card = card;
        }

        protected override SkinElement Initialize(Skin skin, JsonSkinItem item)
        {
            SERoundedRectangle skinElement = new SERoundedRectangle(skin, item.X, item.Y, item.Width, item.Height, item.Radius.Value);
            skinElement.SetBackground(item.BackgroundColor);
            return skinElement;
        }
    }
}
