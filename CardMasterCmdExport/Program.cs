using CardMasterCard.Card;
using CardMasterExport;
using CardMasterExport.Export;
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
                ParametersReader reader = new ParametersReader(); ;
                Parameters prms = reader.Read(args);
                Export(prms);
            }
            catch (ArgumentException ex)
            {
                Usage.ShowErrUsage(ex.Message);
            }

        }

        private static void Export(Parameters prms)
        {
            Console.WriteLine(prms.ToString());
            Console.WriteLine("");

            JsonCardsProject project = JsonCardsProject.LoadProject(prms.JsonProjectFile);

            string skinsFileName = prms.JsonProjectFile.Name.Replace(prms.JsonProjectFile.Extension, ".skin");
            FileInfo skinFile = new FileInfo(Path.Combine(prms.JsonProjectFile.Directory.FullName, skinsFileName));

            switch (prms.ExportMode)
            {
                case "all":
                    ExportAll(project, skinFile, prms);
                    break;

                case "board":
                    ExportBoards(project, skinFile, prms);
                    break;
            }

            //Console.ReadKey(true);
        }

        private static void ExportAll(JsonCardsProject project, FileInfo skinFile, Parameters prms)
        {
            ExportParameters parameters = new ExportParameters(project.Cards, skinFile);
            parameters.exportFormat = Exporter.EXPORT_FORMAT_PNG;
            parameters.exportMode = Exporter.EXPORT_MODE_ALL;
            parameters.TargetFolder = prms.ExportTargetFolder;

            Exporter.Export(parameters);
        }

        private static void ExportBoards(JsonCardsProject project, FileInfo skinFile, Parameters prms)
        {
            ExportParameters parameters = new ExportParameters(project.Cards, skinFile);
            parameters.exportFormat = Exporter.EXPORT_FORMAT_PNG;
            parameters.exportMode = Exporter.EXPORT_MODE_BOARD;
            parameters.TargetFolder = prms.ExportTargetFolder;
            parameters.SpaceBetweenCards = prms.BoardSpace.Value;
            parameters.WithBackSides = prms.WithBackSide.Value;

            Exporter.Export(parameters);
        }

    }
}
