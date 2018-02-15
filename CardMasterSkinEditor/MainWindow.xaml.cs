
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
        int currentSkinItemIndex = -1;
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
                currentSkinDirectoryFullName = skinFile.Directory.FullName;
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
                ((TabItem)tabControl.Items.GetItemAt(i)).Header = currenSkin.Name;

                JsonSkin currentSkin = jsonSkinsProject.Skins.Where(s => s.Name.Equals(currenSkin.Name)).Single();
                FindCustomTextBoxByName("skinHeightTextBox" + i).TextBox = currentSkin.Height.ToString();
                FindCustomTextBoxByName("skinWidthTextBox" + i).TextBox = currentSkin.Width.ToString();

                //Load ListView of skin items
                for (int j = 0; j < currentSkin.Items.Count; j++)
                {
                    FindListViewByName("skinItemListView" + i).Items.Add(currentSkin.Items[j].Comment);
                }
                skinItemListView0.SelectedIndex = 0;
            }


        }

        private int? GetCustomTextBoxValueAsInt(String textboxName)
        {
            String value = FindCustomTextBoxByName(textboxName).TextBox;
            if (value == null || value.Equals(""))
            {
                return null;
            }
            return int.Parse(value);
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
            if (previewCheckBox.IsChecked.Value == true && jsonSkinsProject!=null)
            {
                Drawer drawer = new Drawer(cardTemplate, jsonSkinsProject, currentSkinDirectoryFullName, null);
                drawer.Quality = DQuality;

                //Refresh Image Component  
                Dispatcher.BeginInvoke(new Action(delegate ()
                {
                    cardImage.Source = (ImageSource)converter.Convert(drawer.DrawCard(), null, null, System.Globalization.CultureInfo.CurrentCulture);
                    backCardImage.Source = (ImageSource)converter.Convert(drawer.DrawBackCard(), null, null, System.Globalization.CultureInfo.CurrentCulture);
                }));
            }else
            {
                cardImage.Source =null;
                backCardImage.Source = null;
            }
        }

        private void reload(object sender, RoutedEventArgs e)
        {
            skinFile = new FileInfo(currentSkinFilePath);
            jsonSkinsProject = JsonSkinsProject.LoadProject(skinFile);
            RefreshImages();
            LoadUI();
        }

       

        private void loadViewFromSkinItem(ListView currentListView)
        {

            currentSkinItemIndex = currentListView.SelectedIndex;

            String currentComment = (String)currentListView.Items.GetItemAt(currentSkinItemIndex);
            int tabIndex = tabControl.SelectedIndex;
            String currentSkinName = ((TabItem)tabControl.SelectedItem).Header.ToString();
            JsonSkinItem currentSkinItem = jsonSkinsProject.Skins.Where(n => n.Name.Equals(currentSkinName)).Single().Items.Where(c => c.Comment.Equals(currentComment)).Single();

            FindComboBoxByName("typeComboBox" + tabIndex).SelectedValue = currentSkinItem.Type;
            FindCustomTextBoxByName("customPositionX" + tabIndex).TextBox = currentSkinItem.X.ToString();
            FindCustomTextBoxByName("customPositionY" + tabIndex).TextBox = currentSkinItem.Y.ToString();

            FindCustomTextBoxByName("customWidth" + tabIndex).TextBox = currentSkinItem.Width.ToString();
            FindCustomTextBoxByName("customHeight" + tabIndex).TextBox = currentSkinItem.Height.ToString();
            FindCustomTextBoxByName("customRadius" + tabIndex).TextBox = currentSkinItem.Radius.HasValue ? currentSkinItem.Radius.Value.ToString() : null;

            FindCustomTextBoxByName("customFirstBackgrounColor" + tabIndex).TextBox = currentSkinItem.BackgroundColor;
            FindCustomTextBoxByName("customSecondBackgrounColor" + tabIndex).TextBox = currentSkinItem.BackgroundColor2;

            FindCustomTextBoxByName("customExternalBorderWidth" + tabIndex).TextBox = currentSkinItem.ExternalCardBorderthickness.HasValue ? currentSkinItem.ExternalCardBorderthickness.Value.ToString() : null;
            FindCustomTextBoxByName("customNameAttribute" + tabIndex).TextBox = currentSkinItem.NameAttribute;
            FindCustomTextBoxByName("customBackground" + tabIndex).TextBox = currentSkinItem.Background;
            FindCustomTextBoxByName("customCurveSize" + tabIndex).TextBox = currentSkinItem.CurveSize.HasValue ? currentSkinItem.CurveSize.Value.ToString() : null;

            FindCustomTextBoxByName("customBorderColor" + tabIndex).TextBox = currentSkinItem.BorderColor;
            FindCustomTextBoxByName("customBorderWidth" + tabIndex).TextBox = currentSkinItem.BorderWidth.HasValue ? currentSkinItem.BorderWidth.Value.ToString() : null;

            FindCustomTextBoxByName("customFontName" + tabIndex).TextBox = currentSkinItem.FontName;
            FindCustomTextBoxByName("customFontColor" + tabIndex).TextBox = currentSkinItem.FontColor;
            FindCustomTextBoxByName("customStyle" + tabIndex).TextBox = currentSkinItem.Style;
            FindCustomTextBoxByName("customFontSize" + tabIndex).TextBox = currentSkinItem.FontSize.HasValue ? currentSkinItem.FontSize.Value.ToString() : null;
            FindCustomTextBoxByName("customWithFontBorder" + tabIndex).TextBox = currentSkinItem.WithFontBorder.HasValue ? currentSkinItem.WithFontBorder.Value.ToString() : null; ;

            FindCustomTextBoxByName("customVerticaAlign" + tabIndex).TextBox = currentSkinItem.VerticalAlign;
            FindCustomTextBoxByName("customHorizontalAlign" + tabIndex).TextBox = currentSkinItem.HorizontalAlign;

            FindCustomTextBoxByName("customVisibleConditionAttribute" + tabIndex).TextBox = currentSkinItem.VisibleConditionAttribute;

            FindCustomTextBoxByName("customShadowSize" + tabIndex).TextBox = currentSkinItem.shadowSize.HasValue ? currentSkinItem.shadowSize.Value.ToString() : null; ;
            FindCustomTextBoxByName("customShadowAngle" + tabIndex).TextBox = currentSkinItem.shadowAngle.HasValue ? currentSkinItem.shadowAngle.Value.ToString() : null; ;

            FindCustomTextBoxByName("customPowerIconWidth" + tabIndex).TextBox = currentSkinItem.PowerIconWidth.HasValue ? currentSkinItem.PowerIconWidth.Value.ToString() : null; ;
            FindCustomTextBoxByName("customPowerIconHeight" + tabIndex).TextBox = currentSkinItem.PowerIconHeight.HasValue ? currentSkinItem.PowerIconHeight.Value.ToString() : null; ;

        }

        private void updateSkinItem(ListView currentListView)
        {
            if (currentSkinItemIndex != -1)
            {
                String currentComment = (String)currentListView.Items.GetItemAt(currentSkinItemIndex);
                int tabIndex = tabControl.SelectedIndex;
                String currentSkinName = ((TabItem)tabControl.SelectedItem).Header.ToString();
                JsonSkinItem currentSkinItem = jsonSkinsProject.Skins.Where(n => n.Name.Equals(currentSkinName)).Single().Items.Where(c => c.Comment.Equals(currentComment)).Single();

                jsonSkinsProject.BorderWidth = Int16.Parse(borderWidthTextBox.TextBox);

                currentSkinItem.X = GetCustomTextBoxValueAsInt("customPositionX" + tabIndex).Value;
                currentSkinItem.Y = GetCustomTextBoxValueAsInt("customPositionY" + tabIndex).Value;
                currentSkinItem.Width = GetCustomTextBoxValueAsInt("customWidth" + tabIndex).Value;
                currentSkinItem.Height = GetCustomTextBoxValueAsInt("customHeight" + tabIndex).Value;

                switch (currentSkinItem.Type)
                {
                    case "SERoundedRectangle":
                        currentSkinItem.Radius = GetCustomTextBoxValueAsInt("customRadius" + tabIndex);
                        currentSkinItem.BackgroundColor = FindCustomTextBoxByName("customFirstBackgrounColor" + tabIndex).TextBox;
                        currentSkinItem.BackgroundColor2 = FindCustomTextBoxByName("customSecondBackgrounColor" + tabIndex).TextBox;
                        currentSkinItem.ExternalCardBorderthickness = GetCustomTextBoxValueAsInt("customExternalBorderWidth" + tabIndex);
                        currentSkinItem.Background = FindCustomTextBoxByName("customBackground" + tabIndex).TextBox;
                        break;
                    case "SEImage":
                        currentSkinItem.Radius = GetCustomTextBoxValueAsInt("customRadius" + tabIndex);
                        currentSkinItem.NameAttribute = FindCustomTextBoxByName("customNameAttribute" + tabIndex).TextBox;
                        currentSkinItem.Background = FindCustomTextBoxByName("customBackground" + tabIndex).TextBox;
                        currentSkinItem.VisibleConditionAttribute = FindCustomTextBoxByName("customVisibleConditionAttribute" + tabIndex).TextBox;
                        currentSkinItem.BorderColor = FindCustomTextBoxByName("customBorderColor" + tabIndex).TextBox;
                        break;
                    case "SECurvedRectangle":
                        currentSkinItem.CurveSize = GetCustomTextBoxValueAsInt("customCurveSize" + tabIndex);
                        currentSkinItem.Background = FindCustomTextBoxByName("customBackground" + tabIndex).TextBox;
                        currentSkinItem.BorderColor = FindCustomTextBoxByName("customBorderColor" + tabIndex).TextBox;
                        break;
                    case "SERectangle":
                        currentSkinItem.Background = FindCustomTextBoxByName("customBackground" + tabIndex).TextBox;
                        currentSkinItem.BorderColor = FindCustomTextBoxByName("customBorderColor" + tabIndex).TextBox;
                        break;
                    case "SETextArea":
                        currentSkinItem.FontName = FindCustomTextBoxByName("customFontName" + tabIndex).TextBox;
                        currentSkinItem.FontSize = GetCustomTextBoxValueAsInt("customFontSize" + tabIndex);
                        currentSkinItem.FontColor = FindCustomTextBoxByName("customFontColor" + tabIndex).TextBox;
                        currentSkinItem.Style = FindCustomTextBoxByName("customStyle" + tabIndex).TextBox;
                        currentSkinItem.WithFontBorder = bool.Parse(FindCustomTextBoxByName("customWithFontBorder" + tabIndex).TextBox);

                        currentSkinItem.NameAttribute = FindCustomTextBoxByName("customNameAttribute" + tabIndex).TextBox;

                        currentSkinItem.VerticalAlign = FindCustomTextBoxByName("customVerticaAlign" + tabIndex).TextBox;
                        currentSkinItem.HorizontalAlign = FindCustomTextBoxByName("customHorizontalAlign" + tabIndex).TextBox;

                        currentSkinItem.VisibleConditionAttribute = FindCustomTextBoxByName("customVisibleConditionAttribute" + tabIndex).TextBox;

                        currentSkinItem.PowerIconHeight = GetCustomTextBoxValueAsInt("customPowerIconHeight" + tabIndex);
                        currentSkinItem.PowerIconWidth = GetCustomTextBoxValueAsInt("customPowerIconWidth" + tabIndex);

                        break;
                    default:
                        break;
                }
            }

        }

        private void ManageKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                RefreshImages();
            }
        }

        private void SaveItem0(object sender, RoutedEventArgs e)
        {
            updateSkinItem(skinItemListView0);
            RefreshImages();
        }

        private void SaveItem1(object sender, RoutedEventArgs e)
        {
            updateSkinItem(skinItemListView1);
            RefreshImages();
        }


        private void loadSkinItemAndDisplay1(object sender, SelectionChangedEventArgs e)
        {
            loadViewFromSkinItem(skinItemListView1);
        }

        private void loadSkinItemAndDisplay0(object sender, SelectionChangedEventArgs e)
        {
            loadViewFromSkinItem(skinItemListView0);
        }

        private void OnLostFocusTextBox0(object sender, RoutedEventArgs e)
        {
            updateSkinItem(skinItemListView0);
            RefreshImages();
        }
    }
}
