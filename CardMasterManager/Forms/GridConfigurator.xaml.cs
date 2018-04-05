﻿using CardMasterCard.Card;
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
        static Configuration configuration;
        private  class Configuration
        {
#pragma warning disable IDE1006 // Styles d'affectation de noms : impacte le save / load de la config
            public IDictionary<String, Visibility> fields { get; set; }
#pragma warning restore IDE1006 // Styles d'affectation de noms


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
            configuration = BuildConfiguration(cardGrid);
            DisplayConfiguration(cardGrid, configuration);
        }

        private static Configuration BuildConfiguration(DataGrid cardGrid)
        {
            configuration = new Configuration();
            foreach (DataGridColumn c in cardGrid.Columns)
            {
                configuration.AddConfiguration(c.Header.ToString(), c.Visibility);
            }
            return configuration;

        }
        public static void BuildAndSaveConfiguration(DataGrid cardGrid)
        {
             configuration = BuildConfiguration(cardGrid);
            SaveConfigurationToJson(configuration);
        }



        private void DisplayConfiguration(DataGrid cardGrid,Configuration config)
        {
            int index = 0;
            foreach(KeyValuePair<string, Visibility> entry in config.fields)
            {
                DockPanel stackLine = new DockPanel();

                Border b = new Border
                {
                    CornerRadius = new CornerRadius(0),
                    BorderThickness = new Thickness(1),
                    BorderBrush = new SolidColorBrush(Colors.LightGray)
                };

                TextBox textBox = new TextBox
                {
                    Name = "Txt" + index,
                    Tag = entry.Key,
                    Text = entry.Key,
                    Background = System.Windows.Media.Brushes.AliceBlue
                };
                stackLine.Children.Add(textBox);

                CheckBox cb = new CheckBox
                {
                    Name = "Cb" + index,
                    IsChecked = entry.Value.Equals(Visibility.Visible) ? true : false,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Background = System.Windows.Media.Brushes.AliceBlue
                };
                stackLine.Children.Add(cb);

                b.Child = stackLine;
                mainStack.Children.Add(b);
                index++;
            }

            Button cancelButton = new Button
            {
                Name = "CancelButton",
                Content = "Cancel",
                Width = 40
            };
            cancelButton.Click += (s, e) => { this.Close(); };
            buttonStack.Children.Add(cancelButton);

            Button okButton = new Button
            {
                Name = "OkButton",
                Content = "Save",
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = 40
            };
            okButton.Click += (s, e) => {
                configuration = BuildConfigFromWindows();
                ApplyConfiguration(cardGrid, configuration);
                SaveConfigurationToJson(configuration);
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

            configuration = new Configuration();

            foreach (UIElement mainStackChild in mainStack.Children)
            {
                if (mainStackChild is Border)
                {
                    border = (Border)mainStackChild;
                    dockPanel = (DockPanel)border.Child;

                    String fieldName = ((TextBox)dockPanel.Children[0]).Text;
                    Visibility visible = ((CheckBox)dockPanel.Children[1]).IsChecked.Value ? Visibility.Visible : Visibility.Hidden;
                    configuration.AddConfiguration(fieldName, visible);
                }
            }

            return configuration;
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

                configuration = (Configuration)JsonConvert.DeserializeObject<Configuration>(js);

                sr.Close();
                sr.Dispose();
                sr = null;
                ApplyConfiguration(cardGrid, configuration);
            }
        }
    }
}
