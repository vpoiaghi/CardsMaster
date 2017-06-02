using CardMasterCard.Card;
using CardMasterImageBuilder.Builders;
using CardMasterImageBuilder.SkinElements;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;

namespace CardMasterImageBuilder.Builders
{
    public class SECurvedRectangleBuilder : AbstractBuilder
    {
        private JsonSkinsProject _skinsProject;
        private Card _card;
        public SECurvedRectangleBuilder(JsonSkinsProject skinsProject, Card card)
        {
            _skinsProject = skinsProject;
            _card = card;
        }
        protected override SkinElement Initialize(Skin skin, JsonSkinItem item)
        {
            // Zone entête
            SECurvedRectangle skinElement = new SECurvedRectangle(skin, item.X, item.Y, item.Width, item.Height, item.CurveSize.Value);
            if(item.Background=="DYNAMIC-RARETE")
            {
                skinElement.SetBackground(GetMatchingRarityColor(_skinsProject,_card));
            }
            else
            {
                skinElement.SetBackground(item.Background);
            }
           
            if (item.BorderColor == "DYNAMIC")
            {
                skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(_skinsProject,_card), _skinsProject.BorderWidth.Value);
            }
            else
            {
                skinElement.Border = new SkinElementBorder(ConvertColorFromString(item.BorderColor), _skinsProject.BorderWidth.Value);
            }
           
            skin.Elements.Add(skinElement);

            return skinElement;
        }
    }
}
