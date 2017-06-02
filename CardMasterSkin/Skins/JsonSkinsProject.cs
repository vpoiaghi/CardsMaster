using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CardMasterSkin.Skins
{
    public class JsonSkinsProject
    {
        public Dictionary<String, String> MapLibelleColor { get; set; }
        public Dictionary<String, String> MapKindField { get; set; }
        public Dictionary<String, String> MapLibelleBorderColor { get; set; }
        public Dictionary<String, String> MapRareteColor { get; set; }
        public int BorderWidth { get; set; }
        public List<JsonSkin> Skins { get; set; }

        public static JsonSkinsProject LoadProject(FileInfo file)
        {
            JsonSkinsProject skinsProject = null;

            if ((file != null) && (file.Exists))
            {
                var sr = new StreamReader(file.FullName);
                string js = sr.ReadToEnd();

                skinsProject = JsonConvert.DeserializeObject<JsonSkinsProject>(js);

                sr.Close();
                sr.Dispose();
            }

            return skinsProject;

        }

        public void Save(FileInfo file)
        {
            file.Delete();

            string js = JsonConvert.SerializeObject(this, Formatting.Indented);
            var sw = new StreamWriter(file.FullName, false);

            sw.Write(js);

            sw.Close();
            sw.Dispose();

        }

    }
}
