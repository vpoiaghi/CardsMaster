using CardMasterCard.Card;
using CardMasterImageBuilder.Skins;
using CardMasterSkin.Skins;

namespace CardMasterImageBuilder.Builders.Impl
{
    class BackgroundBuilder
    {
        public SkinTexture Build(JsonSkinItem item, JsonCard card, JsonSkinsProject skinsProject)
        {
            SkinTexture skinBackground = new SkinTexture();

            string itemBg = item.Background;

            if (itemBg == "DYNAMIC-RARETE")
            {
                skinBackground.color1 = GetMatchingRarityColor(skinsProject, card);
                skinBackground.textureType = SkinTexture.TextureTypes.Color;
            }
            else if(itemBg.StartsWith("#"))
            {
                skinBackground.color1 = (Color)colorConverter.ConvertFromString(itemBg);
                skinBackground.textureType = SkinTexture.TextureTypes.Color;
            }
            else
            {
                skinBackground.imageName = itemBg;
                skinBackground.textureType = SkinTexture.TextureTypes.Image;
            }


            return skinBackground;
        }
    }
}
