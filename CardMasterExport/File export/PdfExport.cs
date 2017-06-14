using CardMasterCard.Card;
using System;
using System.IO;

namespace CardMasterExport.FileExport
{
    public class PdfExport : Exporter, IDisposable
    {
        private FileInfo targetFile = null;

        protected override bool BeforeCardsExport()
        {
            this.targetFile = this.parameters.TargetFile;
            throw new NotImplementedException();
        }

        protected override string GetProgressMessage(ProgressState state, int index, int total)
        {
            throw new NotImplementedException();
        }

        protected override void MakeCardExport(JsonCard card)
        {
            //PdfDocument myDoc = new pdfDocument("TUTORIAL", "ME");
            //pdfPage myPage = myDoc.addPage();
            //myPage.addText("Hello World!", 200, 450, predefinedFont.csHelvetica, 20);
            //myDoc.createPDF(@"c:\test.pdf");
        }

        protected override void AfterCardsExport()
        { }

    }
}
