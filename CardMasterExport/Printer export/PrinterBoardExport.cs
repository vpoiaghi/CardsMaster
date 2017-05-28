using CardMasterCard.Card;
using CardMasterCommon.Dialog;
using CardMasterExport.Export;
using CardMasterImageBuilder;
using CardMasterSkin.Skins;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows;
//using System.Windows.Controls;
using System.Windows.Forms;

namespace CardMasterExport.PrinterExport
{
    public class PrinterBoardExport : Exporter, IDisposable
    {
        private const int CARD_WIDTH = 744;
        private const int CARD_HEIGHT = 1038;
        private const int CARD_COUNT_X = 3;
        private const int CARD_COUNT_X_WITH_BACK = 1;
        private const int CARD_COUNT_Y = 3;
        private const int CARD_COUNT_Y_WITH_BACK = 3;

        // Paramètres
        private int spaceBeetweenCards = 0;
        private bool withBackSides = false;

        private int currentX = 0;
        private int currentY = 0;

        int cardIndex = 0;

        private List<Bitmap> boardImages = null;
        private Bitmap boardImage = null;
        private int boardsIndex = 0;

        private int printCardFrom = 0;
        private int printCardTo = 0;

        // Variable temporaire pour n'imprimer qu'une carte pour les tests
        PrintDocument pDoc = null;


        public PrinterBoardExport(List<Card> cardsList, FileInfo skinsFile) : this(null, cardsList, skinsFile)
        { }

        public PrinterBoardExport(Window owner, List<Card> cardsList, FileInfo skinsFile) : base(owner, cardsList, skinsFile)
        {
            pDoc = new PrintDocument();
            pDoc.PrintPage += new PrintPageEventHandler(pdoc_PrintPage);
        }

        public override ExportParameters GetParameters()
        {
            return new Parameters();
        }

        protected override bool BeforeCardsExport()
        {
            bool result = false;

            this.pDoc.DefaultPageSettings.PrinterResolution.Kind = PrinterResolutionKind.High;

            this.pDoc.DefaultPageSettings.PrinterSettings.FromPage = 1;
            this.pDoc.DefaultPageSettings.PrinterSettings.ToPage = this.cardsList.Count;

            PrintDialog printDlg = new PrintDialog();
            printDlg.Document = this.pDoc;

            printDlg.AllowSomePages = true;

            if (printDlg.ShowDialog() == DialogResult.OK)
            {
                this.pDoc.DefaultPageSettings.PrinterResolution.X = 300;
                this.pDoc.DefaultPageSettings.PrinterResolution.Y = 300;

                this.boardImages = new List<Bitmap>();

                this.printCardFrom = this.pDoc.DefaultPageSettings.PrinterSettings.FromPage;
                this.printCardTo = this.pDoc.DefaultPageSettings.PrinterSettings.ToPage;

                this.pDoc.OriginAtMargins = true;

                double cmToUnits = 100 / 2.54;
                int margin = (int)Math.Ceiling(1 * cmToUnits);
                this.pDoc.DefaultPageSettings.Margins.Top = margin;
                this.pDoc.DefaultPageSettings.Margins.Left = margin;
                this.pDoc.DefaultPageSettings.Margins.Bottom = margin;
                this.pDoc.DefaultPageSettings.Margins.Right = margin;

                this.currentX = 0;
                this.currentY = 0;

                Parameters prms = ((Parameters)this.parameters);
                this.spaceBeetweenCards = prms.SpaceBetweenCards;
                this.withBackSides = prms.WithBackSides;

                result = true;
            }

            printDlg.Dispose();
            printDlg = null;

            return result;
        }

        void pdoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            double cmToUnits = 100 / 2.54;
            e.Graphics.DrawImage(boardImages[boardsIndex], 0, 0, (float)(12.6 * cmToUnits), (float)(26.4 * cmToUnits));

            e.HasMorePages = (++boardsIndex < boardImages.Count);
        }

        protected override void AfterCardsExport()
        {
            if (this.boardImages.Count > 0)
            {
                boardsIndex = 0;
                pDoc.Print();

                this.boardImages.Clear();
                this.boardImages = null;
            }
        }

        protected override string GetProgressMessage(ProgressState state, int index, int total)
        {
            string message = "";

            if (state == ProgressState.ExportInProgress)
            {
                message = "Préparation de l'impression en cours : " + index + " / " + total;
            }
            else if (state == ProgressState.ExportEnded)
            {
                message = "Préparation de l'impression terminée";
            }

            return message;
        }

        protected override void MakeCardExport(Card card)
        {
            cardIndex++;

            if ((cardIndex >= printCardFrom) && (cardIndex <= printCardTo))
            {
                int countX = this.withBackSides ? CARD_COUNT_X_WITH_BACK : CARD_COUNT_X;
                int countY = this.withBackSides ? CARD_COUNT_Y_WITH_BACK : CARD_COUNT_Y;
                int cardSides = this.withBackSides ? 2 : 1;

                for (int i = 0; i < card.Nb; i++)
                {
                    MakeCardExportOneCopy(card, this.withBackSides, countX, countY, cardSides);
                }
            }
        }

        private void MakeCardExportOneCopy(Card card, bool withBack, int countX, int countY, int cardSides)
        {
            if (this.boardImage == null)
            {
                this.currentX = 0;
                this.currentY = 0;

                int w = CARD_WIDTH * countX * cardSides + spaceBeetweenCards * ((countX * cardSides) - 1);
                int h = CARD_HEIGHT * countY + spaceBeetweenCards * (countY - 1);

                boardImage = new Bitmap(w, h);
                boardImage.SetResolution(300, 300);

                this.boardImages.Add(this.boardImage);
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
                this.boardImage = null;
            }
        }

        private void DrawCard(Card card, bool withBack)
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

        private Image GetBackImage(Card card, FileInfo skinFile)
        {
            Drawer drawer = new Drawer(card, skinFile, null);
            return drawer.DrawCardBackground();
        }

        public class Parameters : ExportParameters
        {
            public DirectoryInfo TargetFolder { get; set; } = null;
            public int SpaceBetweenCards { get; set; } = 0;
            public bool WithBackSides { get; set; } = false;
        }
    }
}
