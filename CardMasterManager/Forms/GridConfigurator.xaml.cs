using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CardMasterManager.Forms
{
    /// <summary>
    /// Interaction logic for GridConfigurator.xaml
    /// </summary>
    public partial class GridConfigurator : Window
    {

        private  class Configuration
        {
            public IDictionary<String, Visibility> fields { get; set; }
            public Configuration()
            {
                fields = new Dictionary<String, Visibility>();
            }
            public void AddConfiguration(String fieldName, Visibility visible)
            {
                fields.Add(fieldName, visible);
            }

        }

      

        public GridConfigurator(DataGrid cardGrid)
        {

            InitializeComponent();
            Configuration config = BuildConfiguration(cardGrid);
            DisplayConfiguration(cardGrid,config);
        }

        private static Configuration BuildConfiguration(DataGrid cardGrid)
        {
            Configuration configuration = new Configuration();
            foreach (DataGridColumn c in cardGrid.Columns)
            {
                configuration.AddConfiguration(c.Header.ToString(), c.Visibility);
            }
            return configuration;

        }
        public static void BuildAndSaveConfiguration(DataGrid cardGrid)
        {
            Configuration config = BuildConfiguration(cardGrid);
            SaveConfigurationToJson(config);
        }



        private void DisplayConfiguration(DataGrid cardGrid,Configuration config)
        {
            int index = 0;
            foreach(KeyValuePair<string, Visibility> entry in config.fields)
            {
                DockPanel stackLine = new DockPanel();

                Border b = new Border();
                b.CornerRadius = new CornerRadius(0);
                b.BorderThickness = new Thickness(1);
                b.BorderBrush = new SolidColorBrush(Colors.LightGray);

                TextBox textBox = new TextBox();
                textBox.Name = "Txt" + index ;
                textBox.Tag = entry.Key;
                textBox.Text = entry.Key;
                stackLine.Children.Add(textBox);

                CheckBox cb = new CheckBox();
                cb.Name = "Cb"+index;
                cb.IsChecked = entry.Value.Equals(Visibility.Visible) ? true : false;
                cb.HorizontalAlignment = HorizontalAlignment.Right;
                stackLine.Children.Add(cb);

                b.Child = stackLine;
                mainStack.Children.Add(b);
                index++;
            }

            Button cancelButton = new Button();
            cancelButton.Name = "CancelButton";
            cancelButton.Content = "Cancel";
            cancelButton.Width = 40;
            cancelButton.Click += (s, e) => { this.Close(); };
            buttonStack.Children.Add(cancelButton);

            Button okButton = new Button();
            okButton.Name = "OkButton";
            okButton.Content = "Save";
            okButton.HorizontalAlignment = HorizontalAlignment.Right;
            okButton.Width = 40;
            okButton.Click += (s, e) => {
                Configuration newConfig = BuildConfigFromWindows();
                ApplyConfiguration(cardGrid,newConfig);
                SaveConfigurationToJson(newConfig);
                this.Close();
            };
            buttonStack.Children.Add(okButton);

        }

        private static void ApplyConfiguration(DataGrid cardGrid,Configuration newConfig)
        {
            foreach (DataGridColumn c in cardGrid.Columns)
            {

                c.Visibility = newConfig.fields[c.Header.ToString()];
            }
            
        }

        private Configuration BuildConfigFromWindows()
        {
            Border border = null;
            DockPanel dockPanel;

            Configuration newConfiguration = new Configuration();

            foreach (UIElement mainStackChild in mainStack.Children)
            {
                if (mainStackChild is Border)
                {
                    border = (Border)mainStackChild;
                    dockPanel = (DockPanel)border.Child;

                    String fieldName = ((TextBox)dockPanel.Children[0]).Text;
                    Visibility visible = ((CheckBox)dockPanel.Children[1]).IsChecked.Value ? Visibility.Visible : Visibility.Hidden;
                    newConfiguration.AddConfiguration(fieldName, visible);
                }
            }

            return newConfiguration;
        }

        private static void SaveConfigurationToJson(Configuration newConfiguration)
        {
            String js = JsonConvert.SerializeObject(newConfiguration, Formatting.Indented);
            String dir = System.AppDomain.CurrentDomain.BaseDirectory;
            StreamWriter sw = new StreamWriter(dir+"/configuration.json", false);

            sw.Write(js);

            sw.Close();
            sw.Dispose();
            sw = null;
        }

        public static void LoadAndApplyConfiguration(DataGrid cardGrid)
        {
            String dir = System.AppDomain.CurrentDomain.BaseDirectory;
            if (File.Exists(dir + "/configuration.json"))
            {
                StreamReader sr = new StreamReader(dir + "/configuration.json");
                String js = sr.ReadToEnd();

                Configuration configuration = (Configuration)JsonConvert.DeserializeObject<Configuration>(js);

                sr.Close();
                sr.Dispose();
                sr = null;
                ApplyConfiguration(cardGrid, configuration);
            }
        }
    }
}
