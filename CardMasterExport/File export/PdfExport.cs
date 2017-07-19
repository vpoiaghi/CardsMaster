using CardMasterCard.Card;
using CardMasterExport.Export;
using CardMasterImageBuilder;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Drawing;
using System.IO;
using System.Windows;

namespace CardMasterExport.FileExport
{
    public class PdfExport : Exporter, IDisposable
    {
        private const int CARD_WIDTH = 744;
        private const int CARD_HEIGHT = 1038;
        private const int CARD_COUNT_X = 3;
        private const int CARD_COUNT_X_WITH_BACK = 1;
        private const int CARD_COUNT_Y = 3;
        private const int CARD_COUNT_Y_WITH_BACK = 3;

        private FileInfo targetFile = null;
        private int spaceBeetweenCards = 0;
        private bool withBackSides = false;

        private int currentX = 0;
        private int currentY = 0;
        private int currentBoard = 0;
        private Bitmap boardImage = null;
        private Document doc = null;

        protected override bool BeforeCardsExport(ExportParameters parameters)
        {
            bool result = false;

            if (parameters.TargetFile != null) {
                this.currentX = 0;
                this.currentY = 0;
                this.currentBoard = 0;
                this.boardImage = null;

                this.targetFile = parameters.TargetFile;
                this.spaceBeetweenCards = parameters.SpaceBetweenCards;
                this.withBackSides = parameters.WithBackSides;

                doc = new Document(PageSize.A4);
                try
                {
                    PdfWriter.GetInstance(doc, new FileStream(targetFile.FullName, FileMode.Create));
                    doc.Open();
                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }
            }

            return result;
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
            //PdfDocument myDoc = new pdfDocument("TUTORIAL", "ME");
            //pdfPage myPage = myDoc.addPage();
            //myPage.addText("Hello World!", 200, 450, predefinedFont.csHelvetica, 20);
            //myDoc.createPDF(@"c:\test.pdf");

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
            System.Drawing.Image cardFrontImage = null;
            System.Drawing.Image cardBackImage;
            System.Drawing.Image img = null;

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

        private System.Drawing.Image GetBackImage(JsonCard card, FileInfo skinFile)
        {
            Drawer drawer = new Drawer(card, skinFile, null);
            return drawer.DrawBackSideSkin();
        }

        private void SaveImage()
{
            string targetFile = null;

            lock (_lock)
            {
                targetFile = this.targetFile.FullName;
            }

            try {
                iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(boardImage, System.Drawing.Imaging.ImageFormat.Png);

                if (pic.Height > pic.Width)
                {
                    //Maximum height is 800 pixels.
                    float percentage = 0.0f;
                    percentage = 700 / pic.Height;
                    pic.ScalePercent(percentage * 100);
                }
                else
                {
                    //Maximum width is 600 pixels.
                    float percentage = 0.0f;
                    percentage = 540 / pic.Width;
                    pic.ScalePercent(percentage * 100);
                }

                doc.Add(pic);
                doc.NewPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK , MessageBoxImage.Error);
            }
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

            doc.Close();
            doc = null;
        }

    }
}
