using Excel = Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.InteropServices;

namespace CardMasterCard.Card
{
    public class JsonCardsProject
    {
        public List<JsonCard> Cards { get; set; }
        public String SkinFile { get; set; }

        public static JsonCardsProject LoadProject(FileInfo file)
        {
            JsonCardsProject cardsProject = null;

            switch(file.Extension.ToLower())
            {
                case ".xml" :
                    cardsProject = LoadXmlProject(file);
                    break;
                case ".json" :
                    cardsProject = LoadJsonProject(file);
                    break;
                case ".xlsx" :
                    cardsProject = LoadXlsxProject(file);
                    break;
            }

            return cardsProject;

        }

        private static JsonCardsProject LoadXlsxProject(FileInfo file)
        {
#pragma warning disable IDE0017 // Simplifier l'initialisation des objets
            JsonCardsProject cardsProject = new JsonCardsProject();
#pragma warning restore IDE0017 // Simplifier l'initialisation des objets
            cardsProject.Cards = new List<JsonCard>();
            Excel.Application xlApp = new Excel.Application();

            // Open the Excel file.
            // You have pass the full path of the file.
            // In this case file is stored in the Bin/Debug application directory.
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(file.FullName);
            Excel.Worksheet xlWorksheet = (Excel.Worksheet)xlWorkbook.Sheets.get_Item(1);

            // Get the range of cells which has data.
            Excel.Range xlRange = xlWorksheet.UsedRange;
            object[,] valueArray = (object[,])xlRange.get_Value(Excel.XlRangeValueDataType.xlRangeValueDefault);

            // iterate through each cell and display the contents.
            for (int row = 2; row <= xlWorksheet.UsedRange.Rows.Count; ++row)
            {
                if (valueArray[row, 1] == null)
                {
                    break;
                }
                JsonCard card = new JsonCard
                {
                    Name = GetStrValue(valueArray[row, 1]),
                    Kind = GetStrValue(valueArray[row, 2]),
                    Team = GetStrValue(valueArray[row, 3]),
                    Rank = GetStrValue(valueArray[row, 4]),
                    Chakra = GetStrValue(valueArray[row, 5]),
                    Element = GetStrValue(valueArray[row, 6]),
                    Cost = GetStrValue(valueArray[row, 7]),
                    Attack = GetStrValue(valueArray[row, 8]),
                    Defense = GetStrValue(valueArray[row, 9])
                };
                //Power
                String brutPowers = GetStrValue(valueArray[row, 10]);
                if(brutPowers!=null)
                {
                    card.Powers = new List<JsonPower>();
                    String[] lines = brutPowers.Split('\n');
                    foreach (String line in lines)
                    {
#pragma warning disable IDE0017 // Simplifier l'initialisation des objets
                        JsonPower power = new JsonPower();
#pragma warning restore IDE0017 // Simplifier l'initialisation des objets
                        power.Description = line;
                        card.Powers.Add(power);
                    }
                }
                
                card.Citation = GetStrValue(valueArray[row, 11]);
                card.Comments = GetStrValue(valueArray[row, 12]);
                card.Nb = GetIntValue(valueArray[row, 13]);
                card.BackSide = GetStrValue(valueArray[row, 14]);
                card.BackSkinName = GetStrValue(valueArray[row, 15]);
                card.FrontSkinName = GetStrValue(valueArray[row, 16]);
                //Custom
                card.StringField1 = GetStrValue(valueArray[row, 17]);
                card.StringField2 = GetStrValue(valueArray[row, 18]);
                card.StringField3 = GetStrValue(valueArray[row, 19]);
                card.StringField4 = GetStrValue(valueArray[row, 20]);
                card.IntField1 = GetIntValue(valueArray[row, 21]);
                card.IntField2 = GetIntValue(valueArray[row, 22]);
                card.IntField3 = GetIntValue(valueArray[row, 23]);
                card.IntField4 = GetIntValue(valueArray[row, 24]);

                cardsProject.Cards.Add(card);
            }

            // Close the Workbook.
            xlWorkbook.Close(false);

            // Relase COM Object by decrementing the reference count.
            Marshal.ReleaseComObject(xlWorkbook);

            // Close Excel application.
            xlApp.Quit();

            // Release COM object.
            Marshal.FinalReleaseComObject(xlApp);

            return cardsProject;
        }

        private static int? GetIntValue(object v)
        {
           if (v == null)
            {
                return null;
            }else
            {
                return int.Parse(v.ToString());
            }
        }

        private static string GetStrValue(object v)
        {
            return v==null ? "" : v.ToString();
        }

        private static JsonCardsProject LoadXmlProject(FileInfo file)
        {
            JsonCardsProject cardsProject = null;

            Stream myFileStream = file.OpenRead();
            XmlSerializer xs = new XmlSerializer(cardsProject.GetType());

            cardsProject = (JsonCardsProject)xs.Deserialize(myFileStream);

            myFileStream.Close();
            myFileStream.Dispose();
            myFileStream = null;

            return cardsProject; 
        }

        private static JsonCardsProject LoadJsonProject(FileInfo file)
        {
            StreamReader sr = new StreamReader(file.FullName);
            String js = sr.ReadToEnd();

            JsonCardsProject cardsProject = (JsonCardsProject)JsonConvert.DeserializeObject<JsonCardsProject>(js);

            sr.Close();
            sr.Dispose();
            sr = null;

            return cardsProject;
        }

        public void Save(FileInfo file)
        {
            switch (file.Extension.ToLower())
            {
                case ".xml" :
                    SaveXmlProject(file);
                    break;
                case ".json" :
                    SaveJsonProject(file);
                    break;
            }
        }

        private void SaveXmlProject(FileInfo file)
        {
            XmlSerializer xs = new XmlSerializer(this.GetType());
            StreamWriter xwr = new StreamWriter(file.FullName);

            xs.Serialize(xwr, this);

            xwr.Close();
            xwr.Dispose();
            xwr = null;
        }

        private void SaveJsonProject(FileInfo file)
        {
            file.Delete();

            String js = JsonConvert.SerializeObject(this, Formatting.Indented);
            StreamWriter sw = new StreamWriter(file.FullName, false);

            sw.Write(js);

            sw.Close();
            sw.Dispose();
            sw = null;
        }

    }
}
