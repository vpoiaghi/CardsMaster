
using CardMasterCard.Card;
using CardMasterCommon;
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

            //Load Type combo
            for (int j = 0; j < tabControl.Items.Count; j++)
            {
                FindComboBoxByName("typeComboBox" + j).Items.Add("SERoundedRectangle");
                FindComboBoxByName("typeComboBox" + j).Items.Add("SEImage");
                FindComboBoxByName("typeComboBox" + j).Items.Add("SETextArea");
                FindComboBoxByName("typeComboBox" + j).Items.Add("SECurvedRectangle");
                FindComboBoxByName("typeComboBox" + j).Items.Add("SERectangle");
                FindComboBoxByName("typeComboBox" + j).SelectedIndex = 0;
            }



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
            borderWidthTextBox.TextBox = jsonSkinsProject.BorderWidth.Value.ToString();
            for (int i = 0; i < jsonSkinsProject.Skins.Count; i++)
            {
               
                JsonSkin currenSkin = jsonSkinsProject.Skins[i];
                ((TabItem)tabControl.Items.GetItemAt(i)).Header=currenSkin.Name;
                
                JsonSkin currentSkin = jsonSkinsProject.Skins.Where(s => s.Name.Equals(currenSkin.Name)).Single();
                FindCustomTextBoxByName("skinHeightTextBox" +i).TextBox = currentSkin.Height.ToString();
                FindCustomTextBoxByName("skinWidthTextBox" + i).TextBox = currentSkin.Width.ToString();

                //Load ListView of skin items
                for (int j = 0; j < currentSkin.Items.Count; j++)
                {
                    FindListViewByName("skinItemListView" + i).Items.Add(currentSkin.Items[j].Comment);
                }
            }

          
        }


        private void UpdateJsonSkinProject()
        {
            jsonSkinsProject.BorderWidth = Int16.Parse(borderWidthTextBox.TextBox);
            for(int i =0; i < tabControl.Items.Count; i++)
            {
                TabItem currentItem = ((TabItem)tabControl.Items.GetItemAt(i));
                JsonSkin currentSkin = jsonSkinsProject.Skins.Where(s => s.Name.Equals(currentItem.Header)).Single();
                currentSkin.Height = GetCustomTextBoxValueAsInt("skinHeightTextBox" + i);
                currentSkin.Width = GetCustomTextBoxValueAsInt("skinWidthTextBox" + i);

             
            }

        }

        private int GetCustomTextBoxValueAsInt(String textboxName)
        {
            return Int16.Parse(FindCustomTextBoxByName(textboxName).TextBox);
        }

        private ListView FindListViewByName(String name)
        {
            return ((ListView)FindName(name));
        }

        private ComboBox FindComboBoxByName(String name)
        {
            return ((ComboBox)FindName(name));
        }

        private CustomTextBox FindCustomTextBoxByName(String name)
        {
            return ((CustomTextBox)FindName(name));
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
            if (previewCheckBox.IsChecked.Value == true)
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

        private void saveSkinItemAndDisplay(object sender, SelectionChangedEventArgs e)
        {
            ListView currentListView = ((ListView)sender);
            int currentIndex = currentListView.SelectedIndex;
            String currentComment = (String)currentListView.Items.GetItemAt(currentIndex);
            int tabIndex = tabControl.SelectedIndex;
            String currentSkinName = ((TabItem)tabControl.SelectedItem).Header.ToString();

            JsonSkinItem currentSkinItem = jsonSkinsProject.Skins.Where(n => n.Name.Equals(currentSkinName)).Single().Items.Where(c=>c.Comment.Equals(currentComment)).Single();

            FindComboBoxByName("typeComboBox" + tabIndex).SelectedValue = currentSkinItem.Type;
            FindCustomTextBoxByName("customPositionX"+ tabIndex).TextBox = currentSkinItem.X.ToString();
            FindCustomTextBoxByName("customPositionY"+tabIndex).TextBox = currentSkinItem.Y.ToString();
            FindCustomTextBoxByName("customWidth" + tabIndex).TextBox = currentSkinItem.Width.ToString();
            FindCustomTextBoxByName("customHeight" + tabIndex).TextBox = currentSkinItem.Height.ToString();
            FindCustomTextBoxByName("customRadius" + tabIndex).TextBox = currentSkinItem.Radius.HasValue ? currentSkinItem.Radius.Value.ToString() : null ;
            FindCustomTextBoxByName("customFirstBackgrounColor"+ tabIndex).TextBox = currentSkinItem.BackgroundColor;
            FindCustomTextBoxByName("customSecondBackgrounColor" + tabIndex).TextBox = currentSkinItem.BackgroundColor2;
            FindCustomTextBoxByName("customExternalBorderWidth" + tabIndex).TextBox = currentSkinItem.ExternalCardBorderthickness.HasValue ? currentSkinItem.ExternalCardBorderthickness.Value.ToString():null;

            

        }
    }
}
