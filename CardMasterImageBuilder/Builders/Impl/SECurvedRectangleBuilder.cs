using System;
using CardMasterCard.Card;
using CardMasterImageBuilder.Builders;
using CardMasterImageBuilder.SkinElements;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;

namespace CardMasterImageBuilder.Builders.Impl
{
    public class SECurvedRectangleBuilder : AbstractBuilder,IBuilder
    {
        public string TYPE { get { return "SECurvedRectangle";}}

        public SECurvedRectangleBuilder(JsonSkinsProject skinsProject, JsonCard card)
        {
            _skinsProject = skinsProject;
            _card = card;
        }

       

        protected override SkinElement Initialize(Skin skin, JsonSkinItem item)
        {
            // Zone entête
            SECurvedRectangle skinElement = new SECurvedRectangle(skin, item.X, item.Y, item.Width, item.Height, item.Comment, item.CurveSize.Value);
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

            return skinElement;
        }
    }
}
