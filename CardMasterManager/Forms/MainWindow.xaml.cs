using CardMasterCard.Card;
using CardMasterCommon.Dialog;
using CardMasterExport;
using CardMasterExport.Export;
using CardMasterImageBuilder;
using CardMasterManager.Converters;
using CardMasterManager.Forms;
using CardMasterManager.Models.Card;
using CardMasterManager.Utils;
using Microsoft.Win32;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfCharts;

namespace CardMasterManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IThreadedExporterOwner
    {
        public ObservableCollection<Card> GridCardsList { get; set; } = new ObservableCollection<Card>();
        public string[] Axes { get; set; }
       public ObservableCollection<ChartLine> Stats { get; set; }

        public int GridCount {
            set {
                int x = 0;
                foreach( Card c in GridCardsList)
                {
                    x += c.Nb.Value;
                }
                nbCards.Text = value + " cards / " + x + " total cards";
            }
        }
       


        private static object locker = new object();

        private JsonCardsProject cardProjet;
        private Card previousCard;

        private FileInfo cardsFile = null;
        private bool filesChanged = false;

        private DateTime d1;
        private DateTime d2;
        
        public DrawingQuality DQuality { get; set; } = new DrawingQuality();

        public MainWindow()
        {
            InitializeComponent();

            GridConfigurator.LoadAndApplyConfiguration(cardGrid);

            this.MenuItemExportAllToPngFile.IsEnabled = false;
            this.MenuItemExportBoardsToPngFile.IsEnabled = false;
            this.MenuItemPrintBoards.IsEnabled = false;
            this.MenuItemSaveAsJson.IsEnabled = false;
            this.MenuItemExportGameCrafterToPngFile.IsEnabled = false;
          
          

            FilesChanged(false);
            
            DataContext = this;
            GridCount = 0;
            Axes = new[] { "Coût", "ATK", "DEF", "Pouvoirs" };
            Stats = new ObservableCollection<ChartLine>();
        }

        private void UpdateStats(Card c)
        {
            Stats.Clear();
            Stats.Add(new ChartLine
                {
                    LineColor = Colors.Red,
                    FillColor = Color.FromArgb(128, 255, 0, 0),
                    LineThickness = 1,
                    PointDataSource = new List<double>() { GetInt(c.Cost), GetInt(c.Attack), GetInt(c.Defense), c.Powers.Count },
                    Name = ""
                });
         
                
        }

        private double GetInt(string cost)
        {
            Double.TryParse(cost, out double mvalue);
            return mvalue;
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            if (AskIfSaveBefore())
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "All Supported files|*.json;*.xlsx;*.xml|All files (*.*)|*.*"
                };

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
                    this.MenuItemExportGameCrafterToPngFile.IsEnabled = true;
                    
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
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Json files (*.json)|*.json|All files (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                FileInfo newCardsFile = new FileInfo(saveFileDialog.FileName);
                SaveProject(newCardsFile);
                this.cardsFile = newCardsFile;
            }
        }

        private void MenuItemSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Json files (*.json)|*.json|All files (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                FileInfo newCardsFile = new FileInfo(saveFileDialog.FileName);
                FileInfo newSkinsFile = GetSkinFile(newCardsFile, false);
                DirectoryInfo newResourceDir = new DirectoryInfo(Path.Combine(newCardsFile.Directory.FullName, "Resources"));

                if (cardsFile != null)
                {
                    FileInfo oldSkinsFile = GetSkinFile(cardsFile, true);
                    oldSkinsFile.CopyTo(newSkinsFile.FullName);

                    DirectoryInfo oldResourceDir = new DirectoryInfo(Path.Combine(cardsFile.Directory.FullName, "Resources"));

                    if (oldResourceDir.FullName != newResourceDir.FullName)
                    {
                        DirectoryUtils.Copy(oldResourceDir, newResourceDir);
                    }

                }
                else
                {
                    cardProjet = new JsonCardsProject();

                    if (newSkinsFile.Exists)
                    {
                        newSkinsFile.Delete();
                    }
                    newSkinsFile.Create();

                    if (! newResourceDir.Exists)
                    {
                        newResourceDir.Create();
                    }
                }

                this.cardsFile = newCardsFile;
                SaveProject(newCardsFile);
            }
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            if (AskIfSaveBefore())
            {
                Close();
            }
        }
 

        private void CardGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = cardGrid.SelectedIndex;
            if ((cardsFile != null) && (cardGrid.SelectedItem != null))
            {
                if (selectedIndex < GridCardsList.Count)
                {
                    // Si on est pas dans le cas d'un nouveau fichier
                    //L'observable n'est pas syncrhonizé avec la collection filtrée
                    //Card c = GridCardsList[selectedIndex];
                    Card c = (Card)cardGrid.SelectedItem;
                    UpdateStats(c);
                    //dISPLAY COMMENT
                    CommentTextBlock.Text = c.Comments;
                    //If not preview
                    if (!(c == previousCard) && previewCheckBox.IsChecked == true)
                    {
                        
                        previousCard = c;
                        new Thread(() => DisplayCard(c, cardImage, backCardImage)).Start();
                    }else
                    {
                        //Display empty image
                        cardImage.Source = null;
                        backCardImage.Source = null;
                    }
                }
            }
        }

        private void DisplayCard(Card c, System.Windows.Controls.Image frontImage, System.Windows.Controls.Image backImage)
        {
          
            {
                //Select Card from Collection from Name
                JsonCard businessCard = Card.ConvertToMasterCard(c);
                FileInfo skinFile = GetSkinFile(cardsFile, true);

                Drawer drawer = new Drawer(businessCard, skinFile, null)
                {
                    Quality = this.DQuality
                };

                //Refresh Image Component
                DrawingImageToImageSourceConverter converter = new DrawingImageToImageSourceConverter();
                Dispatcher.BeginInvoke(new Action(delegate ()
                {
                    frontImage.Source = (ImageSource)converter.Convert(drawer.DrawCard(), null, null, System.Globalization.CultureInfo.CurrentCulture);
                    backCardImage.Source = (ImageSource)converter.Convert(drawer.DrawBackCard(), null, null, System.Globalization.CultureInfo.CurrentCulture);
                }));
                
            }
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
                ExportParameters parameters = new ExportParameters(cardsList, GetSkinFile(cardsFile, true))
                {
                    exportFormat = Exporter.EXPORT_FORMAT_PNG,
                    exportMode = Exporter.EXPORT_MODE_ALL,
                    WithBackSides = true,
                    TargetFolder = FolderDialog.SelectFolder()
                };

                Exporter.Export(this, parameters);
                NotifyExportDone();
            }

        }

        private void MenuItemExportBoardsToPngFile_Click(object sender, RoutedEventArgs e)
        {
            List<JsonCard> cardsList = GridCardsListToJsonCardsList();

            if (cardsList.Count > 0)
            {
                ExportParameters parameters = new ExportParameters(cardsList, GetSkinFile(cardsFile, true))
                {
                    SpaceBetweenCards = 0,
                    exportFormat = Exporter.EXPORT_FORMAT_PNG,
                    exportMode = Exporter.EXPORT_MODE_BOARD,
                    WithBackSides = true,
                    TargetFolder = FolderDialog.SelectFolder()
                };

                Exporter.Export(this, parameters);
                NotifyExportDone();
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
            debug.Text = GridCardsList.Count + " cards loaded";
            GridCount = cardGrid.Items.Count;


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
                ExportParameters parameters = new ExportParameters(cardsList, GetSkinFile(cardsFile, true))
                {
                    printPrameters = PrinterDialog.SelectPrintParameters(cardsList.Count)
                };

                if (parameters.printPrameters != null)
                {
                    parameters.SpaceBetweenCards = 0;
                    parameters.WithBackSides = true;
                    parameters.exportFormat = Exporter.EXPORT_FORMAT_PRINTER;
                    parameters.exportMode = Exporter.EXPORT_MODE_BOARD;

                    Exporter.Export(this, parameters);
                    NotifyExportDone();
                }
            }

        }

        private FileInfo GetSkinFile(FileInfo cardsFile, bool mustExists)
        {

            if (cardsFile == null)
            {
                throw new ArgumentNullException("cardsFile", "Restitution du nom du fichier skin impossible sans une référence au fichier .json");
            }

            FileInfo skinFile = new FileInfo(Path.Combine(cardsFile.Directory.FullName, cardsFile.Name.Replace(cardsFile.Name, this.cardProjet.SkinFile)));

            if ((mustExists) && (! skinFile.Exists))
            {
                throw new FileNotFoundException("Le fichier '" + skinFile.FullName + "' n'existe pas ou est inaccessible.", skinFile.Name);
            }

            return  skinFile;
        }

        private void ClearSearch(object sender, RoutedEventArgs e)
        {
            ClearSearch();
        }

       private void ClearSearch()
        {
            searchText.Text = "";
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                cardGrid.Items.Filter = null;
                GridCount = cardGrid.Items.Count;
            }));
        }

        private void DisplayConfigurator(object sender, RoutedEventArgs e)
        {
            GridConfigurator form = new GridConfigurator(cardGrid)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Top = 300,
                Left = 300
            };
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
            Card newCard = CardTemplate.BuildTemplateCard();
           
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
            GridCount = cardGrid.Items.Count;
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
                GridCount = cardGrid.Items.Count;
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
            this.MenuItemSaveAs.IsEnabled = true;
            this.MenuItemSaveAsJson.IsEnabled = true;
            this.filesChanged = changed;
            debug.Text = GridCardsList.Count + " cards loaded";
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

        private void CardGrid_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            FilesChanged(true);
        }

        private void MenuItemExportBoardsToPdfFile_Click(object sender, RoutedEventArgs e)
        {
            List<JsonCard> cardsList = GridCardsListToJsonCardsList();

            if (cardsList.Count > 0)
            {

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Pdf files (*.pdf)|*.pdf"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    FileInfo pdfFile = new FileInfo(saveFileDialog.FileName);


                    ExportParameters parameters = new ExportParameters(cardsList, GetSkinFile(this.cardsFile, true))
                    {
                        SpaceBetweenCards = 0,
                        exportFormat = Exporter.EXPORT_FORMAT_PDF,
                        exportMode = Exporter.EXPORT_MODE_BOARD,
                        WithBackSides = true,
                        TargetFile = pdfFile
                    };

                    Exporter.Export(this, parameters);
                    NotifyExportDone();
                }
            }
        }

        private void MenuItemExportAllToGameCrafter_Click(object sender, RoutedEventArgs e)
        {
            List<JsonCard> cardsList = GridCardsListToJsonCardsList();

            if (cardsList.Count > 0)
            {
                ExportParameters parameters = new ExportParameters(cardsList, GetSkinFile(cardsFile, true))
                {
                    exportFormat = Exporter.EXPORT_FORMAT_PNG,
                    exportMode = Exporter.EXPORT_GAME_CRAFTER,
                    WithBackSides = true,
                    TargetFolder = FolderDialog.SelectFolder()
                };

                Exporter.Export(this, parameters);
                NotifyExportDone();
            }

          
        }

        private void NotifyExportDone()
        {
            debug.Text = "Collection Exported";
        }

        private void SearchDataGrid(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if ((GridCardsList.Count > 0) && (e.Key==Key.Enter))
            {
                String filterText = ((TextBox)sender).Text;
                if (filterText.Equals(null) || filterText.Equals(""))
                {
                    Dispatcher.BeginInvoke(new Action(delegate ()
                    {
                        cardGrid.Items.Filter = null;
                        GridCount = cardGrid.Items.Count;
                    }));
                }
                else
                {
                    Dispatcher.BeginInvoke(new Action(delegate ()
                    {
                  
                        cardGrid.Items.Filter = (c) =>
                        {
                            return CardMatcher.Matches((Card)c, filterText);
                        };
                        GridCount = cardGrid.Items.Count;
                    }));
                   
                }
            }
        }

       

        private void UnfilterWarningList(object sender, RoutedEventArgs e)
        {
            ClearSearch();
        }

        private void FilterWarningList(object sender, RoutedEventArgs e)
        {

            Dispatcher.BeginInvoke(new Action(delegate ()
            {

                cardGrid.Items.Filter = (c) =>
                {
                    return CardMatcher.IsWarning((Card)c);
                };
                GridCount = cardGrid.Items.Count;
            }));
        }

        private void PreviewCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CardGrid_SelectionChanged(null, null);


        }

        private void CardGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                var notificationManager = new NotificationManager();

                notificationManager.Show(new NotificationContent
                {
                    Title = "Saving File",
                    
                    Type = NotificationType.Success
                },expirationTime:TimeSpan.FromSeconds(1));
                SaveProject(cardsFile);
                e.Handled = true;
            }
        }
    }
}
