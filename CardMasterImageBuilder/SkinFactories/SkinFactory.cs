using CardMasterCard.Card;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;
using System;
using System.Drawing;
using System.IO;

namespace CardMasterImageBuilder
{
    public abstract class SkinFactory
    {
        public const string RESOURCES_DIRECTORY_NAME = "Resources";

        protected ColorConverter colorConverter = new ColorConverter();

        protected SkinsProject skinsProject = null;
        protected DirectoryInfo resourcesDirectory = null;
        protected Card card = null;
        protected int width = 0;
        protected int height = 0;


        protected abstract Skin GetSkin();

        public Skin GetSkin(Card card, FileInfo skinsFile, String skinName)
        {
            Skin skin = null;

            // 300 ppp 
            this.width = 744;
            this.height = 1038;
            // --> 6.3 x 8.79 cm

            this.skinsProject = SkinsProject.LoadProject(skinsFile);
            this.card = card;

            if (skinsProject != null)
            {
                this.resourcesDirectory = new DirectoryInfo(Path.Combine( skinsFile.Directory.FullName, RESOURCES_DIRECTORY_NAME));

                skin = GetSkin();
            }

            return skin;
        }

    }
}
