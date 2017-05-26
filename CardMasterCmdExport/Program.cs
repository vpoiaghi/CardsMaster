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

            CardsProject project = CardsProject.LoadProject(prms.JsonProjectFile);

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

        private static void ExportAll(CardsProject project, FileInfo skinFile, Parameters prms)
        {
            PngExport exporter = new PngExport(project.Cards, skinFile);
            PngExport.Parameters parameters = (PngExport.Parameters)exporter.GetParameters();
            parameters.TargetFolder = prms.ExportTargetFolder;
            exporter.Export(parameters);
        }

        private static void ExportBoards(CardsProject project, FileInfo skinFile, Parameters prms)
        {
            PngBoardExport exporter = new PngBoardExport(project.Cards, skinFile);
            PngBoardExport.Parameters parameters = (PngBoardExport.Parameters)exporter.GetParameters();
            parameters.TargetFolder = prms.ExportTargetFolder;
            parameters.SpaceBetweenCards = prms.BoardSpace.Value;
            parameters.WithBackSides = prms.WithBackSide.Value;
            exporter.Export(parameters);
        }

    }
}
