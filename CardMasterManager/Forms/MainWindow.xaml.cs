﻿using CardMasterCard.Card;
using CardMasterExport;
using CardMasterExport.FileExport;
using CardMasterImageBuilder;
using CardMasterManager.Converters;
using CardMasterCommon.Converters;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
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
    public partial class MainWindow : Window
    {
        private bool onLoading;
        private static object locker = new object();

        private CardsProject cardProjet;
        private FileInfo cardsFile;
        private FileInfo skinsFile;
        private Card previousCard;

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
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                cardsFile = new FileInfo(openFileDialog.FileName);
                string skinsFileName = cardsFile.Name.Replace(cardsFile.Extension, ".skin");
                skinsFile = new FileInfo(Path.Combine(cardsFile.Directory.FullName, skinsFileName));

                cardProjet = CardsProject.LoadProject(cardsFile);
                
                List<Card> cards = new List<Card>();
                foreach (CardMasterCard.Card.Card card in cardProjet.Cards)
                {
                    Card c = Card.ConvertCard(card);
                    cards.Add(c);
                }

                LoadCards(cards);

                CmbSmoothingMode.SelectedItem = DQuality.SmoothingMode;
                CmbCompositingMode.SelectedItem = DQuality.CompositingMode;
                CmbCompositingQuality.SelectedItem = DQuality.CompositingQuality;
                CmbPixelOffsetMode.SelectedItem = DQuality.PixelOffsetMode;
                CmbTextRenderingHint.SelectedItem = DQuality.TextRenderingHint;
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
                FileInfo newSkinsFile = new FileInfo(Path.Combine(cardsFile.Directory.FullName, skinsFileName));

                SaveProject(newCardsFile);
                skinsFile.CopyTo(newSkinsFile.FullName);

                cardsFile = newCardsFile;
                skinsFile = newSkinsFile;
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
                //debug.Text = "ligne sélectionnée : " + c.Name;
                if (!(c == previousCard))
                {
                    previousCard = c;
                    new Thread(() => DisplayCard(c, cardImage)).Start();
                }
            }
        }

        private void DisplayCard(Card c, System.Windows.Controls.Image image)
        {
            //Select Card from Collection from Name
            CardMasterCard.Card.Card businessCard = Card.ConvertToMasterCard(c);
                      
            Drawer drawer = new Drawer(businessCard, skinsFile, null);
            drawer.Quality = this.DQuality;
            
            //Refresh Image Component
            DrawingImageToImageSourceConverter converter = new DrawingImageToImageSourceConverter();
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                image.Source = (ImageSource)converter.Convert(drawer.DrawCard(), null, null, System.Globalization.CultureInfo.CurrentCulture);
            }));
        }

        private void ComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            Card card = (Card)cb.DataContext;
            cardGrid.SelectedItem = card;
        }

        private void MenuItemExportAllToPngFile_Click(object sender, RoutedEventArgs e)
        {
            var cardsList = new List<CardMasterCard.Card.Card>();

            foreach (object item in cardGrid.Items)
            {
                cardsList.Add(Card.ConvertToMasterCard((Card)item));
            }
            
            if (cardsList.Count > 0)
            {
                PngExport exp = new PngExport(this, cardsList, skinsFile);
                PngExport.Parameters parameters = (PngExport.Parameters)exp.GetParameters();
                exp.progressChangedEvent += new PngExport.ProgressChanged(ExportProgressChanged);
                exp.Export(parameters);
            }

        }

        private void MenuItemExportBoardsToPngFile_Click(object sender, RoutedEventArgs e)
        {
            var cardsList = new List<CardMasterCard.Card.Card>();

            foreach (object item in cardGrid.Items)
            {
                cardsList.Add(Card.ConvertToMasterCard((Card)item));
            }

            if (cardsList.Count > 0)
            {
                PngBoardExport exp = new PngBoardExport(this, cardsList, skinsFile);
                PngBoardExport.Parameters parameters = (PngBoardExport.Parameters)exp.GetParameters();
                parameters.SpaceBetweenCards = 0;
                exp.progressChangedEvent += new PngBoardExport.ProgressChanged(ExportProgressChanged);
                exp.Export(parameters);
            }

        }

        private void ExportProgressChanged(object sender, ProgressChangedArg args)
        {
            debug.Text = args.Message;
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
                CardMasterCard.Card.Card c = null;

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
            }
        }

        private void CmbQuality_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DQuality.SmoothingMode = (SmoothingMode)CmbSmoothingMode.SelectedItem;
                DQuality.CompositingMode = (CompositingMode)CmbCompositingMode.SelectedItem;
                DQuality.CompositingQuality = (CompositingQuality)CmbCompositingQuality.SelectedItem;
                DQuality.PixelOffsetMode = (PixelOffsetMode)CmbPixelOffsetMode.SelectedItem;
                DQuality.TextRenderingHint = (TextRenderingHint)CmbTextRenderingHint.SelectedItem;

                Card c = ((Card)cardGrid.SelectedItem);
                if (c != null)
                {
                    new Thread(() => DisplayCard(c, cardImage)).Start();
                }
            }
            catch(NullReferenceException)
            { }
        }

    }
}
