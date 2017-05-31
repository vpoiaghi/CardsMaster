using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMasterSkin.Skins
{
    public class JsonSkin
    {
        public String Name { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public List<JsonSkinItem> Items { get; set; }
    }
}
