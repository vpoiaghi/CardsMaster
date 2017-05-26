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
            //return GetSkin96(card, skinsFile, skinName);
            return GetSkin300(card, skinsFile, skinName);
        }

        private static Skin GetSkin96(Card card, FileInfo skinsFile, String skinName)
        {
            Skin skin = null;

            // 96 ppp --> 9.92 x 13.84 cm
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
                skinElement = new SERoundedRectangle(skin, 0, 0, w, h, borderSize);
                skinElement.SetBackground(card.Nb == 1 ? Color.Black : Color.Gold);
                skin.Elements.Add(skinElement);

                // Texture de fond
                skinElement = new SERectangle(skin, borderSize, borderSize, w - borderSize * 2, h - borderSize * 2);
                skinElement.SetBackground(GetMatchingBackground(skinsProject, card));
                skin.Elements.Add(skinElement);

                // Zone entête
                skinElement = new SECurvedRectangle(skin, 22, 22, w - 44, 40, 8);
                skinElement.SetBackground("Pierre 01");
                skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(skinsProject, card), skinsProject.BorderWidth);
                skin.Elements.Add(skinElement);

                skinElement = new SETextArea(skin, 28, 22, w - 44, 40, "<Nom>");
                ((SETextArea)skinElement).TextAttribute = "Name";
                ((SETextArea)skinElement).TextVerticalAlign = VerticalAlignment.Center;
                skin.Elements.Add(skinElement);

                // Zone de coût
                skinElement = new SEImage(skin, w - 65, 27, 30, 30, "mana_circle");
                skin.Elements.Add(skinElement);

                skinElement = new SETextArea(skin, w - 64, 27, 30, 30, "?");
                ((SETextArea)skinElement).TextAttribute = "Cost";
                ((SETextArea)skinElement).TextAlign = HorizontalAlignment.Center;
                ((SETextArea)skinElement).TextVerticalAlign = VerticalAlignment.Center;
                skin.Elements.Add(skinElement);

                // Image
                skinElement = new SEImage(skin, 30, 62, w - 60, 220);
                ((SEImage)skinElement).NameAttribute = "Name";
                ((SEImage)skinElement).ResourceType = ResourceTypes.Image;
                skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(skinsProject, card), skinsProject.BorderWidth);
                skin.Elements.Add(skinElement);

                // Zone équipe
                skinElement = new SECurvedRectangle(skin, 22, 282, w - 44, 40, 8);
                skinElement.SetBackground("Pierre 01");
                skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(skinsProject, card), skinsProject.BorderWidth);
                skin.Elements.Add(skinElement);

                skinElement = new SETextArea(skin, 28, 282, w - 44, 40, "<Equipe>");
                ((SETextArea)skinElement).TextAttribute = skinsProject.TeamStringFormat;
                ((SETextArea)skinElement).TextVerticalAlign = VerticalAlignment.Center;
                ((SETextArea)skinElement).TextFont = new Font("Bell MT", 12, FontStyle.Bold);
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

                // Zone de PV is non vide
                skinElement = new SEImage(skin, w - 68, 440, 50, 50, "shield");
                skinElement.Visible = IsAttributeNonEmpty(card, "Defense");
                skin.Elements.Add(skinElement);
                
                skinElement = new SETextArea(skin, w - 58, 450, 30, 30, "?");
                ((SETextArea)skinElement).TextAttribute = "Defense";
                skinElement.Visible = IsAttributeNonEmpty(card, "Defense");
                ((SETextArea)skinElement).TextAlign = HorizontalAlignment.Center;
                ((SETextArea)skinElement).TextVerticalAlign = VerticalAlignment.Center;
                skin.Elements.Add(skinElement);

                // Zone de Attack si non vide
                skinElement = new SEImage(skin, 14, 438, 60, 60, "star2");
                skinElement.Visible = IsAttributeNonEmpty(card, "Attack");
                skin.Elements.Add(skinElement);

                skinElement = new SETextArea(skin, 31, 454, 30, 30, "?");
                ((SETextArea)skinElement).TextAttribute = "Attack";
                skinElement.Visible = IsAttributeNonEmpty(card, "Attack");
                ((SETextArea)skinElement).TextAlign = HorizontalAlignment.Center;
                ((SETextArea)skinElement).TextVerticalAlign = VerticalAlignment.Center;
                skin.Elements.Add(skinElement);

                //Zone rareté
                skinElement = new SECurvedRectangle(skin, 325, 293, 15, 15, 1);
                skinElement.SetBackground(GetMatchingRarityColor(skinsProject, card));
                skinElement.Border = new SkinElementBorder(Color.Black, 1);
                skin.Elements.Add(skinElement);
            }
            return skin;

        }

        private static Skin GetSkin300(Card card, FileInfo skinsFile, String skinName)
        {
            Skin skin = null;

            // 300 ppp --> 6.3 x 8.79 cm
            int w = 744;
            int h = 1038;

            int borderSize = 28;

            SkinsProject skinsProject = SkinsProject.LoadProject(skinsFile);

            if (skinsProject != null)
            {

                DirectoryInfo texturesDirectory = new DirectoryInfo(skinsProject.TexturesDirectory);
                DirectoryInfo imagesDirectory = new DirectoryInfo(skinsProject.ImagesDirectory);

                skin = new Skin(w, h, imagesDirectory, texturesDirectory);
                SkinElement skinElement;

                // Bordures
                skinElement = new SERoundedRectangle(skin, 0, 0, 744, 1038, borderSize);
                skinElement.SetBackground(card.Nb == 1 ? Color.Black : Color.Gold);
                skin.Elements.Add(skinElement);

                // Texture de fond
                skinElement = new SERectangle(skin, 28, 28, 688, 982);
                skinElement.SetBackground(GetMatchingBackground(skinsProject, card));
                skin.Elements.Add(skinElement);

                // Zone entête
                skinElement = new SECurvedRectangle(skin, 40, 40, 664, 80, 18);
                skinElement.SetBackground("Pierre 01");
                skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(skinsProject, card), skinsProject.BorderWidth);
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
                skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(skinsProject, card), skinsProject.BorderWidth);
                skin.Elements.Add(skinElement);

                // Zone équipe
                skinElement = new SECurvedRectangle(skin, 40, 565, 664, 80, 18);
                skinElement.SetBackground("Pierre 01");
                skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(skinsProject, card), skinsProject.BorderWidth);
                skin.Elements.Add(skinElement);

                skinElement = new SETextArea(skin, 58, 565, 628, 80, "<Equipe>");
                ((SETextArea)skinElement).TextFont = new Font("Bell MT", 10, FontStyle.Bold);
                ((SETextArea)skinElement).TextAttribute = skinsProject.TeamStringFormat;
                ((SETextArea)skinElement).TextVerticalAlign = VerticalAlignment.Center;
                ((SETextArea)skinElement).TextFont = new Font("Bell MT", 8, FontStyle.Bold);
                skin.Elements.Add(skinElement);

                //Zone rareté
                skinElement = new SECurvedRectangle(skin, 655, 590, 30, 30, 5);
                skinElement.SetBackground(GetMatchingRarityColor(skinsProject, card));
                skinElement.Border = new SkinElementBorder(Color.Black, 1);
                skin.Elements.Add(skinElement);

                // Zone de pouvoirs
                skinElement = new SERectangle(skin, 58, 645, 628, 285);
                skinElement.SetBackground("Pierre 01");
                skinElement.Border = new SkinElementBorder(GetMatchingBorderColor(skinsProject, card), skinsProject.BorderWidth);
                skin.Elements.Add(skinElement);

                skinElement = new SETextArea(skin, 70, 655, 604, 265, "<pouvoirs>");
                ((SETextArea)skinElement).TextAttribute = "Powers";
                ((SETextArea)skinElement).TextFont = new Font("Bell MT", 8, FontStyle.Bold);
                ((SETextArea)skinElement).TextAlign = HorizontalAlignment.LeftWithIcon;
                ((SETextArea)skinElement).WordSpaceOffsetX = 0;  // <------------------------------------------------- C'EST LA !!!!!!!!!!!!!!
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
                skinElement.Visible = IsAttributeNonEmpty(card, "Attack");
                skin.Elements.Add(skinElement);

                skinElement = new SETextArea(skin, 52, 900, 60, 60, "?");
                ((SETextArea)skinElement).TextFont = new Font("Bell MT", 10, FontStyle.Bold);
                ((SETextArea)skinElement).TextAttribute = "Attack";
                skinElement.Visible = IsAttributeNonEmpty(card, "Attack");
                ((SETextArea)skinElement).TextAlign = HorizontalAlignment.Center;
                ((SETextArea)skinElement).TextVerticalAlign = VerticalAlignment.Center;
                skin.Elements.Add(skinElement);

                // Zone de PV is non vide
                skinElement = new SEImage(skin, 612, 880, 100, 100, "shield");
                skinElement.Visible = IsAttributeNonEmpty(card, "Defense");
                skin.Elements.Add(skinElement);

                skinElement = new SETextArea(skin, 632, 900, 60, 60, "?");
                ((SETextArea)skinElement).TextFont = new Font("Bell MT", 10, FontStyle.Bold);
                ((SETextArea)skinElement).TextAttribute = "Defense";
                skinElement.Visible = IsAttributeNonEmpty(card, "Defense");
                ((SETextArea)skinElement).TextAlign = HorizontalAlignment.Center;
                ((SETextArea)skinElement).TextVerticalAlign = VerticalAlignment.Center;
                skin.Elements.Add(skinElement);

            }
            return skin;

        }

        private static String GetMatchingBackground(SkinsProject skinsProject, Card card)
        {
            String attributeName = skinsProject.MapKindField[card.Kind];
            String attributeValue = getAttributeValueAsString(card, attributeName);
            return skinsProject.MapLibelleColor[attributeValue];
        }

        private static Boolean IsAttributeNonEmpty(Card card, String attributeName)
        {
            return getAttributeValueAsString(card, attributeName) != "";
        }
        private static String getAttributeValueAsString(Card card, String attributeName)
        {
            return (String)card.GetType().GetProperty(attributeName).GetValue(card, null);
        }

        private static Color GetMatchingRarityColor(SkinsProject skinsProject, Card card)
        {
            return ConvertColorFromString(skinsProject.MapRareteColor[card.Rank]);
        }
        private static Color ConvertColorFromString(String color)
        {
            return (Color)colorConverter.ConvertFromString(color);
        }
        private static Color GetMatchingBorderColor(SkinsProject skinsProject, Card card)
        {
            String attributeName = skinsProject.MapKindField[card.Kind];
            String attributeValue = getAttributeValueAsString(card,attributeName);
            String rgbCode = skinsProject.MapLibelleBorderColor[attributeValue];
            return ConvertColorFromString(rgbCode);
        }


    }
}
