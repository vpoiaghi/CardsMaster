using CardMasterCard.Card;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;
using System;
using System.Drawing;

namespace CardMasterImageBuilder.Builders
{
    public abstract class AbstractBuilder
    {
        protected abstract SkinElement Initialize(Skin skin, JsonSkinItem item);

        public SkinElement Build(Skin skin, JsonSkinItem item)
        {
            SkinElement skinElement = Initialize(skin, item);

            if ((item.shadowAngle != null) && (item.shadowAngle != 0) && (item.shadowSize != 0))
            {
                skinElement.Shadow = new SkinElementShadow(item.shadowSize.Value, item.shadowAngle.Value);
            }

            return skinElement;
        }

        protected String GetMatchingBackground(JsonSkinsProject skinsProject,Card card)
        {
            String attributeName = skinsProject.MapKindField[card.Kind];
            String attributeValue = getAttributeValueAsString(card,attributeName);
            return skinsProject.MapLibelleColor[attributeValue];
        }

        protected Boolean IsAttributeNonEmpty(Card card,String attributeName)
        {
            return getAttributeValueAsString(card,attributeName) != "";
        }
        protected String getAttributeValueAsString(Card card,String attributeName)
        {
            return (String)card.GetType().GetProperty(attributeName).GetValue(card, null);
        }

        protected Color GetMatchingRarityColor(JsonSkinsProject skinsProject, Card card)
        {
            return ConvertColorFromString(skinsProject.MapRareteColor[card.Rank]);
        }
        protected Color ConvertColorFromString(String color)
        {
            return CustomColorConverter.Instance.ConvertFromString(color);
        }
        protected Color GetMatchingBorderColor(JsonSkinsProject skinsProject, Card card)
        {
            String attributeName = skinsProject.MapKindField[card.Kind];
            String attributeValue = getAttributeValueAsString(card,attributeName);
            String rgbCode = skinsProject.MapLibelleBorderColor[attributeValue];
            return ConvertColorFromString(rgbCode);
        }
    }
}
