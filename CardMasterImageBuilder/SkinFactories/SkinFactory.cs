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

        protected JsonSkinsProject jsonSkinsProject = null;

        public Skin GetSkin(JsonCard jsonCard, FileInfo skinsFile, String skinName, SkinSide side)
        {
            Skin skin = null;

            jsonSkinsProject = JsonSkinsProject.LoadProject(skinsFile);

            if (jsonSkinsProject != null)
            {
                DirectoryInfo resourcesDirectory = new DirectoryInfo(Path.Combine(skinsFile.Directory.FullName, RESOURCES_DIRECTORY_NAME));
                BuilderRegister.getInstance().Register(new BuilderParameter(resourcesDirectory, jsonSkinsProject));
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

            jsonSkin = side.Equals(SkinSide.FRONT) ?
                        jsonSkinsProject.Skins.Single(sk => sk.Name == jsonCard.FrontSkinName) :
                        jsonSkin = jsonSkinsProject.Skins.Single(sk => sk.Name == jsonCard.BackSkinName);

            Skin skin = new Skin(jsonSkin.Width, jsonSkin.Height);

            foreach (JsonSkinItem jsonSkinItem in jsonSkin.Items)
            {
                skin.Elements.Add(BuilderRegister.getInstance().getBuilder(jsonSkinItem.Type).Build(jsonSkinItem, jsonCard));
            }

            return skin;
        }

    }
}
