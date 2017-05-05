using CardMasterCard.Card;
using CardMasterImageBuilder;
using CardMasterManager.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
        private CardsProject cardProjet;
        private FileInfo skinsFile;
        private Card previousCard;
        public MainWindow()
        {
            InitializeComponent();
            //cardImage.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Yamato.jpg"));
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo cardsFile = new FileInfo(openFileDialog.FileName);
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
            }
        }

        private void LoadCards(List<Card> cards)
        {
            //cardGrid.ItemsSource = cards;

            cardGrid.Items.Clear();

            foreach ( Card card in cards )
            {
                cardGrid.Items.Add(card);
            }

        }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemSaveAs_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemClose_Click(object sender, RoutedEventArgs e)
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

    }
}
