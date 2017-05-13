using CardMasterCard.Card;
using CardMasterExport.FileExport;
using System;
using System.IO;

namespace CardMasterCmdExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var prms = new Parameters(args); ;
                Export(prms);
            }
            catch (ArgumentException ex)
            {
                Usage.ShowErrUsage(ex.Message);
            }

        }

        private static void Export(Parameters prms)
        {
            Console.WriteLine("Projet            " + prms.JsonProjectFile.FullName);
            Console.WriteLine("Dossier cible     " + prms.ExportTargetFolder);
            Console.WriteLine("Tout exporter     " + (prms.ExportAll ? "oui" : "non"));
            Console.WriteLine("Format d'export   " + prms.ExportFormat);
            Console.WriteLine("");

            CardsProject project = CardsProject.LoadProject(prms.JsonProjectFile);

            string skinsFileName = prms.JsonProjectFile.Name.Replace(prms.JsonProjectFile.Extension, ".skin");
            FileInfo skinFile = new FileInfo(Path.Combine(prms.JsonProjectFile.Directory.FullName, skinsFileName));

            var exporter = new PngExport(project.Cards, skinFile, prms.ExportTargetFolder);
            exporter.Export();

            //Console.ReadKey(true);
        }

    }
}
