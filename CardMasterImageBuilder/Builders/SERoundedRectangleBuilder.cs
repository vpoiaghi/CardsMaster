﻿using CardMasterCard.Card;
using CardMasterImageBuilder.Builders;
using CardMasterImageBuilder.SkinElements;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;

namespace CardMasterImageBuilder.Builders
{
    public class SERoundedRectangleBuilder : AbstractBuilder
    {
      
        public SERoundedRectangleBuilder(JsonSkinsProject skinsProject, JsonCard card)
        {
            _skinsProject = skinsProject;
            _card = card;
        }

        protected override SkinElement Initialize(Skin skin, JsonSkinItem item)
        {
            SERoundedRectangle skinElement = new SERoundedRectangle(skin, item.X, item.Y, item.Width, item.Height, item.Comment, item.Radius.Value);
            skinElement.SetBackground(item.BackgroundColor);
            return skinElement;
        }
    }
}
