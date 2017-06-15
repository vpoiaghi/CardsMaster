﻿using CardMasterCard.Card;
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
        private bool onLoading;
        private static object locker = new object();

        private JsonCardsProject cardProjet;
        private FileInfo cardsFile;
        private Card previousCard;

        private DateTime d1;
        private DateTime d2;
        private bool isSearching = false;

        public DrawingQuality DQuality { get; set; } = new DrawingQuality();

        public MainWindow()
        {
            InitializeComponent();

            this.onLoading = false;

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.MenuItemSave.IsEnabled = false;
            this.MenuItemSaveAs.IsEnabled = false;
            this.MenuItemExportAllToPngFile.IsEnabled = false;
            this.MenuItemExportBoardsToPngFile.IsEnabled = false;
            this.MenuItemPrintBoards.IsEnabled = false;
            GridConfigurator.LoadAndApplyConfiguration(cardGrid);
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                cardsFile = new FileInfo(openFileDialog.FileName);
                cardProjet = JsonCardsProject.LoadProject(cardsFile);
                
                List<Card> cards = new List<Card>();
                foreach (JsonCard card in cardProjet.Cards)
                {
                    Card c = Card.ConvertCard(card);
                    cards.Add(c);
                }

                LoadCards(cards);

               
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

        private void MenuItemSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                FileInfo newCardsFile = new FileInfo(saveFileDialog.FileName);

                string skinsFileName = cardsFile.Name.Replace(newCardsFile.Extension, ".skin");
                FileInfo oldSkinsFile = new FileInfo(Path.Combine(cardsFile.Directory.FullName, skinsFileName));
                FileInfo newSkinsFile = new FileInfo(Path.Combine(newCardsFile.Directory.FullName, skinsFileName));

                SaveProject(newCardsFile);
                oldSkinsFile.CopyTo(newSkinsFile.FullName);

                DirectoryInfo oldResourceDir = new DirectoryInfo(Path.Combine(cardsFile.Directory.FullName, "Resources"));
                DirectoryInfo newResourceDir = new DirectoryInfo(Path.Combine(newCardsFile.Directory.FullName, "Resources"));

                DirectoryUtils.Copy(oldResourceDir, newResourceDir);

                this.cardsFile = newCardsFile;
            }
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
 

        private void cardGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cardGrid.SelectedItem != null)
            {
                Card c = ((Card)cardGrid.SelectedItem);

                if (!(c == previousCard))
                {
                    previousCard = c;
                    new Thread(() => DisplayCard(c, cardImage,backCardImage)).Start();
                }
            }
        }

        private void DisplayCard(Card c, System.Windows.Controls.Image frontImage, System.Windows.Controls.Image backImage)
        {
            //Select Card from Collection from Name
            JsonCard businessCard = Card.ConvertToMasterCard(c);
            FileInfo skinFile = GetSkinFile();
            if (skinFile != null)
            {
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
        }

        private void ComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            Card card = (Card)cb.DataContext;
            cardGrid.SelectedItem = card;
        }

        private void MenuItemExportAllToPngFile_Click(object sender, RoutedEventArgs e)
        {
            var cardsList = new List<JsonCard>();

            foreach (object item in cardGrid.Items)
            {
                cardsList.Add(Card.ConvertToMasterCard((Card)item));
            }
            
            if (cardsList.Count > 0)
            {
                ExportParameters parameters = new ExportParameters(cardsList, GetSkinFile());
                parameters.exportFormat = Exporter.EXPORT_FORMAT_PNG;
                parameters.exportMode = Exporter.EXPORT_MODE_ALL;
                parameters.TargetFolder = FolderDialog.SelectFolder();

                Exporter.Export(this, parameters);
            }

        }

        private void MenuItemExportBoardsToPngFile_Click(object sender, RoutedEventArgs e)
        {
            var cardsList = new List<JsonCard>();

            foreach (object item in cardGrid.Items)
            {
                cardsList.Add(Card.ConvertToMasterCard((Card)item));
            }

            if (cardsList.Count > 0)
            {
                ExportParameters parameters = new ExportParameters(cardsList, GetSkinFile());
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
            this.onLoading = true;

            cardGrid.Items.Clear();

            foreach (Card card in cards)
            {
                cardGrid.Items.Add(card);
            }

            this.onLoading = false;
        }

        private void SaveProject(FileInfo cardsFile)
        {
            if ((cardProjet != null) && (cardsFile != null))
            {
                JsonCard c = null;

                List<Card> cards = GetCardsFromGrid();

                cardProjet.Cards.Clear();

                foreach (Card card in cards)
                {
                    c = Card.ConvertToMasterCard(card);
                    cardProjet.Cards.Add(c);
                }

                cardProjet.Save(cardsFile);

                MenuItemSave.IsEnabled = false;
                MenuItemSave.IsEnabled = false;
            }
        }

        private List<Card> GetCardsFromGrid()
        {
            var cards = new List<Card>();

            foreach (Card card in cardGrid.Items)
            {
                cards.Add(card);
            }

            return cards;
        }

        private void DataGrid_TextCellChanged(object sender, TextChangedEventArgs e)
        {
            DataGridValuesChanged();
        }

        private void DataGrid_ComboCellChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGridValuesChanged();
        }

        private void DataGridValuesChanged()
        {
            if (!this.onLoading)
            {
                this.MenuItemSave.IsEnabled = true;
                this.MenuItemSaveAs.IsEnabled = true;
                this.MenuItemExportAllToPngFile.IsEnabled = true;
                this.MenuItemExportBoardsToPngFile.IsEnabled = true;
                this.MenuItemPrintBoards.IsEnabled = true;
            }
        }

       

        private void MenuItemPrintBoards_Click(object sender, RoutedEventArgs e)
        {
            var cardsList = new List<JsonCard>();

            foreach (object item in cardGrid.Items)
            {
                cardsList.Add(Card.ConvertToMasterCard((Card)item));
            }

            if (cardsList.Count > 0)
            {
                ExportParameters parameters = new ExportParameters(cardsList, GetSkinFile());

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

        private FileInfo GetSkinFile()
        {   if (this.cardsFile == null) return null;
            FileInfo skinFile = new FileInfo(Path.Combine(this.cardsFile.Directory.FullName, this.cardsFile.Name.Replace(this.cardsFile.Extension, ".skin")));
            return skinFile.Exists ? skinFile : null;
        }

        private void ClearSearch(object sender, RoutedEventArgs e)
        {
            searchText.Text = "";
        }

        private void filterDataGrid(object sender, TextChangedEventArgs e)
        {
            if (!isSearching)
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
            GridConfigurator.BuildAndSaveConfiguration(cardGrid);
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
            if (cardGrid.Items.Count == 0 )
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
            cardGrid.Items.Insert(indexWhereToInsert, newCard);
            cardGrid.SelectedItem = cardGrid.Items.GetItemAt(indexWhereToInsert);
            cardGrid.Focus();

        }  

        private void DeleteRowClick(object sender, RoutedEventArgs e)
        {
            if (cardGrid.SelectedIndex != -1)
            {
                int selectedIndex = cardGrid.SelectedIndex;
                cardGrid.Items.RemoveAt(selectedIndex);
                if (cardGrid.Items.Count > 0)
                {
                    cardGrid.SelectedItem = cardGrid.Items.GetItemAt(selectedIndex == 0 ? 0 : selectedIndex - 1);
                    cardGrid.Focus();
                }
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
            Card c = ((Card)cardGrid.SelectedItem);
            cardGrid.Items.RemoveAt(sourceIndex);
            cardGrid.Items.Insert(targetIndex, c);
            cardGrid.SelectedIndex = targetIndex;
            cardGrid.SelectedItem = cardGrid.Items.GetItemAt(targetIndex);
            cardGrid.Focus();
        }
    }
}
