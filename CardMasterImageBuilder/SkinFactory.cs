using CardMasterCard.Card;
using CardMasterSkin.Skins;
using CardMasterSkin.Elements;
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace CardMasterImageBuilder
{
    public class SkinFactory
    {
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
                skinElement.SetBackground("eau");
                skin.Elements.Add(skinElement);

                // Zone entête
                skinElement = new SECurvedRectangle(skin, 22, 22, w - 44, 40, 8);
                skinElement.SetBackground("Pierre 01");
                skinElement.Border = new SkinElementBorder(Color.DarkGreen, 3);
                skin.Elements.Add(skinElement);

                skinElement = new SETextArea(skin, 22, 22, w - 44, 40, "<Nom>");
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
                skinElement.Border = new SkinElementBorder(Color.DarkGreen, 3);
                skin.Elements.Add(skinElement);

                // Zone équipe
                skinElement = new SECurvedRectangle(skin, 22, 282, w - 44, 40, 8);
                skinElement.SetBackground("Pierre 01");
                skinElement.Border = new SkinElementBorder(Color.DarkGreen, 3);
                skin.Elements.Add(skinElement);

                skinElement = new SETextArea(skin, 22, 282, w - 44, 40, "<Equipe>");
                ((SETextArea)skinElement).TextAttribute = "@Team@ :  @Chakra@ %(@Element@)%";
                ((SETextArea)skinElement).TextVerticalAlign = VerticalAlignment.Center;
                skin.Elements.Add(skinElement);

                // Zone de pouvoirs
                skinElement = new SERectangle(skin, 30, 322, w - 60, 150);
                skinElement.SetBackground("Pierre 01");
                skinElement.Border = new SkinElementBorder(Color.DarkGreen, 3);
                skin.Elements.Add(skinElement);

                skinElement = new SETextArea(skin, 35, 327, w - 70, 140, "<Epouvoirs>");
                ((SETextArea)skinElement).TextAttribute = "Powers";
                skin.Elements.Add(skinElement);

                // Zone Citation
                skinElement = new SETextArea(skin, 35, 380, w - 100, 140, "<Citation>");
                ((SETextArea)skinElement).TextAttribute = "Citation";
                ((SETextArea)skinElement).TextVerticalAlign = VerticalAlignment.Center;
                ((SETextArea)skinElement).TextFont = new Font("Arial", 10, FontStyle.Italic); 
                skin.Elements.Add(skinElement);
            }
            return skin;

        }

    }
}
