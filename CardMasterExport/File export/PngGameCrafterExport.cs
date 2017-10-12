using CardMasterCard.Card;
using CardMasterExport.Export;
using CardMasterImageBuilder;
using CardMasterSkin.Skins;
using System;
using System.Drawing;
using System.IO;

namespace CardMasterExport.FileExport

{
    public class PngGameCrafterExport : Exporter, IDisposable
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

         
            string targetFolder = null;

            lock (_lock){
                targetFolder = this.targetFolder.FullName + "\\" + card.BackSide;
            }

            drawer = new Drawer(card, skinsFile, null);
            Directory.CreateDirectory(targetFolder);
            for (int i = 1; i <= card.Nb; i++)
            {
                Image imgFront = null;
                imgFront = drawer.DrawCard();
                imgFront.Save(Path.Combine(targetFolder, fileName + "-" + i + ".png"));
                imgFront.Dispose();
                imgFront = null;
            }
            if (!File.Exists(Path.Combine(targetFolder, "0-Back" + ".png")))
            {
                Image imgBack = null;
                imgBack = drawer.DrawBackCard();
           
                imgBack.Save(Path.Combine(targetFolder, "0-Back" + ".png"));
           
                imgBack.Dispose();
                imgBack = null;
            }
            drawer = null;
            

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
