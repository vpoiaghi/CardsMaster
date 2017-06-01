using CardMasterImageBuilder.Converters;
using CardMasterSkin.Elements;
using CardMasterSkin.Skins;
using System;
using System.Drawing;
using System.Linq;

namespace CardMasterImageBuilder.SkinFactories
{
    class FrontSideSkinFactory : SkinFactory
    {
        protected override Skin GetSkin()
        {
            JsonSkin jsonSkin = skinsProject.Skins.Single(sk => sk.Name == card.FrontSkinName);
            Skin skin = new Skin(jsonSkin.Width, jsonSkin.Height, this.resourcesDirectory);
            SkinElement skinElement;

            foreach (JsonSkinItem item in jsonSkin.Items)
            {
                switch (item.Type)
                {
                    case "SERoundedRectangle":
                        skinElement = new SERoundedRectangleBuilder(skinsProject,card).Build(skin, item);
                        break;
                    case "SEImage":
                        skinElement = new SEImageBuilder(skinsProject,card).Build(skin, item);
                        break;
                    case "SERectangle":
                        skinElement = new SERectangleBuilder(skinsProject,card).Build(skin, item);
                        break;
                    case "SECurvedRectangle":
                        skinElement = new SECurvedRectangleBuilder(skinsProject, card).Build(skin, item);
                        break;
                    case "SETextArea":
                        skinElement = new SETextAreaBuilder(skinsProject, card).Build(skin, item);
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
