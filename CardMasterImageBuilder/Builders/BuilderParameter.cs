using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardMasterCard.Card;

namespace CardMasterImageBuilder.Builders
{
    public class BuilderParameter
    {
        public JsonSkinsProject JsonSkinsProject { get; set; }
        public JsonSkinItem JsonSkinItem { get; set; }
        public JsonCard JsonCard { get; set; }
        public DirectoryInfo ResourcesDirectory { get; set; }

       

        public BuilderParameter(Skin skin, JsonSkinsProject jsonSkinsProject, JsonSkinItem jsonSkinItem, JsonCard jsonCard)
        {
            this.JsonSkinsProject = jsonSkinsProject;
            this.JsonSkinItem = jsonSkinItem;
            this.JsonCard = jsonCard;
            this.ResourcesDirectory = skin.ResourcesDirectory;
        }
    }
}
