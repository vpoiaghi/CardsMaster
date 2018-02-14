using System;
using System.Collections.Generic;
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

namespace CardMasterCommon
{
    /// <summary>
    /// Interaction logic for CustomTextBox.xaml
    /// </summary>
    public partial class CustomTextBox : UserControl
    {
        public CustomTextBox()
        {
            InitializeComponent();
        }

        string LocalLabel = null;
        string LocalTextBox = null;

        public string Label
        {
            get { return LocalLabel; }
            set
            {
                LocalLabel = value;
                BaseLabel.Content = value;
            }
        }

        public double TextBoxWidth
        {
            get { return BaseTextBox.Width; }
            set
            {
                BaseTextBox.Width = value;

            }
        }

        public string TextBox
        {
            get { return BaseTextBox.Text; }
            set
            {
                LocalTextBox = value;
                BaseTextBox.Text = value;
            }
        }
    }
}
