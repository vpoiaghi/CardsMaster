using CardMasterCard.Card;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMasterImageBuilder.Builders
{
    public interface IBuilder
    {
        String TYPE { get; }

        SkinElement Build(JsonSkinsProject skinsProject, JsonCard card, Skin skin, JsonSkinItem item);
    }


}
