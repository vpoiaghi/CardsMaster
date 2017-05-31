using CardMasterSkin.Elements;
using CardMasterSkin.Skins;
using System.Drawing;

namespace CardMasterImageBuilder.SkinFactories
{
    class FrontSideSkinFactory : SkinFactory
    {
        protected override Skin GetSkin()
        {
            Skin skin = new Skin(this.width, this.height, this.imagesDirectory, this.texturesDirectory);
            SkinElement skinElement;

            // Bordures
            skinElement = new SERoundedRectangle(skin, 0, 0, 744, 1038, 28);
            skinElement.SetBackground(Color.Black);
            skin.Elements.Add(skinElement);

            // Texture de fond
            skinElement = new SERectangle(skin, 28, 28, 688, 982);
            skinElement.SetBackground(GetMatchingBackground());
            skin.Elements.Add(skinElement);

            // Zone entête
            skinElement = new SECurvedRectangle(skin, 40, 40, 664, 80, 18);
            skinElement.SetBackground("Pierre 01");
            skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(), skinsProject.BorderWidth);
            skin.Elements.Add(skinElement);

            skinElement = new SETextArea(skin, 58, 40, 628, 80, "<Nom>");
            ((SETextArea)skinElement).TextFont = new Font("Bell MT", 10, FontStyle.Bold);
            ((SETextArea)skinElement).TextAttribute = "Name";
            ((SETextArea)skinElement).TextVerticalAlign = VerticalAlignment.Center;
            skin.Elements.Add(skinElement);

            // Zone de coût
            skinElement = new SEImage(skin, 620, 50, 60, 60, "mana_circle");
            skin.Elements.Add(skinElement);

            skinElement = new SETextArea(skin, 620, 50, 60, 60, "?");
            ((SETextArea)skinElement).TextFont = new Font("Bell MT", 10, FontStyle.Bold);
            ((SETextArea)skinElement).TextAttribute = "Cost";
            ((SETextArea)skinElement).TextAlign = HorizontalAlignment.Center;
            ((SETextArea)skinElement).TextVerticalAlign = VerticalAlignment.Center;
            skin.Elements.Add(skinElement);

            // Image
            skinElement = new SEImage(skin, 58, 120, 628, 445);
            ((SEImage)skinElement).NameAttribute = "Name";
            ((SEImage)skinElement).ResourceType = ResourceTypes.Image;
            skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(), skinsProject.BorderWidth);
            skin.Elements.Add(skinElement);

            // Zone équipe
            skinElement = new SECurvedRectangle(skin, 40, 565, 664, 80, 18);
            skinElement.SetBackground("Pierre 01");
            skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(), skinsProject.BorderWidth);
            skin.Elements.Add(skinElement);

            skinElement = new SETextArea(skin, 58, 565, 628, 80, "<Equipe>");
            ((SETextArea)skinElement).TextFont = new Font("Bell MT", 10, FontStyle.Bold);
            ((SETextArea)skinElement).TextAttribute = skinsProject.TeamStringFormat;
            ((SETextArea)skinElement).TextVerticalAlign = VerticalAlignment.Center;
            ((SETextArea)skinElement).TextFont = new Font("Bell MT", 8, FontStyle.Bold);
            skin.Elements.Add(skinElement);

            //Zone rareté
            skinElement = new SECurvedRectangle(skin, 655, 590, 30, 30, 5);
            skinElement.SetBackground(GetMatchingRarityColor());
            skinElement.Border = new SkinElementBorder(Color.Black, 1);
            skin.Elements.Add(skinElement);

            // Zone de pouvoirs
            skinElement = new SERectangle(skin, 58, 645, 628, 285);
            skinElement.SetBackground("Pierre 01");
            skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(), skinsProject.BorderWidth);
            skin.Elements.Add(skinElement);

            skinElement = new SETextArea(skin, 70, 655, 604, 265, "<pouvoirs>");
            ((SETextArea)skinElement).TextAttribute = "Powers";
            ((SETextArea)skinElement).TextFont = new Font("Bell MT", 8, FontStyle.Bold);
            ((SETextArea)skinElement).TextAlign = HorizontalAlignment.LeftWithIcon;
            ((SETextArea)skinElement).WordSpaceOffsetX = 0;
            ((SETextArea)skinElement).RowSpaceOffsetY = 0;
            skin.Elements.Add(skinElement);

            // Zone Citation
            skinElement = new SETextArea(skin, 58, 880, 628, 45, "<Citation>");
            ((SETextArea)skinElement).TextAttribute = "Citation";
            ((SETextArea)skinElement).TextVerticalAlign = VerticalAlignment.Center;
            ((SETextArea)skinElement).TextAlign = HorizontalAlignment.Center;
            ((SETextArea)skinElement).TextFont = new Font("Informal Roman", 8, FontStyle.Bold | FontStyle.Italic);
            skin.Elements.Add(skinElement);

            // Zone de Attack si non vide
            skinElement = new SEImage(skin, 32, 880, 100, 100, "star2");
            skinElement.Visible = IsAttributeNonEmpty("Attack");
            skin.Elements.Add(skinElement);

            skinElement = new SETextArea(skin, 52, 900, 60, 60, "?");
            ((SETextArea)skinElement).TextFont = new Font("Bell MT", 10, FontStyle.Bold);
            ((SETextArea)skinElement).TextAttribute = "Attack";
            skinElement.Visible = IsAttributeNonEmpty("Attack");
            ((SETextArea)skinElement).TextAlign = HorizontalAlignment.Center;
            ((SETextArea)skinElement).TextVerticalAlign = VerticalAlignment.Center;
            skin.Elements.Add(skinElement);

            // Zone de PV is non vide
            skinElement = new SEImage(skin, 612, 880, 100, 100, "shield");
            skinElement.Visible = IsAttributeNonEmpty("Defense");
            skin.Elements.Add(skinElement);

            skinElement = new SETextArea(skin, 632, 900, 60, 60, "?");
            ((SETextArea)skinElement).TextFont = new Font("Bell MT", 10, FontStyle.Bold);
            ((SETextArea)skinElement).TextAttribute = "Defense";
            skinElement.Visible = IsAttributeNonEmpty("Defense");
            ((SETextArea)skinElement).FontColor = ConvertColorFromString("black");
            ((SETextArea)skinElement).TextAlign = HorizontalAlignment.Center;
            ((SETextArea)skinElement).TextVerticalAlign = VerticalAlignment.Center;
            skin.Elements.Add(skinElement);

            return skin;
        }
    }
}
