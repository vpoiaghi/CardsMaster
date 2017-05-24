using CardMasterCard.Card;
using CardMasterExport.Export;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;

namespace CardMasterExport
{
    public abstract class Exporter
    {
        private bool exportRunning = false;
        protected static object _lock = new object();

        private Window owner = null;
        protected List<Card> cardsList = null;
        protected FileInfo skinsFile = null;
        protected ExportParameters parameters = null;
        
        protected delegate void ProgressChangedEvent(ProgressChangedArg arg);
        public delegate void ProgressChanged(object sender, ProgressChangedArg args);

        public event ProgressChanged progressChangedEvent;

        protected abstract bool BeforeCardsExport();
        protected abstract void MakeCardExport(Card card);
        protected abstract string GetProgressMessage(ProgressState state, int index, int total);

        public abstract ExportParameters GetParameters();

        //public Exporter()
        //{ }

        protected Exporter(List<Card> cardsList, FileInfo skinsFile) : this(null, cardsList, skinsFile)
        { }

        protected Exporter(Window owner, List<Card> cardsList, FileInfo skinsFile)
        {
            if (cardsList == null)
            {
                throw new ArgumentNullException("cardsList");
            }
            if (skinsFile == null)
            {
                throw new ArgumentNullException("skinsFile");
            }

            this.owner = owner;
            this.cardsList = cardsList;
            this.skinsFile = skinsFile;
        }
        public void Dispose()
        {
            owner = null;
            cardsList = null;
            skinsFile = null;
        }

        public void Export(ExportParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            else
            {
                this.parameters = parameters;
            }

            if ((cardsList != null) && (skinsFile != null) && (!exportRunning))
            {
                if (BeforeCardsExport())
                {
                    var t = new Thread(new ThreadStart(StartExport));

                    exportRunning = true;

                    t.IsBackground = true;
                    t.Start();

                    if (this.owner == null)
                    {
                        t.Join();
                    }

                }

                exportRunning = false;
            }

        }

        private void StartExport()
        {
            List<Card> cards;
            lock (_lock)
            {
                cards = this.cardsList;
            }

            if (cards != null && cards.Count > 0)
            {
                int cardIndex = 0;
                int cardCount = cards.Count;

                foreach (Card card in cards)
                {
                    MakeCardExport(card);
                    cardIndex = cardIndex + 1;
                    ShowProgress(cardIndex, cardCount);
                }

                // Export terminé
                ShowExportEnded();

                if (owner != null)
                {
                    // Attend quatre secondes avant d'effacer le dernier message
                    Thread.Sleep(4000);
                    ShowTreatmentEnded();
                }
            }

            exportRunning = false;
        }

        private void ShowProgress(int index, int total)
        {
            string message = GetProgressMessage(ProgressState.ExportInProgress, index, total);
            ShowMessage(ProgressState.ExportInProgress, message, index, total);
        }

        private void ShowExportEnded()
        {
            string message = GetProgressMessage(ProgressState.ExportEnded, 0, 0);
            ShowMessage(ProgressState.ExportEnded, message, 0, 0);
        }

        private void ShowTreatmentEnded()
        {
            ShowMessage(ProgressState.TreatmentEnded, "", 0, 0);
        }

        private void ShowMessage(ProgressState state, string message, int index, int total)
        {
            if (owner != null)
            {
                var arg = new ProgressChangedArg(state, message, index, total);

                try
                {
                    owner.Dispatcher.Invoke((ProgressChangedEvent)SendProgressChangedEvent, arg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
            else
            {
                ClearCurrentConsoleLine();

                if (state == ProgressState.ExportInProgress)
                {
                    Console.Write(message);
                }
                else
                {
                    Console.WriteLine(message);
                }
            }

        }

        private void SendProgressChangedEvent(ProgressChangedArg arg)
        {
            progressChangedEvent(this, arg);
        }

        private void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

    }
}
