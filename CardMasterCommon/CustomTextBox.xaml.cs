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
        // events exposed to container
        public static readonly RoutedEvent OnLostFocusTextBoxEvent =
            EventManager.RegisterRoutedEvent("OnLostFocusTextBox", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(CustomTextBox));

        // expose and raise 'OnLostFocus' event
        public event RoutedEventHandler OnLostFocusTextBox
        {
            add { AddHandler(OnLostFocusTextBoxEvent, value); }
            remove { RemoveHandler(OnLostFocusTextBoxEvent, value); }
        }

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

        private void BaseTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OnLostFocusTextBoxEvent));
        }

        private void BaseTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            t.SelectAll();
        }
    }
}
