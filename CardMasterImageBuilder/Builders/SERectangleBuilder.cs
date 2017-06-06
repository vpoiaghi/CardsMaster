using CardMasterCard.Card;
using CardMasterImageBuilder.Builders;
using CardMasterImageBuilder.SkinElements;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;

namespace CardMasterImageBuilder.Builders
{
    public class SERectangleBuilder : AbstractBuilder
    {
        private JsonSkinsProject _skinsProject;
        private Card _card;
        public SERectangleBuilder(JsonSkinsProject skinsProject, Card card)
        {
            _skinsProject = skinsProject;
            _card = card;
        }
        protected override SkinElement Initialize(Skin skin, JsonSkin jsonSkin, JsonSkinItem item)
        {
            SERectangle skinElement = new SERectangle(skin, item.X, item.Y, item.Width, item.Height);
            if(item.Background == "DYNAMIC")
            {
                skinElement.SetBackground(GetMatchingBackground(_skinsProject,_card));
            }
            else
            {
                skinElement.SetBackground(item.Background);
            }

            if (item.BorderColor != null)
            {
                if (item.BorderColor == "DYNAMIC")
                {
                    skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(_skinsProject, _card), _skinsProject.BorderWidth.Value);
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
