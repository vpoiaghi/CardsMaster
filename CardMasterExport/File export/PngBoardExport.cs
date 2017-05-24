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
    public class PngBoardExport : Exporter, IDisposable
    {
        private const int CARD_WIDTH = 744;
        private const int CARD_HEIGHT = 1038;
        private const int CARD_COUNT_X = 3;
        private const int CARD_COUNT_Y = 3;

        private DirectoryInfo targetFolder = null;
        private int currentX = 0;
        private int currentY = 0;
        private int currentBoard = 0;
        private Bitmap boardImage = null;

        public PngBoardExport(List<Card> cardsList, FileInfo skinsFile) : base(cardsList, skinsFile)
        { }

        public PngBoardExport(Window owner, List<Card> cardsList, FileInfo skinsFile) : base(owner, cardsList, skinsFile)
        { }

        public override ExportParameters GetParameters()
        {
            return new Parameters();
        }

        protected override bool BeforeCardsExport()
        {
            this.currentX = 0;
            this.currentY = 0;
            this.currentBoard = 0;
            this.boardImage = null;

            this.targetFolder = ((Parameters)this.parameters).TargetFolder;

            if (this.targetFolder == null)
            {
                this.targetFolder = FolderDialog.SelectFolder();
            }

            return (this.targetFolder != null);
        }

        protected override void AfterCardsExport()
        {
            if (this.boardImage != null)
            {
                this.currentBoard++;
                SaveImage();
                this.boardImage.Dispose();
                this.boardImage = null;
            }
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

        protected override void MakeCardExport(Card card)
        {
            for (int i = 0; i < card.Nb; i++)
            {
                MakeCardExportOneCopy(card);
            }
        }

        private void MakeCardExportOneCopy(Card card)
        {
            if (this.boardImage == null)
            {
                boardImage = new Bitmap(CARD_WIDTH * CARD_COUNT_X, CARD_HEIGHT * CARD_COUNT_Y);
                boardImage.SetResolution(300, 300);
            }

            DrawCard(card);

            this.currentX++;

            if (this.currentX == CARD_COUNT_X)
            {
                this.currentX = 0;
                this.currentY++;
            }

            if (this.currentY == CARD_COUNT_Y)
            {
                this.currentBoard++;

                SaveImage();
                this.boardImage.Dispose();
                this.boardImage = null;

                this.currentX = 0;
                this.currentY = 0;
            }
        }

        private void DrawCard(Card card)
        {
            Drawer drawer = null;
            Image cardImage = null;
            Image img = null;

            lock (_lock)
            {
                img = this.boardImage;
            }

            drawer = new Drawer(card, skinsFile, null);
            cardImage = drawer.DrawCard();

            Graphics g = Graphics.FromImage(this.boardImage);
            g.DrawImage(cardImage, new PointF(CARD_WIDTH * currentX, CARD_HEIGHT * currentY));
            g.Dispose();

            g = null;
            drawer = null;
            img = null;
        }

        private void SaveImage()
        {
            string targetFolder = null;

            lock (_lock)
            {
                targetFolder = this.targetFolder.FullName;
            }

            string boardName = "Planche " + this.currentBoard.ToString("00000") + ".png";

            this.boardImage.Save(Path.Combine(targetFolder, boardName));
        }

        public class Parameters : ExportParameters
        {
            public DirectoryInfo TargetFolder { get; set; } = null;
        }
    }
}
