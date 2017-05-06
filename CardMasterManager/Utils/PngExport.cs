using CardMasterImageBuilder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;

namespace CardMasterManager.Utils
{
    class PngExport : IDisposable
    {
        private static Object _lock = new Object();

        private bool exportRunning = false;
        private string targetFolder = null;

        private Window owner;
        private List<Card> cardsList = null;
        private FileInfo skinsFile = null;

        public delegate void ShowExportProgress(int index, int total);
        public delegate void ProgressChanged(object sender, ProgressChangedArg args);

        public event ProgressChanged progressChangedEvent;

        public PngExport(Window owner) {
            this.owner = owner;
        }


        public void Export(List<Card> cardsList, FileInfo skinsFile)
        {
            if ((cardsList != null) && (skinsFile != null) && (!exportRunning))
            {
                lock (_lock)
                {
                    this.cardsList = cardsList;
                    this.skinsFile = skinsFile;
                    this.targetFolder = FolderDialog.SelectFolder();
                }
                    
                if (targetFolder != null)
                {
                    Thread t = new Thread(new ThreadStart(ExportCard));

                    exportRunning = true;

                    t.IsBackground = true;
                    t.Start();
                }

                exportRunning = false;

            }

        }
        
        private void ExportCard()
        {
            Drawer drawer = null;
            System.Drawing.Image img = null;

            List<Card> cards = null;
            string targetFolder = null;

            lock (_lock){
                cards = cardsList;
                targetFolder = this.targetFolder;
            }

            int cardIndex = 0;
            int cardCount = cards.Count;

            foreach (Card card in cards)
            {
                string fileName = card.Name.Replace(":", "-").Replace("\"", "'").Replace("*", " ");

                drawer = new Drawer(Card.ConvertToMasterCard(card), skinsFile, null);
                img = drawer.DrawCard();

                img.Save(Path.Combine(targetFolder, fileName + ".png"));
                img.Dispose();

                drawer = null;
                img = null;

                cardIndex = cardIndex + 1;

                ShowProgress(cardIndex, cardCount);
            }

            Thread.Sleep(4000);
            ShowProgress(0, 0);

            exportRunning = false;

        }

        private void ShowProgress(int index, int total)
        {
            try
            {
                owner.Dispatcher.Invoke((ShowExportProgress)Progres, index, total);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

        }

        private void Progres(int index, int total)
        {

            string message = "";

            if (index < total)
            {
                message = "Export en cours : " + index + " / " + total;
            }
            else if ((index == 0) && (total == 0))
            {
                message = "";
            }
            else if (index == total)
            {
                message = "Export terminé";
            }
            
            Console.WriteLine(message);

            progressChangedEvent(this, new ProgressChangedArg(index, total, message));

        }

        public void Dispose()
        {
            owner = null;
            cardsList = null;
            skinsFile = null;
        }

        public class ProgressChangedArg
        {
            public ProgressChangedArg(int index, int total, string message)
            {
                this.Index = index;
                this.Total = total;
                this.Message = message;
            }
        
            public int Index { get; }
            public int Total { get; }
            public string Message { get; }
        }
        
    }
}
