using CardMasterCard.Card;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMasterManager.Models.Card
{
    public class CardTemplate
    {
        public JsonCard template { get; set; }

        public static CardMasterManager.Card BuildTemplateCard()
        {
            String dir = System.AppDomain.CurrentDomain.BaseDirectory;
            if (File.Exists(dir + "/template.json"))
            {
                StreamReader sr = new StreamReader(dir + "/template.json");
                String js = sr.ReadToEnd();

                CardTemplate jsonCardTemplate = (CardTemplate)JsonConvert.DeserializeObject<CardTemplate>(js);

                sr.Close();
                sr.Dispose();
                sr = null;
                return CardMasterManager.Card.ConvertCard(jsonCardTemplate.template);
            }
            return null;
        }
    }
}
