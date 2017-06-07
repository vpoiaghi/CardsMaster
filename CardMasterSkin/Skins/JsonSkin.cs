using System;
using System.Collections.Generic;

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
