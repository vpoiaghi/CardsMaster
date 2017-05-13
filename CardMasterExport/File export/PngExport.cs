using CardMasterCard.Card;
using CardMasterCommon.Dialog;
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

        public PngExport(Window owner, List<Card> cardsList, FileInfo skinsFile) : this(owner, cardsList, skinsFile, null)
        { }

        public PngExport(List<Card> cardsList, FileInfo skinsFile, DirectoryInfo targetFolder) : base(cardsList, skinsFile)
        {
            this.targetFolder = targetFolder;
        }

        public PngExport(Window owner, List<Card> cardsList, FileInfo skinsFile, DirectoryInfo targetFolder) : base(owner, cardsList, skinsFile)
        {
            this.targetFolder = targetFolder;
        }   

        protected override bool BeforeCardsExport()
        {
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

            List<Card> cards = null;
            string targetFolder = null;

            lock (_lock){
                cards = this.cardsList;
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
