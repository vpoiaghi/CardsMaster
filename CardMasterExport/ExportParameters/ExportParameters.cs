using CardMasterCard.Card;
using System;
using System.Collections.Generic;
using System.IO;

namespace CardMasterExport.Export
{
    public class ExportParameters
    {
        public List<JsonCard> cardsList { get; }
        public FileInfo skinsFile { get; }

        public string exportFormat { get; set; } = Exporter.EXPORT_FORMAT_PNG;
        public string exportMode { get; set; } = Exporter.EXPORT_MODE_ALL;

        public FileInfo TargetFile { get; set; } = null;
        public DirectoryInfo TargetFolder { get; set; } = null;

        public int SpaceBetweenCards { get; set; } = 0;
        public bool WithBackSides { get; set; } = false;

        public ExportParameters(JsonCard card, FileInfo skinsFile)
        {
            if (card == null)
            {
                throw new ArgumentNullException("card");
            }
            if (skinsFile == null)
            {
                throw new ArgumentNullException("skinsFile");
            }

            this.cardsList = new List<JsonCard>();
            this.cardsList.Add(card);

            this.skinsFile = skinsFile;
        }

        public ExportParameters(List<JsonCard> cardsList, FileInfo skinsFile)
        {
            if (cardsList == null)
            {
                throw new ArgumentNullException("cardsList");
            }
            if (skinsFile == null)
            {
                throw new ArgumentNullException("skinsFile");
            }

            this.cardsList = cardsList;
            this.skinsFile = skinsFile;
        }


    }
}
