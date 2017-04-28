using CardMasterCard.Card;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                CardsProject cardProjet = CardsProject.LoadProject(new FileInfo(openFileDialog.FileName));
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
                String i = ((Card)cardGrid.SelectedItem).Name;
                debug.Text = "ligne sélectionnée : " + i;
                String url = Environment.CurrentDirectory + @"\Yamato.jpg";
                cardImage.Source = new BitmapImage(new Uri(url));
            }
           
        }
    }
}
