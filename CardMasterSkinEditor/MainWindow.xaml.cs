
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
                BuildTemplate();
                RefreshImages();
               
            }
        }

        private void BuildTemplate()
        {
            cardTemplate = new JsonCard();
            cardTemplate.Attack = "4";
            cardTemplate.Cost = "6";
            cardTemplate.Defense = "3";
            cardTemplate.Citation = "texte citation";
            cardTemplate.Chakra = "Spécial";
            cardTemplate.Background = null;
            cardTemplate.Rank = "Rank";
            cardTemplate.Element = "Eau";
            cardTemplate.Kind = "Ninja";
            cardTemplate.Name = "Card Name";
            cardTemplate.Powers = new List<JsonPower>();
            cardTemplate.BackSkinName = "BackSkin1";
            cardTemplate.FrontSkinName = "FrontSkin1";
            cardTemplate.BackSide = "Back-Draw";
            cardTemplate.Team = "Equipe";
            cardTemplate.StringField1 = "<<Konoha>>Naruto Shippuden";
            JsonPower p1 = new JsonPower(); p1.Description = "<<Permanent>>  Ligne courte pouvoir 1";
            JsonPower p2 = new JsonPower(); p2.Description = "<<activate>>  Ceci est une ligne de pouvoir avec un texte assez long pour occuper plusieurs lignes";
            cardTemplate.Powers.Add(p1);
            cardTemplate.Powers.Add(p2);
        }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void RefreshImages()
        {
            Drawer drawer = new Drawer(cardTemplate, jsonSkinsProject, currentSkinDirectoryFullName, null);
            drawer.Quality = this.DQuality;

            //Refresh Image Component
            DrawingImageToImageSourceConverter converter = new DrawingImageToImageSourceConverter();
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                cardImage.Source = (ImageSource)converter.Convert(drawer.DrawCard(), null, null, System.Globalization.CultureInfo.CurrentCulture);
                backCardImage.Source = (ImageSource)converter.Convert(drawer.DrawBackCard(), null, null, System.Globalization.CultureInfo.CurrentCulture);
            }));
        }

        private void refresh(object sender, RoutedEventArgs e)
        {
            skinFile = new FileInfo(currentSkinFilePath);
            RefreshImages();
        }
    }
}
