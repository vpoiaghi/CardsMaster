using CardMasterCard.Card;
using System;
using System.Collections.Generic;
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
            LoadCards();
           
        }

        private void LoadCards()
        {
            List<Card> cards = MockCards.GetSamples();
            CardComputer computer = new CardComputer(cards);
            SortedDictionary<int, int> repartitionCost = computer.GetRepartitionByCost();
            StringBuilder sb = new StringBuilder();
            sb.Append("Répartition par coût").Append("\n");
            foreach(KeyValuePair<int,int> pair in repartitionCost)
            {
                sb.Append(pair.Key).Append(" : ").Append(pair.Value).Append(" éléments. \r\n");
            }
            sb.Append("\n").Append("Nombre total de cartes : " + computer.GetTotalCount());
            richTextBox.AppendText(sb.ToString());
            LoadLineChartData(repartitionCost);
        }

        private void LoadLineChartData(SortedDictionary<int, int>  repartitionCost)
        {
            ((LineSeries)mcChart.Series[0]).ItemsSource = repartitionCost;
            
        }

    }
}
