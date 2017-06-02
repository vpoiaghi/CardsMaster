using CardMasterImageBuilder.Builders;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;
using System;
using System.Linq;

namespace CardMasterImageBuilder
{
    class BackSideFactory : SkinFactory
    {
        protected override Skin GetSkin()
        {
            JsonSkin jsonSkin = skinsProject.Skins.Single(sk => sk.Name == card.BackSkinName);
            Skin skin = new Skin(jsonSkin.Width, jsonSkin.Height, this.resourcesDirectory);
            SkinElement skinElement;
           
            foreach (JsonSkinItem item in jsonSkin.Items)
            {            
                switch(item.Type)
                {
                    case "SERoundedRectangle":
                        skinElement = new SERoundedRectangleBuilder(skinsProject,card).Build(skin, item);
                        break;
                    case "SEImage":
                        skinElement = new SEImageBuilder(skinsProject, card).Build(skin, item);
                        break;
                    default:
                        throw new Exception("Type #" + item.Type + " not found");
                }
                skin.Elements.Add(skinElement);
            }       
            return skin;
        }
    }
}
