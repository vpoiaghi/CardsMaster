using CardMasterCard.Card;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using System;
using WpfCharts;
using System.Windows.Media;

namespace CardMasterStat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Random random = new Random(1234);
        string filePath;
        public string[] Axes { get; set; }
        public ObservableCollection<ChartLine> Lines { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;

        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            DataContext = this;

            Axes = new[] { "Item 1", "Item 2", "Item 3", "Item 4", "Item 5", "Item 6", "Item 7" };

            Lines = new ObservableCollection<ChartLine> {
                                                            new ChartLine {
                                                                              LineColor = Colors.Red,
                                                                              FillColor = Color.FromArgb(128, 255, 0, 0),
                                                                              LineThickness = 2,
                                                                              PointDataSource = GenerateRandomDataSet(Axes.Length),
                                                                              Name = "Chart 1"
                                                                          },
                                                            new ChartLine {
                                                                              LineColor = Colors.Blue,
                                                                              FillColor = Color.FromArgb(128, 0, 0, 255),
                                                                              LineThickness = 2,
                                                                              PointDataSource = GenerateRandomDataSet(Axes.Length),
                                                                              Name = "Chart 2"
                                                                          }
                                                        };
        }
        public List<double> GenerateRandomDataSet(int nmbrOfPoints)
        {
            var pts = new List<double>(nmbrOfPoints);
            for (var i = 0; i < nmbrOfPoints; i++)
            {
                pts.Add(random.NextDouble());
            }
            return pts;
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
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Json files (*.json)|*.json|All files (*.*)|*.*"
            };
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
            statusLabel.Text = "Status : Cards Loaded";
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            LoadCardFile(filePath);
        }

        private void MouseDoubleClock(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }
    }
}
