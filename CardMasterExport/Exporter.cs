using CardMasterCard.Card;
using CardMasterExport.Export;
using CardMasterExport.FileExport;
using CardMasterExport.PrinterExport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Threading;

namespace CardMasterExport
{
    public abstract class Exporter
    {
        public const string EXPORT_MODE_ALL = "all";
        public const string EXPORT_MODE_BOARD = "board";

        public const string EXPORT_FORMAT_PNG = "png";
        public const string EXPORT_FORMAT_PDF = "pdf";
        public const string EXPORT_FORMAT_PRINTER = "printer";


        private static bool exportRunning = false;
        protected static object _lock = new object();

        private IExporterOwner owner = null;
        private ExportParameters parameters = null;
        protected List<JsonCard> cardsList = null;
        protected FileInfo skinsFile = null;

        protected delegate void ProgressChangedEventSender(ProgressChangedArg arg);

        protected abstract bool BeforeCardsExport(ExportParameters parameters);
        protected abstract void MakeCardExport(JsonCard card);
        protected abstract void AfterCardsExport();
        protected abstract string GetProgressMessage(ProgressState state, int index, int total);


        public void Dispose()
        {
            owner = null;
            cardsList = null;
            skinsFile = null;
        }

        public static void Export(IExporterOwner owner, ExportParameters parameters)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("Owner", "Export");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters", "Export");
            }
            if (parameters.cardsList == null)
            {
                throw new ArgumentNullException("parameters.cardsList", "Export");
            }
            if (parameters.skinsFile == null)
            {
                throw new ArgumentNullException("parameters.skinsFile", "Export");
            }
            if (! parameters.skinsFile.Exists)
            {
                throw new FileNotFoundException("Export : le fichier skinFile n'existe pas.");
            }
            if (parameters.cardsList.Count == 0)
            {
                throw new ArgumentException("Export : Il n'y a aucune carte a exporter.");
            }

            Exporter exporter = GetExporter(parameters);

            exporter.owner = owner;
            exporter.parameters = parameters;
            exporter.cardsList = parameters.cardsList;
            exporter.skinsFile = parameters.skinsFile;

            Export(exporter);
        }

        private static Exporter GetExporter(ExportParameters parameters)
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

            if (exporter == null)
            {
                throw new Exception("Aucun exporter trouvé pour ces paramètres.");
            }

            return exporter;
        }

        private static void Export(Exporter exporter)
        {
            if (!exportRunning)
            {
                if (exporter.BeforeCardsExport(exporter.parameters))
                {
                    exportRunning = true;

                    Thread t = new Thread(exporter.StartExport);
                    t.IsBackground = true;
                    t.Start(exporter.cardsList);

                    if (! (exporter.owner is IThreadedExporterOwner))
                    {
                        t.Join();
                    }
                }
            }

        }

        public void StartExport(object cardsList)
        {
            List<JsonCard> cards = (List<JsonCard>) cardsList;
            int cardsCount = cards.Count;

            if (cardsCount > 0)
            {
                int cardIndex = 0;

                foreach (JsonCard card in cards)
                {
                    MakeCardExport(card);
                    cardIndex = cardIndex + 1;
                    ShowProgress(cardIndex, cardsCount);
                }

                AfterCardsExport();

                // Export terminé
                ShowExportEnded();

                // Attend quatre secondes avant d'effacer le dernier message
                Thread.Sleep(4000);
                ShowTreatmentEnded();
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
                Dispatcher dispatcher = null;

                try
                {
                    if (owner is IThreadedExporterOwner)
                    {
                        dispatcher = ((IThreadedExporterOwner)owner).Dispatcher;
                    }

                    if (dispatcher != null)
                    {
                        // Si l'appelant a un dispatcher valide, on l'utilise pour gérer
                        // le traitement du message de progression dans son propre thread
                        // de façon asynchrone.
                        dispatcher.Invoke((ProgressChangedEventSender)SendProgressChangedEvent, arg);
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
        }

        private void SendProgressChangedEvent(ProgressChangedArg arg)
        {
            owner.ExportProgressChanged(this, arg);
        }

    }
}
