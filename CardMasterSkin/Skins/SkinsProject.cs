using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CardMasterSkin.Skins
{
    public class SkinsProject
    {
        public string TexturesDirectory { get; set; }
        public string ImagesDirectory { get; set; }
        public Dictionary<String, String> MapLibelleColor { get; set; }
        public Dictionary<String, String> MapKindField { get; set; }
        public string TeamStringFormat { get; set; }

        public static SkinsProject LoadProject(FileInfo file)
        {
            SkinsProject skinsProject = null;

            if ((file != null) && (file.Exists))
            {
                var sr = new StreamReader(file.FullName);
                string js = sr.ReadToEnd();

                skinsProject = JsonConvert.DeserializeObject<SkinsProject>(js);

                sr.Close();
                sr.Dispose();

                skinsProject.TexturesDirectory = skinsProject.TexturesDirectory.Replace("\\\\", "\\");
                skinsProject.ImagesDirectory = skinsProject.ImagesDirectory.Replace("\\\\", "\\");

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
