using CardMasterCard.Card;
using CardMasterImageBuilder.Builders;
using CardMasterImageBuilder.SkinElements;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;

namespace CardMasterImageBuilder.Converters
{
    public class SECurvedRectangleBuilder : AbstractBuilder
    {
        private SkinsProject _skinsProject;
        private Card _card;
        public SECurvedRectangleBuilder(SkinsProject skinsProject, Card card)
        {
            _skinsProject = skinsProject;
            _card = card;
        }
        public SECurvedRectangle Build(Skin skin, JsonSkinItem item)
        {
            // Zone entête
            SECurvedRectangle skinElement = new SECurvedRectangle(skin, item.X, item.Y, item.Width, item.Height, item.CurveSize);
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
                skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(_skinsProject,_card), _skinsProject.BorderWidth);
            }
            else
            {
                skinElement.Border = new SkinElementBorder(ConvertColorFromString(item.BorderColor), _skinsProject.BorderWidth);
            }
           
            skin.Elements.Add(skinElement);

            return skinElement;
        }
    }
}
