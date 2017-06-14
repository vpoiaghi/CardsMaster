using CardMasterCard.Card;
using CardMasterExport;
using CardMasterExport.Export;
using System;
using System.IO;

namespace CardMasterCmdExport
{
    public class Export : IExporterOwner
    {
        public Export()
        { }

        public void StartExport(string[] arguments)
        {
            try
            {
                ParametersReader reader = new ParametersReader();
                Parameters prms = reader.Read(arguments);
                StartExport(prms);
            }
            catch (ArgumentException ex)
            {
                Usage.ShowErrUsage(ex.Message);
            }
        }

        private void StartExport(Parameters prms)
        {
            Console.WriteLine(prms.ToString());
            Console.WriteLine("");

            JsonCardsProject project = JsonCardsProject.LoadProject(prms.JsonProjectFile);

            string skinsFileName = prms.JsonProjectFile.Name.Replace(prms.JsonProjectFile.Extension, ".skin");
            FileInfo skinFile = new FileInfo(Path.Combine(prms.JsonProjectFile.Directory.FullName, skinsFileName));

            ExportParameters parameters = new ExportParameters(project.Cards, skinFile);
            parameters.exportFormat = prms.ExportFormat;
            parameters.exportMode = prms.ExportMode;
            parameters.TargetFolder = prms.ExportTargetFolder;

            Exporter.Export(this, parameters);
        }

        void IExporterOwner.ExportProgressChanged(object sender, ProgressChangedArg args)
        {
            ClearCurrentConsoleLine();

            if (args.State == ProgressState.ExportInProgress)
            {
                Console.Write(args.Message);
            }
            else
            {
                Console.WriteLine(args.Message);
            }
        }

        private void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}
