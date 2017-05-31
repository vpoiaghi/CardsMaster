using CardMasterCard.Card;
using CardMasterSkin.Elements;
using CardMasterSkin.Skins;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
namespace CardMasterImageBuilder
{
    class BackSideFactory : SkinFactory
    {
        protected override Skin GetSkin()
        {
            JsonSkin jsonSkin = skinsProject.Skins.Single(c => c.Name == card.SkinName);
            Skin skin = new Skin(jsonSkin.Width, jsonSkin.Height, this.imagesDirectory, this.texturesDirectory);
            SkinElement skinElement;
           
            foreach (JsonSkinItem item in jsonSkin.Items)
            {
                if(item.Type== "SERoundedRectangle")
                {
                    skinElement = new SERoundedRectangle(skin,item.X, item.Y, item.Width, item.Height, item.Radius);
                    skinElement.SetBackground(item.BackgroundColor);
                }
                else if (item.Type== "SEImage")
                {
                    skinElement = new SEImage(skin, item.X, item.Y, item.Width, item.Height);
                    ((SEImage)skinElement).NameAttribute = item.NameAttribute;
                    ((SEImage)skinElement).ResourceType = ResourceTypes.Texture;
                }
                else
                {
                    throw new Exception("Type not found");
                }
               
                skin.Elements.Add(skinElement);
            }       

            return skin;
        }
    }
}
