using CardMasterCard.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMasterSkinEditor
{
    public class CardBuilder
    {
        public static JsonCard BuildTemplate()
        {
            JsonCard cardTemplate = new JsonCard();
            cardTemplate.Attack = "4";
            cardTemplate.Cost = "6";
            cardTemplate.Defense = "3";
            cardTemplate.Citation = "texte citation";
            cardTemplate.Chakra = "Spécial";
            cardTemplate.Background = null;
            cardTemplate.Rank = "Rank";
            cardTemplate.Element = "Eau";
            cardTemplate.Kind = "Ninja";
            cardTemplate.Name = "Card Name";
            cardTemplate.Powers = new List<JsonPower>();
            cardTemplate.BackSkinName = "BackSkin1";
            cardTemplate.FrontSkinName = "FrontSkin1";
            cardTemplate.BackSide = "Back-Draw";
            cardTemplate.Team = "Equipe";
            cardTemplate.StringField1 = "<<Konoha>>Naruto Shippuden";
            JsonPower p1 = new JsonPower(); p1.Description = "<<Permanent>>  Ligne courte pouvoir 1";
            JsonPower p2 = new JsonPower(); p2.Description = "<<activate>>  Ceci est une ligne de pouvoir avec un texte assez long pour occuper plusieurs lignes";
            cardTemplate.Powers.Add(p1);
            cardTemplate.Powers.Add(p2);
            return cardTemplate;
        }
    }
}
