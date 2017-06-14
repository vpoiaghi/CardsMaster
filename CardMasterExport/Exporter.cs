using CardMasterCard.Card;
using CardMasterExport.Export;
using CardMasterExport.FileExport;
using CardMasterExport.PrinterExport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;

namespace CardMasterExport
{
    public abstract class Exporter
    {
        public const string EXPORT_MODE_ALL = "all";
        public const string EXPORT_MODE_BOARD = "board";

        public const string EXPORT_FORMAT_PNG = "png";
        public const string EXPORT_FORMAT_PDF = "pdf";
        public const string EXPORT_FORMAT_PRINTER = "printer";


        private bool exportRunning = false;
        protected static object _lock = new object();

        private IExporterOwner owner = null;
        protected List<JsonCard> cardsList = null;
        protected FileInfo skinsFile = null;
        protected ExportParameters parameters = null;

        //protected delegate void ProgressChangedEvent(ProgressChangedArg arg);
        //public delegate void ProgressChanged(object sender, ProgressChangedArg args);
        //public event ProgressChanged progressChangedEvent;

        protected delegate void ProgressChangedEventSender(ProgressChangedArg arg);
        public delegate void ProgressChangedEventHandler(object sender, ProgressChangedArg args);
        public event ProgressChangedEventHandler ProgressChanged;


        protected abstract bool BeforeCardsExport();
        protected abstract void MakeCardExport(JsonCard card);
        protected abstract void AfterCardsExport();
        protected abstract string GetProgressMessage(ProgressState state, int index, int total);


        public void Dispose()
        {
            owner = null;
            cardsList = null;
            skinsFile = null;
        }

        public static void Export(ExportParameters parameters)
        {
            Export(parameters);
        }

        public static void Export(IExporterOwner owner, ExportParameters parameters)
        {
            Exporter exporter = null;

            switch (parameters.exportFormat)
            {
                case EXPORT_FORMAT_PDF:
                    exporter = new PdfExport();
                    break;
                case EXPORT_FORMAT_PNG:
                    switch (parameters.exportMode)
                    {
                        case EXPORT_MODE_ALL:
                            exporter = new PngExport();
                            break;
                        case EXPORT_MODE_BOARD:
                            exporter = new PngBoardExport();
                            break;
                    }
                    break;
                case EXPORT_FORMAT_PRINTER:
                    exporter = new PrinterBoardExport();
                    break;
            }

            if (exporter != null)
            {
                exporter.owner = owner;
                exporter.parameters = parameters;
                exporter.startExport();
            }
            else
            {
                throw new Exception("Aucun exporter trouvé pour ces paramètres.");
            }
        }

        private void startExport()
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            else
            {
                this.cardsList = parameters.cardsList;
                this.skinsFile = parameters.skinsFile;
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
            }

        }

        private void StartExport()
        {
            List<JsonCard> cards;
            lock (_lock)
            {
                cards = this.cardsList;
            }

            if (cards != null && cards.Count > 0)
            {
                int cardIndex = 0;
                int cardCount = cards.Count;

                foreach (JsonCard card in cards)
                {
                    MakeCardExport(card);
                    cardIndex = cardIndex + 1;
                    ShowProgress(cardIndex, cardCount);
                }

                AfterCardsExport();

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
                    if (owner.Dispatcher != null)
                    {
                        // Si l'appelant a un dispatcher valide, on l'utilise pour gérer
                        // le traitement du message de progression dans son propre thread
                        // de façon asynchrone.
                        owner.Dispatcher.Invoke((ProgressChangedEventSender)SendProgressChangedEvent, arg);
                    }
                    else
                    {
                        // Si l'appelant n'a pas dispatcher valide, le traitement du 
                        // message de progression est géré de façon synchrone.
                        SendProgressChangedEvent(arg);
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
            else
            {
                throw new Exception("Exportation : aucun appelant n'a été défini (owner=null).");

                //ClearCurrentConsoleLine();

                //if (state == ProgressState.ExportInProgress)
                //{
                //    Console.Write(message);
                //}
                //else
                //{
                //    Console.WriteLine(message);
                //}
            }

        }

        private void SendProgressChangedEvent(ProgressChangedArg arg)
        {
            //ProgressChanged(this, arg);
            owner.ExportProgressChanged(this, arg);
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
