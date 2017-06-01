using CardMasterCard.Card;
using CardMasterSkin.Skins;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMasterImageBuilder.Builders
{
    public class AbstractBuilder
    {
        protected String GetMatchingBackground(SkinsProject skinsProject,Card card)
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

        protected Color GetMatchingRarityColor(SkinsProject skinsProject, Card card)
        {
            return ConvertColorFromString(skinsProject.MapRareteColor[card.Rank]);
        }
        protected Color ConvertColorFromString(String color)
        {
            return CustomColorConverter.Instance.ConvertFromString(color);
        }
        protected Color GetMatchingBorderColor(SkinsProject skinsProject, Card card)
        {
            String attributeName = skinsProject.MapKindField[card.Kind];
            String attributeValue = getAttributeValueAsString(card,attributeName);
            String rgbCode = skinsProject.MapLibelleBorderColor[attributeValue];
            return ConvertColorFromString(rgbCode);
        }
    }
}
