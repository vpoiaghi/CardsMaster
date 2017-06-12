using CardMasterCard.Card;
using CardMasterImageBuilder.Builders;
using CardMasterImageBuilder.Builders.Impl;
using CardMasterImageBuilder.Elements.TextFormater;
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

        protected JsonSkinsProject jsonSkinsProject = null;

        protected DirectoryInfo resourcesDirectory = null;

        public Skin GetSkin(JsonCard jsonCard, FileInfo skinsFile, String skinName, SkinSide side)
        {
            Skin skin = null;

            jsonSkinsProject = JsonSkinsProject.LoadProject(skinsFile);

            if (jsonSkinsProject != null)
            {
                this.resourcesDirectory = new DirectoryInfo(Path.Combine( skinsFile.Directory.FullName, RESOURCES_DIRECTORY_NAME));
                skin = GetSkin(jsonCard, side);
            }

            return skin;
        }
        public enum SkinSide
        {
            FRONT,
            BACK
        }
        protected Skin GetSkin(JsonCard jsonCard, SkinSide side)
        {
            JsonSkin jsonSkin;
            if (side.Equals(SkinSide.FRONT))
            {
                jsonSkin = jsonSkinsProject.Skins.Single(sk => sk.Name == jsonCard.FrontSkinName);
            }else
            {
                jsonSkin = jsonSkinsProject.Skins.Single(sk => sk.Name == jsonCard.BackSkinName);
            }
            
            Skin skin = new Skin(jsonSkin.Width, jsonSkin.Height, this.resourcesDirectory);

            foreach (JsonSkinItem jsonSkinItem in jsonSkin.Items)
            {
                BuilderParameter builderParameter = new BuilderParameter(skin,jsonSkinsProject,jsonSkinItem,jsonCard);
                skin.Elements.Add(BuilderRegister.getInstance().getBuilder(jsonSkinItem.Type).Build(builderParameter));  
            }

            return skin;
        }

    }
}
