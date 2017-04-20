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
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CardMasterStat
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

        private void LoadCards(List<Card> cards)
        {         
            CardComputer computer = new CardComputer(cards);  
            ((ColumnSeries)chartCost.Series[0]).ItemsSource = computer.GetRepartitionByCost();
            ((ColumnSeries)chartAtk.Series[0]).ItemsSource = computer.GetRepartitionByAttack();
            ((ColumnSeries)chartDef.Series[0]).ItemsSource = computer.GetRepartitionByDefense();
            ((ColumnSeries)chartNature.Series[0]).ItemsSource = computer.GetRepartitionByNature();
        }

       

        private void MenuItemClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItemXml_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*";
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


        private void MenuItemJson_Click(object sender, RoutedEventArgs e)
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
    }
}
