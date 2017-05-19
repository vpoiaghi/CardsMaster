using CardMasterCard.Card;
using CardMasterSkin.Skins;
using CardMasterSkin.Elements;
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CardMasterImageBuilder
{
    public class SkinFactory
    {
        static private ColorConverter colorConverter = new ColorConverter();
        public static Skin GetSkin(Card card, FileInfo skinsFile, String skinName)
        {
            Skin skin = null;

            int w = 375;
            int h = 523;
            int borderSize = 15;
          
            SkinsProject skinsProject = SkinsProject.LoadProject(skinsFile);

            if (skinsProject != null)
            {

                DirectoryInfo texturesDirectory = new DirectoryInfo(skinsProject.TexturesDirectory);
                DirectoryInfo imagesDirectory = new DirectoryInfo(skinsProject.ImagesDirectory);

                skin = new Skin(w, h, imagesDirectory, texturesDirectory);
                SkinElement skinElement;

                // Bordures
                skinElement = new SERoundedRectangle(skin, w, h, borderSize);
                skinElement.SetBackground(Color.Black);
                skin.Elements.Add(skinElement);

                // Texture de fond
                skinElement = new SERectangle(skin, borderSize, borderSize, w - borderSize * 2, h - borderSize * 2);
                skinElement.SetBackground(GetMatchingBackground(skinsProject,card));
                skin.Elements.Add(skinElement);

                // Zone entête
                skinElement = new SECurvedRectangle(skin, 22, 22, w - 44, 40, 8);
                skinElement.SetBackground("Pierre 01");
                skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(skinsProject,card), skinsProject.BorderWidth);
                skin.Elements.Add(skinElement);

                skinElement = new SETextArea(skin, 30, 22, w - 44, 40, "<Nom>");
                ((SETextArea)skinElement).TextAttribute = "Name";
                ((SETextArea)skinElement).TextVerticalAlign = VerticalAlignment.Center;
                //((SETextArea)skinElement).TextFont = new Font("Arial", 26, FontStyle.Italic); // --> Exemple de définition de la police de caractère. Par défaut c'est new Font("Bell MT", 14, FontStyle.Bold);
                skin.Elements.Add(skinElement);

                // Zone de coût
                skinElement = new SEImage(skin, w - 65, 27, 30, 30, "mana_circle");
                skin.Elements.Add(skinElement);

                skinElement = new SETextArea(skin, w - 65, 27, 30, 30, "?");
                ((SETextArea)skinElement).TextAttribute = "Cost";
                ((SETextArea)skinElement).TextAlign = HorizontalAlignment.Center;
                ((SETextArea)skinElement).TextVerticalAlign = VerticalAlignment.Center;
                skin.Elements.Add(skinElement);

                // Image
                skinElement = new SEImage(skin, 30, 62, w - 60, 220);
                ((SEImage)skinElement).NameAttribute = "Name";
                ((SEImage)skinElement).ResourceType = ResourceTypes.Image;
                skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(skinsProject,card), skinsProject.BorderWidth);
                skin.Elements.Add(skinElement);

                // Zone équipe
                skinElement = new SECurvedRectangle(skin, 22, 282, w - 44, 40, 8);
                skinElement.SetBackground("Pierre 01");
                skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(skinsProject, card), skinsProject.BorderWidth);
                skin.Elements.Add(skinElement);

                skinElement = new SETextArea(skin, 30, 282, w - 44, 40, "<Equipe>");
                ((SETextArea)skinElement).TextAttribute = skinsProject.TeamStringFormat;
                ((SETextArea)skinElement).TextVerticalAlign = VerticalAlignment.Center;
                skin.Elements.Add(skinElement);

                // Zone de pouvoirs
                skinElement = new SERectangle(skin, 30, 322, w - 60, 150);
                skinElement.SetBackground("Pierre 01");
                skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(skinsProject, card), skinsProject.BorderWidth);
                skin.Elements.Add(skinElement);

                skinElement = new SETextArea(skin, 35, 330, w - 70, 140, "<Epouvoirs>");
                ((SETextArea)skinElement).TextAttribute = "Powers";
                ((SETextArea)skinElement).TextFont = new Font("Bell MT", 12, FontStyle.Bold);
                ((SETextArea)skinElement).TextAlign = HorizontalAlignment.LeftWithIcon;
                skin.Elements.Add(skinElement);

                // Zone Citation
                skinElement = new SETextArea(skin, 50, 385, w - 100, 140, "<Citation>");
                ((SETextArea)skinElement).TextAttribute = "Citation";
                ((SETextArea)skinElement).TextVerticalAlign = VerticalAlignment.Center;
                ((SETextArea)skinElement).TextAlign = HorizontalAlignment.Center;
                ((SETextArea)skinElement).TextFont = new Font("Informal Roman", 12, FontStyle.Bold | FontStyle.Italic);
                skin.Elements.Add(skinElement);
            }
            return skin;

        }

        private static String GetMatchingBackground(SkinsProject skinsProject,Card card)
        {   
            String attributeName = skinsProject.MapKindField[card.Kind];
            String attributeValue = (String)card.GetType().GetProperty(attributeName).GetValue(card, null);
            return skinsProject.MapLibelleColor[attributeValue];
        }
        private static Color GetMatchingBorderColor(SkinsProject skinsProject, Card card)
        {
            String attributeName = skinsProject.MapKindField[card.Kind];
            String attributeValue = (String)card.GetType().GetProperty(attributeName).GetValue(card, null);
            String rgbCode = skinsProject.MapLibelleBorderColor[attributeValue];
            return (Color)colorConverter.ConvertFromString(rgbCode);
        }


    }
}
