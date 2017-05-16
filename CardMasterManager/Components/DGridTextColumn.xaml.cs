using System.Windows.Controls;
using System.Windows.Data;

namespace CardMasterManager.Components
{
    /// <summary>
    /// Interaction logic for DataGridTextColumn.xaml
    /// </summary>
    public partial class DGridTextColumn : DataGridTemplateColumn
    {
        private string binding = null;

        public string AttributeBinding { get { return binding; } set { binding = value; ApplyBinding(binding); } }

        public DGridTextColumn() : base()
        {
            InitializeComponent();
        }

        private void ApplyBinding(string binding)
        {
            //object o = this.CellTemplate.Template;
            //this.Binding = new System.Windows.Data.Binding(binding);

            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DataGrid_TextCellChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
