using CardMasterCard.Card;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using System;

namespace CardMasterStat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string filePath;

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

        

            ((ScatterSeries)chartRatio.Series[0]).ItemsSource = computer.GetRepartitionByRatio();
            ((LineSeries)chartRatio.Series[1]).ItemsSource = computer.GetLowerLineRatio();
            ((LineSeries)chartRatio.Series[2]).ItemsSource = computer.GetHigherLineRatio();

        }
  

        private void MenuItemClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItemJson_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                LoadCardFile(filePath);
            }
        }

        private void LoadCardFile(string filePath)
        {
            JsonCardsProject cardProjet = JsonCardsProject.LoadProject(new FileInfo(filePath));
            List<Card> cards = new List<Card>();
            foreach (JsonCard card in cardProjet.Cards)
            {
                Card c = Card.ConvertCard(card);
                cards.Add(c);
            }
            LoadCards(cards);
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            LoadCardFile(filePath);
        }
    }
}
