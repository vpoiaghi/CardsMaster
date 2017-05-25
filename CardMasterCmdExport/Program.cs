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
            Console.WriteLine("Projet                   " + prms.JsonProjectFile.FullName);
            Console.WriteLine("Dossier cible            " + prms.ExportTargetFolder);
            Console.WriteLine("Mode d'exportation       " + prms.ExportMode);
            Console.WriteLine("Espace entre les cartes  " + prms.BoardSpace);
            Console.WriteLine("Format d'export          " + prms.ExportFormat);
            Console.WriteLine("");

            CardsProject project = CardsProject.LoadProject(prms.JsonProjectFile);

            string skinsFileName = prms.JsonProjectFile.Name.Replace(prms.JsonProjectFile.Extension, ".skin");
            FileInfo skinFile = new FileInfo(Path.Combine(prms.JsonProjectFile.Directory.FullName, skinsFileName));

            switch (prms.ExportMode)
            {
                case ExportModes.all:
                    ExportAll(project, skinFile, prms);
                    break;

                case ExportModes.board:
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
            parameters.SpaceBetweenCards = prms.BoardSpace;
            exporter.Export(parameters);
        }

    }
}
