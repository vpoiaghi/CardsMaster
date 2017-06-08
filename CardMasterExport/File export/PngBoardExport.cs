using CardMasterCard.Card;
using CardMasterCommon.Dialog;
using CardMasterExport.Export;
using CardMasterImageBuilder;
using CardMasterSkin.Skins;
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
        private const int CARD_COUNT_X_WITH_BACK = 1;
        private const int CARD_COUNT_Y = 3;
        private const int CARD_COUNT_Y_WITH_BACK = 3;

        // Paramètres
        private DirectoryInfo targetFolder = null;
        private int spaceBeetweenCards = 0;
        private bool withBackSides = false;

        private int currentX = 0;
        private int currentY = 0;
        private int currentBoard = 0;
        private Bitmap boardImage = null;

        public PngBoardExport(List<JsonCard> cardsList, FileInfo skinsFile) : base(cardsList, skinsFile)
        { }

        public PngBoardExport(Window owner, List<JsonCard> cardsList, FileInfo skinsFile) : base(owner, cardsList, skinsFile)
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

            Parameters prms = ((Parameters)this.parameters);
            this.targetFolder = prms.TargetFolder;
            this.spaceBeetweenCards = prms.SpaceBetweenCards;
            this.withBackSides = prms.WithBackSides;

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

        protected override void MakeCardExport(JsonCard card)
        {
            bool withBack = this.withBackSides;

            int countX = withBack ? CARD_COUNT_X_WITH_BACK : CARD_COUNT_X;
            int countY = withBack ? CARD_COUNT_Y_WITH_BACK : CARD_COUNT_Y;
            int cardSides = withBack ? 2 : 1;

            for (int i = 0; i < card.Nb; i++)
            {
                MakeCardExportOneCopy(card, withBack, countX, countY, cardSides);
            }
        }

        private void MakeCardExportOneCopy(JsonCard card, bool withBack, int countX, int countY, int cardSides)
        {
            if (this.boardImage == null)
            {
                int w = CARD_WIDTH * countX * cardSides + spaceBeetweenCards * ((countX * cardSides) - 1);
                int h = CARD_HEIGHT * countY + spaceBeetweenCards * (countY - 1);
                boardImage = new Bitmap(w, h);
                boardImage.SetResolution(300, 300);
            }

            DrawCard(card, withBack);

            this.currentX++;

            if (this.currentX == countX)
            {
                this.currentX = 0;
                this.currentY++;
            }

            if (this.currentY == countY)
            {
                this.currentBoard++;

                SaveImage();
                this.boardImage.Dispose();
                this.boardImage = null;

                this.currentX = 0;
                this.currentY = 0;
            }
        }

        private void DrawCard(JsonCard card, bool withBack)
        {
            Drawer drawer = null;
            Image cardFrontImage = null;
            Image cardBackImage;
            Image img = null;

            lock (_lock)
            {
                img = this.boardImage;
            }

            int spaceX = (this.currentX > 0) ? this.spaceBeetweenCards : 0;
            int spaceY = (this.currentY > 0) ? this.spaceBeetweenCards : 0;

            drawer = new Drawer(card, skinsFile, null);
            cardFrontImage = drawer.DrawCard();

            Graphics g = Graphics.FromImage(this.boardImage);
            g.DrawImage(cardFrontImage, new PointF((CARD_WIDTH + spaceX) * currentX, (CARD_HEIGHT + spaceY) * currentY));

            if (withBack)
            {
                cardBackImage = GetBackImage(card, skinsFile);
                g.DrawImage(cardBackImage, new PointF((CARD_WIDTH + spaceX) * (currentX + 1), (CARD_HEIGHT + spaceY) * currentY));
            }

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

        private Image GetBackImage(JsonCard card, FileInfo skinFile)
        {
            Drawer drawer = new Drawer(card, skinFile, null);
            return drawer.DrawBackSideSkin();
        }


        public class Parameters : ExportParameters
        {
            public DirectoryInfo TargetFolder { get; set; } = null;
            public int SpaceBetweenCards { get; set; } = 0;
            public bool WithBackSides { get; set; } = false;
        }
    }
}
