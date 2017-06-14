using CardMasterCard.Card;
using CardMasterImageBuilder.Converters;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;
using System;
using System.Drawing;

namespace CardMasterImageBuilder.Builders
{
    public abstract class AbstractBuilder : IBuilder
    {
        protected BuilderParameter builderParameter;
        public abstract string TYPE { get; }
        public abstract SkinElement Build(JsonSkinItem jsonSkinItem, JsonCard jsonCard);

        protected SkinElement ManageShadow(SkinElement skinElement, JsonSkinItem item)
        {
            if ((item.shadowAngle != null) && (item.shadowAngle != 0) && (item.shadowSize != 0))
            {
                skinElement.Shadow = new SkinElementShadow(item.shadowSize.Value, item.shadowAngle.Value);
            }

            return skinElement;
        }

        protected String GetMatchingBackground(JsonSkinsProject skinsProject, JsonCard card)
        {
            String attributeName = skinsProject.MapKindField[card.Kind];
            String attributeValue = getAttributeValueAsString(card,attributeName);
            return skinsProject.MapLibelleColor[attributeValue];
        }

        protected Boolean IsAttributeNonEmpty(JsonCard card,String attributeName)
        {
            return getAttributeValueAsString(card,attributeName) != "";
        }
        protected String getAttributeValueAsString(JsonCard card,String attributeName)
        {
            return (String)card.GetType().GetProperty(attributeName).GetValue(card, null);
        }

        protected Color GetMatchingRarityColor(JsonSkinsProject skinsProject, JsonCard card)
        {
            String stringColor = skinsProject.MapRareteColor.ContainsKey(card.Rank) ? skinsProject.MapRareteColor[card.Rank] : skinsProject.MapRareteColor[""];
            return ConvertColorFromString(stringColor);
        }
        protected Color ConvertColorFromString(String color)
        {
            return CustomColorConverter.Instance.ConvertFromString(color);
        }
        protected Color GetMatchingBorderColor(JsonSkinsProject skinsProject, JsonCard card)
        {
            String attributeName = skinsProject.MapKindField[card.Kind];
            String attributeValue = getAttributeValueAsString(card,attributeName);
            String rgbCode = skinsProject.MapLibelleBorderColor[attributeValue];
            return ConvertColorFromString(rgbCode);
        }

      
    }
}
