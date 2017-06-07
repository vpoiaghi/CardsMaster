using CardMasterCard.Card;
using CardMasterImageBuilder.Builders;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace CardMasterImageBuilder
{
    public class SkinFactory
    {
        public const string RESOURCES_DIRECTORY_NAME = "Resources";

        protected ColorConverter colorConverter = new ColorConverter();

        protected JsonSkinsProject skinsProject = null;

        protected DirectoryInfo resourcesDirectory = null;
        protected Card card = null;

        public Skin GetSkin(Card card, FileInfo skinsFile, String skinName, SkinSide side)
        {
            Skin skin = null;

            skinsProject = JsonSkinsProject.LoadProject(skinsFile);

            this.card = card;

            if (skinsProject != null)
            {
                this.resourcesDirectory = new DirectoryInfo(Path.Combine( skinsFile.Directory.FullName, RESOURCES_DIRECTORY_NAME));

                skin = GetSkin(side);
            }

            return skin;
        }
        public enum SkinSide
        {
            FRONT,
            BACK
        }
        protected Skin GetSkin(SkinSide side)
        {
            JsonSkin jsonSkin;
            if (side.Equals(SkinSide.FRONT))
            {
                jsonSkin = skinsProject.Skins.Single(sk => sk.Name == card.FrontSkinName);
            }else
            {
                jsonSkin = skinsProject.Skins.Single(sk => sk.Name == card.BackSkinName);
            }
            
            Skin skin = new Skin(jsonSkin.Width, jsonSkin.Height, this.resourcesDirectory);
            SkinElement skinElement;

            foreach (JsonSkinItem item in jsonSkin.Items)
            {
                switch (item.Type)
                {
                    case "SERoundedRectangle":
                        skinElement = new SERoundedRectangleBuilder(skinsProject, card).Build(skin, item);
                        break;
                    case "SEImage":
                        skinElement = new SEImageBuilder(skinsProject, card).Build(skin,  item);
                        break;
                    case "SERectangle":
                        skinElement = new SERectangleBuilder(skinsProject, card).Build(skin, item);
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
