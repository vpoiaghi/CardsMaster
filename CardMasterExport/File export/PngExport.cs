using CardMasterCard.Card;
using CardMasterCommon.Dialog;
using CardMasterExport.Export;
using CardMasterImageBuilder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;

namespace CardMasterExport.FileExport

{
    public class PngExport : Exporter, IDisposable
    {
        private DirectoryInfo targetFolder = null;

        public PngExport(List<Card> cardsList, FileInfo skinsFile) : base(cardsList, skinsFile)
        { }

        public PngExport(Window owner, List<Card> cardsList, FileInfo skinsFile) : base(owner, cardsList, skinsFile)
        { }

        public override ExportParameters GetParameters()
        {
            return new Parameters();
        }

        protected override bool BeforeCardsExport()
        {
            this.targetFolder = ((Parameters)this.parameters).TargetFolder;

            if (this.targetFolder == null)
            {
                this.targetFolder = FolderDialog.SelectFolder();
            }

            return (this.targetFolder != null);
        }

        protected override void MakeCardExport(Card card)
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

        public class Parameters : ExportParameters
        {
            public DirectoryInfo TargetFolder { get; set; } = null;
        }

    }

}
