﻿using CardMasterCard.Card;
using CardMasterImageBuilder;
using CardMasterManager.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CardMasterManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CardsProject cardProjet;
        public MainWindow()
        {
            InitializeComponent();
            cardImage.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Yamato.jpg"));
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                cardProjet = CardsProject.LoadProject(new FileInfo(openFileDialog.FileName));
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
            cardGrid.ItemsSource = cards;
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
                debug.Text = "ligne sélectionnée : " + c.Name;

                //Select Card from Collection from Name
                CardMasterCard.Card.Card businessCard = cardProjet.Cards.Where(card => card.Name == c.Name).First();
                //Refresh Image Component
                cardImage.Source = imageSourceForImageControl(new Drawer(businessCard, new DirectoryInfo(cardProjet.TexturesDirectory)).DrawCard());
            }
           
        }

        private ImageSource imageSourceForImageControl(System.Drawing.Image image)
        {
            DrawingImageToImageSourceConverter c = new DrawingImageToImageSourceConverter();
            return (ImageSource)c.Convert(image, null,null, System.Globalization.CultureInfo.CurrentCulture);
        }

    }
}
