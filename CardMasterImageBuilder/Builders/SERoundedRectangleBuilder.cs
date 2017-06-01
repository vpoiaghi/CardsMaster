using CardMasterCard.Card;
using CardMasterSkin.Elements;
using CardMasterSkin.Skins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMasterImageBuilder.Converters
{
    public class SERoundedRectangleBuilder
    {
        private SkinsProject _skinsProject;
        private Card _card;

        public SERoundedRectangleBuilder(SkinsProject skinsProject, Card card)
        {
            _skinsProject = skinsProject;
            _card = card;
        }

        public SERoundedRectangle Build(Skin skin, JsonSkinItem item)
        {
            SERoundedRectangle skinElement = new SERoundedRectangle(skin, item.X, item.Y, item.Width, item.Height, item.Radius);
            skinElement.SetBackground(item.BackgroundColor);
            return skinElement;
        }
    }
}
