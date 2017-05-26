using System.IO;

namespace CardMasterCmdExport
{
    class Parameters
    {
        public FileInfo JsonProjectFile { get; set; } = null;

        public DirectoryInfo ExportTargetFolder { get; set; } = null;


        // Paramètres
        // Ils doivent être initialisés à Null et doivent donc être Nullable.

        public string ExportMode { get; set; } = null;

        public string ExportFormat { get; set; } = null;

        public int? BoardSpace { get; set; } = null;

        public bool? WithBackSide { get; set; } = null;
    }
}
