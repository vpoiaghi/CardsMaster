using CardMasterCard.Card;
using CardMasterImageBuilder.Builders;
using CardMasterSkin.Elements;
using CardMasterSkin.Skins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMasterImageBuilder.Converters
{
    public class SERectangleBuilder : AbstractBuilder
    {
        private SkinsProject _skinsProject;
        private Card _card;
        public SERectangleBuilder(SkinsProject skinsProject, Card card)
        {
            _skinsProject = skinsProject;
            _card = card;
        }
        public SERectangle Build(Skin skin, JsonSkinItem item)
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
                    skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(_skinsProject, _card), _skinsProject.BorderWidth);
                }
                else
                {
                    skinElement.Border = new SkinElementBorder(ConvertColorFromString(item.BorderColor), _skinsProject.BorderWidth);
                }

            }

            return skinElement;
        }
    }
}
