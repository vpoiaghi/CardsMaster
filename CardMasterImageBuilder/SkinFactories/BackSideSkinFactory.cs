using CardMasterCard.Card;
using CardMasterSkin.Elements;
using CardMasterSkin.Skins;
using System;
using System.Drawing;
using System.IO;

namespace CardMasterImageBuilder
{
    class BackSideFactory : SkinFactory
    {
        protected override Skin GetSkin()
        {
            Skin skin = new Skin(this.width, this.height, this.imagesDirectory, this.texturesDirectory);
            SkinElement skinElement;

            // Bordure
            skinElement = new SERoundedRectangle(skin, 0, 0, 744, 1038, 28);
            skinElement.SetBackground(Color.Black);
            skin.Elements.Add(skinElement);

            // Image
            skinElement = new SEImage(skin, 28, 28, 688, 982);
            ((SEImage)skinElement).NameAttribute = "BackSide";
            ((SEImage)skinElement).ResourceType = ResourceTypes.Image;
            skin.Elements.Add(skinElement);

            return skin;
        }
    }
}
