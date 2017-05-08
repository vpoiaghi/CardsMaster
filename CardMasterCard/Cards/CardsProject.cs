using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml.Serialization;

namespace CardMasterCard.Card
{
    public class CardsProject
    {
        public CardsSet Cards { get; set; }

        public static CardsProject LoadProject(FileInfo file)
        {
            CardsProject cardsProject = null;

            switch(file.Extension.ToLower())
            {
                case ".xml" :
                    cardsProject = LoadXmlProject(file);
                    break;
                case ".json" :
                    cardsProject = LoadJsonProject(file);
                    break;
            }

            return cardsProject;

        }

        private static CardsProject LoadXmlProject(FileInfo file)
        {
            CardsProject cardsProject = null;

            Stream myFileStream = file.OpenRead();
            XmlSerializer xs = new XmlSerializer(cardsProject.GetType());

            cardsProject = (CardsProject)xs.Deserialize(myFileStream);

            myFileStream.Close();
            myFileStream.Dispose();
            myFileStream = null;

            return cardsProject; 
        }

        private static CardsProject LoadJsonProject(FileInfo file)
        {
            StreamReader sr = new StreamReader(file.FullName);
            String js = sr.ReadToEnd();

            CardsProject cardsProject = (CardsProject)JsonConvert.DeserializeObject<CardsProject>(js);

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
