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
            ((ColumnSeries)chartCost.Series[0]).ItemsSource = computer.GetRepartitionByCost();
            ((ColumnSeries)chartAtk.Series[0]).ItemsSource = computer.GetRepartitionByAttack();
            ((ColumnSeries)chartDef.Series[0]).ItemsSource = computer.GetRepartitionByDefense();
            ((ColumnSeries)chartNature.Series[0]).ItemsSource = computer.GetRepartitionByNature();
        }

   
    }
}
