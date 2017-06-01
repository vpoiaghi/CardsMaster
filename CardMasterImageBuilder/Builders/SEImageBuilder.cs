using CardMasterSkin.Elements;
using CardMasterSkin.Skins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMasterImageBuilder.Converters
{
    public class SEImageBuilder
    {
        public static SEImage Build(Skin skin, JsonSkinItem item)
        {
            SEImage skinElement = new SEImage(skin, item.X, item.Y, item.Width, item.Height);
            skinElement.NameAttribute = item.NameAttribute;
            return skinElement;
        }
    }
}
