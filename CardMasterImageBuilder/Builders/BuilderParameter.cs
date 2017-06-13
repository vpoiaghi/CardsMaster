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
        public DirectoryInfo ResourcesDirectory { get; set; }

        public BuilderParameter(DirectoryInfo resourcesDirectory, JsonSkinsProject jsonSkinsProject)
        {
            this.JsonSkinsProject = jsonSkinsProject;
            this.ResourcesDirectory = resourcesDirectory;
        }
    }
}
