using System;
using System.Collections.Generic;
using System.IO;

namespace CardMasterCmdExport
{
    public enum ExportModes
    {
        all, board
    };

    public enum ExportFormats
    {
        PNG, PDF
    };

    class Parameters
    {
        public FileInfo JsonProjectFile { get; set; } = null;
        public DirectoryInfo ExportTargetFolder { get; set; } = null;
        public ExportModes ExportMode { get; set; } = ExportModes.all;
        public ExportFormats ExportFormat { get; set; } = ExportFormats.PNG;
        public int BoardSpace { get; set; } = 0;

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
                ReadCardProject(args);
                ReadTarget(args);
                ReadParameters(args);
            }
        }

        private void ReadCardProject(string[] args)
        {
            this.JsonProjectFile = new FileInfo(args[0]);
            if (!this.JsonProjectFile.Exists)
            {
                throw new ArgumentException("Le projet json n'existe pas ou n'a pu être trouvé.");
            }
        }

        private void ReadTarget(string[] args)
        {
            this.ExportTargetFolder = new DirectoryInfo(args[1]);
            if (!this.ExportTargetFolder.Exists)
            {
                throw new ArgumentException("Le dossier " + args[1] + " cible n'existe pas ou n'a pu être trouvé.");
            }
        }

        private void ReadParameters(string[] args)
        {
            int i = 1;
            while (++i < args.Length)
            {
                if (args[i].StartsWith("/m:"))
                {
                    this.ExportMode = StringArgToExportModes(args[i]);
                }
                else if (args[i].StartsWith("/f:"))
                {
                    this.ExportFormat = StringArgToExportFormats(args[i]);
                }
                else if (args[i].StartsWith("/s:"))
                {
                    this.BoardSpace = StringArgToInt("s", args[i]);
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

        private ExportModes StringArgToExportModes(string value)
        {
            string m = value.Substring(3).Trim().ToLower();

            switch (m)
            {
                case "all": return ExportModes.all;
                case "board": return ExportModes.board;
            }

            throw new ArgumentException("Mode " + m + " inconnu.");
        }

        private int StringArgToInt(string argName, string arg)
        {
            string strValue = arg.Substring(argName.Length + 2).Trim().ToLower();
            int intValue = 0;

            try
            {
                intValue = int.Parse(strValue);
            }
            catch (FormatException ex)
            {
                throw new FormatException("Le paramètre " + argName + " " + " n'est pas un entier valide.", ex);
            }

            return intValue;
        }
    }
}
