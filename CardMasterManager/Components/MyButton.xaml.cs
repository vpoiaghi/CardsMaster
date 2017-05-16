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

namespace CardMasterManager.Components
{
    /// <summary>
    /// Interaction logic for MyButton.xaml
    /// </summary>
    public partial class MyButton : Button
    {
        public MyButton() : base()
        {
            InitializeComponent();

            this.AddText("essai");
        }

        public static readonly DependencyProperty SetTitiProperty =
            DependencyProperty.Register("SetTiti", typeof(string), typeof(MyButton), new
                PropertyMetadata("", new PropertyChangedCallback(OnSetTextChanged)));

        public string SetToto
        {
            get { return (string)GetValue(SetTitiProperty); }
            set { SetValue(SetTitiProperty, value); }
        }

        private static void OnSetTextChanged(DependencyObject d,   DependencyPropertyChangedEventArgs e)
        {
            MyButton mb = d as MyButton;
            mb.OnSetTextChanged(e);
        }

        private void OnSetTextChanged(DependencyPropertyChangedEventArgs e)
        {
            //tbTest.Text = e.NewValue.ToString();
        }
    }
}
