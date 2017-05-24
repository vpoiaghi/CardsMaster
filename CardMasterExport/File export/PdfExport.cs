using CardMasterCard.Card;
using CardMasterExport.Export;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace CardMasterExport.FileExport
{
    public class PdfExport : Exporter, IDisposable
    {
        private FileInfo targetFile = null;

        public PdfExport(List<Card> cardsList, FileInfo skinsFile) : base(cardsList, skinsFile)
        { }

        public PdfExport(Window owner, List<Card> cardsList, FileInfo skinsFile) : base(owner, cardsList, skinsFile)
        { }

        public override ExportParameters GetParameters()
        {
            return new Parameters();
        }

        protected override bool BeforeCardsExport()
        {
            this.targetFile = ((Parameters)this.parameters).TargetFile;
            throw new NotImplementedException();
        }

        protected override string GetProgressMessage(ProgressState state, int index, int total)
        {
            throw new NotImplementedException();
        }

        protected override void MakeCardExport(Card card)
        {
            //PdfDocument myDoc = new pdfDocument("TUTORIAL", "ME");
            //pdfPage myPage = myDoc.addPage();
            //myPage.addText("Hello World!", 200, 450, predefinedFont.csHelvetica, 20);
            //myDoc.createPDF(@"c:\test.pdf");
        }

        public class Parameters : ExportParameters
        {
            public FileInfo TargetFile { get; set; } = null;
        }
    }
}
