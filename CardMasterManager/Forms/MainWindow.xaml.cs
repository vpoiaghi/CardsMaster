using CardMasterCard.Card;
using CardMasterCommon.Dialog;
using CardMasterExport;
using CardMasterExport.Export;
using CardMasterImageBuilder;
using CardMasterManager.Converters;
using CardMasterManager.Forms;
using CardMasterManager.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CardMasterManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IThreadedExporterOwner
    {
        public ObservableCollection<Card> GridCardsList { get; set; } = new ObservableCollection<Card>();

        private static object locker = new object();

        private JsonCardsProject cardProjet;
        private Card previousCard;

        private FileInfo cardsFile = null;
        private bool filesChanged = false;

        private DateTime d1;
        private DateTime d2;
        private bool isSearching = false;

        public DrawingQuality DQuality { get; set; } = new DrawingQuality();

        public MainWindow()
        {
            InitializeComponent();

            GridConfigurator.LoadAndApplyConfiguration(cardGrid);

            this.MenuItemExportAllToPngFile.IsEnabled = false;
            this.MenuItemExportBoardsToPngFile.IsEnabled = false;
            this.MenuItemPrintBoards.IsEnabled = false;
            this.MenuItemSaveAsJson.IsEnabled = false;


            FilesChanged(false);

            DataContext = this;
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            if (AskIfSaveBefore())
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "All Supported files|*.json;*.xlsx;*.xml|All files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == true)
                {
                    this.cardsFile = new FileInfo(openFileDialog.FileName);
                    this.cardProjet = JsonCardsProject.LoadProject(cardsFile);

                    List<Card> cards = new List<Card>();
                    foreach (JsonCard card in this.cardProjet.Cards)
                    {
                        Card c = Card.ConvertCard(card);
                        cards.Add(c);
                    }

                    LoadCards(cards);

                    FilesChanged(false);
                    this.MenuItemExportAllToPngFile.IsEnabled = true;
                    this.MenuItemExportBoardsToPngFile.IsEnabled = true;
                    this.MenuItemPrintBoards.IsEnabled = true;
                }
            }
        }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            if (cardsFile == null)
            {
                MenuItemSaveAs_Click(sender, e);
            }
            else
            {
                SaveProject(cardsFile);
            }
        }

        private void MenuItemSaveAsJson_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                FileInfo newCardsFile = new FileInfo(saveFileDialog.FileName);
                SaveProject(newCardsFile);
                this.cardsFile = newCardsFile;
            }
        }

        private void MenuItemSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                FileInfo newCardsFile = new FileInfo(saveFileDialog.FileName);

                FileInfo oldSkinsFile = GetSkinFile(cardsFile);
                FileInfo newSkinsFile = GetSkinFile(newCardsFile);

                SaveProject(newCardsFile);
                oldSkinsFile.CopyTo(newSkinsFile.FullName);

                DirectoryInfo oldResourceDir = new DirectoryInfo(Path.Combine(cardsFile.Directory.FullName, "Resources"));
                DirectoryInfo newResourceDir = new DirectoryInfo(Path.Combine(newCardsFile.Directory.FullName, "Resources"));

                if (oldResourceDir.FullName != newResourceDir.FullName)
                {
                    DirectoryUtils.Copy(oldResourceDir, newResourceDir);
                }

                this.cardsFile = newCardsFile;
            }
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            if (AskIfSaveBefore())
            {
                Close();
            }
        }
 

        private void cardGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cardsFile != null) && (cardGrid.SelectedItem != null))
            {
                if (!((cardGrid.SelectedIndex == 0) && (GridCardsList.Count == 0)))
                {
                    // Si on est pas dans le cas d'un nouveau fichier

                    Card c = GridCardsList[cardGrid.SelectedIndex];

                    if (!(c == previousCard))
                    {
                        previousCard = c;
                        new Thread(() => DisplayCard(c, cardImage, backCardImage)).Start();
                    }
                }
            }
        }

        private void DisplayCard(Card c, System.Windows.Controls.Image frontImage, System.Windows.Controls.Image backImage)
        {
            //Select Card from Collection from Name
            JsonCard businessCard = Card.ConvertToMasterCard(c);
            FileInfo skinFile = GetSkinFile(cardsFile);

            Drawer drawer = new Drawer(businessCard, skinFile, null);
            drawer.Quality = this.DQuality;

            //Refresh Image Component
            DrawingImageToImageSourceConverter converter = new DrawingImageToImageSourceConverter();
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                frontImage.Source = (ImageSource)converter.Convert(drawer.DrawCard(), null, null, System.Globalization.CultureInfo.CurrentCulture);
                backCardImage.Source = (ImageSource)converter.Convert(drawer.DrawBackSideSkin(), null, null, System.Globalization.CultureInfo.CurrentCulture);
            }));
        }

        private void ComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if ((cardGrid.SelectedItem != null) && (cardGrid.SelectedIndex > 0))
            {
                Card card = GridCardsList[cardGrid.SelectedIndex];
                ComboBox cb = (ComboBox)sender;
                cardGrid.SelectedItem = card;
            }
        }

        private void MenuItemExportAllToPngFile_Click(object sender, RoutedEventArgs e)
        {
            List<JsonCard> cardsList = GridCardsListToJsonCardsList();

            if (cardsList.Count > 0)
            {
                ExportParameters parameters = new ExportParameters(cardsList, GetSkinFile(cardsFile));
                parameters.exportFormat = Exporter.EXPORT_FORMAT_PNG;
                parameters.exportMode = Exporter.EXPORT_MODE_ALL;
                parameters.TargetFolder = FolderDialog.SelectFolder();

                Exporter.Export(this, parameters);
            }

        }

        private void MenuItemExportBoardsToPngFile_Click(object sender, RoutedEventArgs e)
        {
            List<JsonCard> cardsList = GridCardsListToJsonCardsList();

            if (cardsList.Count > 0)
            {
                ExportParameters parameters = new ExportParameters(cardsList, GetSkinFile(cardsFile));
                parameters.SpaceBetweenCards = 0;
                parameters.exportFormat = Exporter.EXPORT_FORMAT_PNG;
                parameters.exportMode = Exporter.EXPORT_MODE_BOARD;
                parameters.TargetFolder = FolderDialog.SelectFolder();

                Exporter.Export(this, parameters);
            }

        }

        public void ExportProgressChanged(object sender, ProgressChangedArg args)
        {
            debug.Text = args.Message;

            if (args.Index == 1)
            {
                d1 = DateTime.Now;
            }

            if (args.State == ProgressState.ExportEnded)
            {
                d2 = DateTime.Now;
                //MessageBox.Show((d2.Subtract(d1)).TotalSeconds.ToString());
            }

        }

        private void LoadCards(List<Card> cards)
        {
            GridCardsList.Clear();

            foreach (Card card in cards)
            {
                GridCardsList.Add(card);
            }


        }

        private void SaveProject(FileInfo cardsFile)
        {
            if ((cardProjet != null) && (cardsFile != null))
            {
                cardProjet.Cards = GridCardsListToJsonCardsList();
                cardProjet.Save(cardsFile);

                FilesChanged(false);
            }
        }

        private void MenuItemPrintBoards_Click(object sender, RoutedEventArgs e)
        {
            List<JsonCard> cardsList = GridCardsListToJsonCardsList();

            if (cardsList.Count > 0)
            {
                ExportParameters parameters = new ExportParameters(cardsList, GetSkinFile(cardsFile));

                parameters.printPrameters = PrinterDialog.SelectPrintParameters(cardsList.Count);

                if (parameters.printPrameters != null)
                {
                    parameters.SpaceBetweenCards = 0;
                    parameters.WithBackSides = true;
                    parameters.exportFormat = Exporter.EXPORT_FORMAT_PRINTER;
                    parameters.exportMode = Exporter.EXPORT_MODE_BOARD;

                    Exporter.Export(this, parameters);
                }
            }

        }

        private FileInfo GetSkinFile(FileInfo cardsFile)
        {
            if (cardsFile == null)
            {
                throw new ArgumentNullException("cardsFile", "Restitution du nom du fichier skin impossible sans une référence au fichier .json");
            }

            FileInfo skinFile = new FileInfo(Path.Combine(cardsFile.Directory.FullName, cardsFile.Name.Replace(cardsFile.Extension, ".skin")));

            if (! skinFile.Exists)
            {
                throw new FileNotFoundException("Le fichier '" + skinFile.FullName + "' n'existe pas ou est inaccessible.", skinFile.Name);
            }

            return  skinFile;
        }

        private void ClearSearch(object sender, RoutedEventArgs e)
        {
            searchText.Text = "";
        }

        private void filterDataGrid(object sender, TextChangedEventArgs e)
        {
            if ((GridCardsList.Count > 0) && (!isSearching))
            {
                String filterText = ((TextBox)sender).Text;
                if (filterText.Equals(null) || filterText.Equals(""))
                {
                    Dispatcher.BeginInvoke(new Action(delegate ()
                    {
                        isSearching = true;
                        cardGrid.Items.Filter = null;
                        isSearching = false;
                    }));
                }
                else
                {
                    Dispatcher.BeginInvoke(new Action(delegate ()
                    {
                        isSearching = true;
                        cardGrid.Items.Filter = (c) =>
                        {
                            return CardMatcher.Matches((Card)c, filterText);
                        };
                        isSearching = false;
                    }));
                    
                }
            }
          
        }

        private void DisplayConfigurator(object sender, RoutedEventArgs e)
        {
            GridConfigurator form = new GridConfigurator(cardGrid);
            form.Owner = this;
            form.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            form.Top = 300;
            form.Left = 300;
            form.ShowDialog();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (AskIfSaveBefore())
            {
                GridConfigurator.BuildAndSaveConfiguration(cardGrid);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void AddRowClick(object sender, RoutedEventArgs e)
        {

            Card newCard = new Card();
            newCard.Nature = NatureCard.Eau;
            newCard.Kind = CardKind.Ninja;
            newCard.Name = "Template";
            newCard.Powers = new List<JsonPower>();
            newCard.BackSkinName = "BackSkin1";
            newCard.FrontSkinName = "FrontSkin1";
            newCard.BackSide = "Back-Draw2";
            newCard.Rank = "";
            JsonPower p = new JsonPower(); p.Description = "Power";
            newCard.Powers.Add(p);

            int indexWhereToInsert = 0;
            if (GridCardsList.Count == 0 )
            {
                indexWhereToInsert = 0;
            }else if( cardGrid.SelectedIndex == -1)
            {
                indexWhereToInsert = cardGrid.Items.Count;
            }
            else
            {
                indexWhereToInsert = cardGrid.SelectedIndex;
            }
            GridCardsList.Insert(indexWhereToInsert, newCard);
            cardGrid.SelectedIndex = indexWhereToInsert;
            cardGrid.Focus();

            FilesChanged(true);
        }  

        private void DeleteRowClick(object sender, RoutedEventArgs e)
        {
            if (cardGrid.SelectedIndex != -1)
            {
                int selectedIndex = cardGrid.SelectedIndex;
                GridCardsList.RemoveAt(selectedIndex);
                if (cardGrid.Items.Count > 0)
                {
                    cardGrid.SelectedIndex = selectedIndex == 0 ? 0 : selectedIndex - 1;
                    cardGrid.Focus();
                }
                FilesChanged(true);
            }
        }

        private void MoveDownRowClick(object sender, RoutedEventArgs e)
        {
            if (cardGrid.SelectedIndex != -1 && cardGrid.SelectedIndex<cardGrid.Items.Count-1)
            {
                int originalIndex = cardGrid.SelectedIndex;
                int targetIndex = originalIndex + 1;
                SwapGridLines(originalIndex, targetIndex);
            }
        }
        private void MoveUpRowClick(object sender, RoutedEventArgs e)
        {
            if (cardGrid.SelectedIndex >0 )
            {
                int originalIndex = cardGrid.SelectedIndex;
                int targetIndex = originalIndex - 1;
                SwapGridLines(originalIndex, targetIndex);
            }
        }

        private void SwapGridLines(int sourceIndex,int targetIndex)
        {
            GridCardsList.Move(sourceIndex, targetIndex);
            cardGrid.Focus();
            FilesChanged(true);
        }

        private void FilesChanged(bool changed)
        {
            this.MenuItemSave.IsEnabled = changed;
            this.MenuItemSaveAs.IsEnabled = changed;
            this.MenuItemSaveAsJson.IsEnabled = true;
            this.filesChanged = changed;
        }

        private bool AskIfSaveBefore()
        {
            bool result = true;

            if (this.filesChanged == true)
            {
                String source = this.cardsFile == null ? "la grille" : this.cardsFile.Name;
                MessageBoxResult msgResult = MessageBox.Show("Voulez-vous enregistrer les modifications apportées à '" + source + "' ?", 
                    "Enregistrer...", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);

                switch (msgResult)
                {
                    case MessageBoxResult.Yes:
                        SaveProject(cardsFile);
                        break;
                    case MessageBoxResult.No:
                        // Rien à faire
                        break;
                    case MessageBoxResult.Cancel:
                        result = false;
                        break;
                }

            }

            return result;
        }

        private List<JsonCard> GridCardsListToJsonCardsList()
        {
            List<JsonCard> jsonCardsList = new List<JsonCard>();

            foreach (object item in GridCardsList)
            {
                jsonCardsList.Add(Card.ConvertToMasterCard((Card)item));
            }

            return jsonCardsList;
        }

        private void cardGrid_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            FilesChanged(true);
        }

      
    }
}
