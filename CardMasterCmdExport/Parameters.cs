using System;
using System.IO;

namespace CardMasterCmdExport
{
    public enum ExportFormats
    {
        PNG, PDF
    };

    class Parameters
    {
        public FileInfo JsonProjectFile { get; set; } = null;
        public DirectoryInfo ExportTargetFolder { get; set; } = null;
        public bool ExportAll { get; set; } = false;
        public ExportFormats ExportFormat { get; set; } = ExportFormats.PNG;

        public Parameters(string[] args)
        {

            if (args == null)
            {
                throw new ArgumentException("Nombre de paramètres invalide.");
            }
            else if ((args.Length == 1) && (args[0].Equals("/?")))
            {
                throw new ArgumentException("");
            }
            else if  (args.Length < 2)
            {
                throw new ArgumentException("Nombre de paramètres invalide.");
            }
            else
            {
                this.JsonProjectFile = new FileInfo(args[0]);
                if (!this.JsonProjectFile.Exists)
                {
                    throw new ArgumentException("Le projet json n'existe pas ou n'a pu être trouvé.");
                }

                this.ExportTargetFolder = new DirectoryInfo(args[1]);
                if (!this.ExportTargetFolder.Exists)
                {
                    throw new ArgumentException("Le dossier " + args[1] + " cible n'existe pas ou n'a pu être trouvé.");
                }

                int i = 1;
                while (++i < args.Length)
                {
                    if (args[i].Equals("/all"))
                    {
                        this.ExportAll = true;
                    }
                    else if (args[i].StartsWith("/f:"))
                    {
                        this.ExportFormat = StringArgToExportFormats(args[i]);
                    }
                    else if (args[i].Equals("/?"))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw new ArgumentException("Paramètre " + args[i] + " inconnu.");
                    }
                }
            }
        }

        private ExportFormats StringArgToExportFormats(string value)
        {
            string f = value.Substring(3).Trim().ToLower();

            switch (f)
            {
                case "png": return ExportFormats.PNG;
                case "pdf": return ExportFormats.PDF;
            }

            throw new ArgumentException("Format " + f + " inconnu.");
        }
    }

}
