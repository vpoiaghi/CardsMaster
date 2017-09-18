using CardMasterCard.Card;
using CardMasterImageBuilder.Builders;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

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
                try
                {
                    skin = GetSkin(jsonCard, side);
                }catch(SkinNotFoundException e)
                {
                    throw e;
                }
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
                        jsonSkinsProject.Skins.SingleOrDefault(sk => sk.Name == jsonCard.FrontSkinName) :
                        jsonSkin = jsonSkinsProject.Skins.SingleOrDefault(sk => sk.Name == jsonCard.BackSkinName);
            if(jsonSkin == null)
            {
                throw new SkinNotFoundException();
            }

            Skin skin = new Skin(jsonSkin.Width, jsonSkin.Height);

            foreach (JsonSkinItem jsonSkinItem in jsonSkin.Items)
            {
                skin.Elements.Add(BuilderRegister.getInstance().getBuilder(jsonSkinItem.Type).Build(jsonSkinItem, jsonCard));
            }

            return skin;
        }

    }

    [Serializable]
    internal class SkinNotFoundException : Exception
    {
        public SkinNotFoundException()
        {
        }

        public SkinNotFoundException(string message) : base(message)
        {
        }

        public SkinNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SkinNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
