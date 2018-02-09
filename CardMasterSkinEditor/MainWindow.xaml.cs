
using CardMasterCard.Card;
using CardMasterImageBuilder;
using CardMasterManager.Converters;
using CardMasterSkin.Skins;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace CardMasterSkinEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        JsonCard cardTemplate;
        FileInfo skinFile;
        JsonSkinsProject jsonSkinsProject;
        String currentSkinFilePath;
        String currentSkinDirectoryFullName;
        public DrawingQuality DQuality { get; set; } = new DrawingQuality();
        DrawingImageToImageSourceConverter converter = new DrawingImageToImageSourceConverter();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Supported files|*.skin;|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                currentSkinFilePath = openFileDialog.FileName;
                skinFile = new FileInfo(currentSkinFilePath);
                currentSkinDirectoryFullName=skinFile.Directory.FullName;
                jsonSkinsProject = JsonSkinsProject.LoadProject(skinFile);
                cardTemplate = CardBuilder.BuildTemplate();
                RefreshImages();
                LoadUI();
            }
        }

        private void LoadUI()
        {
            borderWidthTextBox.Text = jsonSkinsProject.BorderWidth.Value.ToString();
            for (int i = 0; i < jsonSkinsProject.Skins.Count; i++)
            {
               
                JsonSkin currenSkin = jsonSkinsProject.Skins[i];
                ((TabItem)tabControl.Items.GetItemAt(i)).Header=currenSkin.Name;
                
                JsonSkin currentSkin = jsonSkinsProject.Skins.Where(s => s.Name.Equals(currenSkin.Name)).Single();
                FindTextBoxByName("skinHeightTextBox" +i).Text = currentSkin.Height.ToString();
                FindTextBoxByName("skinWidthTextBox" + i).Text = currentSkin.Width.ToString();
            }

          
        }


        private void UpdateJsonSkinProject()
        {
            jsonSkinsProject.BorderWidth = Int16.Parse(borderWidthTextBox.Text);
            for(int i =0; i < tabControl.Items.Count; i++)
            {
                TabItem currentItem = ((TabItem)tabControl.Items.GetItemAt(i));
                JsonSkin currentSkin = jsonSkinsProject.Skins.Where(s => s.Name.Equals(currentItem.Header)).Single();
                currentSkin.Height = GetTextBoxValueAsInt("skinHeightTextBox" + i);
                currentSkin.Width = GetTextBoxValueAsInt("skinWidthTextBox" + i);
            }

        }

        private int GetTextBoxValueAsInt(String textboxName)
        {
            return Int16.Parse(((TextBox)FindName(textboxName)).Text);
        }

        private TextBox FindTextBoxByName(string textboxName)
        {
            return ((TextBox)FindName(textboxName));
        }


        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Non implémenté");
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void RefreshImages()
        {

            Drawer drawer = new Drawer(cardTemplate, jsonSkinsProject, currentSkinDirectoryFullName, null);
            drawer.Quality = DQuality;

            //Refresh Image Component  
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                cardImage.Source = (ImageSource)converter.Convert(drawer.DrawCard(), null, null, System.Globalization.CultureInfo.CurrentCulture);
                backCardImage.Source = (ImageSource)converter.Convert(drawer.DrawBackCard(), null, null, System.Globalization.CultureInfo.CurrentCulture);
            }));
        }

        private void refresh(object sender, RoutedEventArgs e)
        {
            UpdateJsonSkinProject();
            RefreshImages();
        }

        private void reload(object sender, RoutedEventArgs e)
        {
            skinFile = new FileInfo(currentSkinFilePath);
            RefreshImages();
        }

       
    }
}
