using CardMasterCard.Card;
using CardMasterExport.Export;
using CardMasterImageBuilder;
using CardMasterSkin.Skins;
using System;
using System.Drawing;
using System.IO;

namespace CardMasterExport.FileExport

{
    public class PngExport : Exporter, IDisposable
    {
        private DirectoryInfo targetFolder = null;

        protected override bool BeforeCardsExport(ExportParameters parameters)
        {
            this.targetFolder = parameters.TargetFolder;

            return (this.targetFolder != null);
        }

        protected override void MakeCardExport(JsonCard card, JsonSkin skin)
        {
            Drawer drawer = null;
            Image img = null;

            string targetFolder = null;

            lock (_lock){
                targetFolder = this.targetFolder.FullName;
            }

            string fileName = card.Name
                .Replace("\\", " ")
                .Replace("/", " ")
                .Replace(":", "-")
                .Replace("*", " ")
                .Replace("?", " ")
                .Replace("\"", "'")
                .Replace("<", " ")
                .Replace(">", " ")
                .Replace("|", " ");

            drawer = new Drawer(card, skinsFile, null);
            img = drawer.DrawCard();

            img.Save(Path.Combine(targetFolder, fileName + ".png"));
            img.Dispose();

            drawer = null;
            img = null;

        }

        protected override void AfterCardsExport()
        { }

        protected override string GetProgressMessage(ProgressState state, int index, int total)
        {
            string message = "";

            if (state == ProgressState.ExportInProgress)
            {
                message = "Export en cours : " + index + " / " + total;
            }
            else if (state == ProgressState.ExportEnded)
            {
                message = "Export terminé";
            }

            return message;
        }

    }
}
