using CardMasterCommon.Dialog;
using CardMasterImageClipping.Selection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CardMasterImageClipping
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum SelectionState
        {
            NoSelection,       // pas de sélection en cours
            SelectionStart,    // début de la sélection
            OnSelection,       // sélection en cours
            OnSelectionOut,    // sélection en cours mais souris hors de la zone image
            SelectionEnd       // sélection terminée (bouton de souris laché, la sélection est figée)
        }

        private const int TARGET_WIDTH = 628;
        private const int TARGET_HEIGHT = 445;
        private const int TARGET_RESOLUTION = 300;

        private SelectRectangle selRectangle = null;

        public ObservableCollection<CardImage> SourceImagesList { get; set; } = new ObservableCollection<CardImage>();

        private SelectionState selState = SelectionState.NoSelection;
        private CardImage selectedItem = null;
        private ImageSelection imgSelection = null;

        public MainWindow()
        {
            InitializeComponent();
            ImgTarget.Width = TARGET_WIDTH * ImgTarget.Height / TARGET_HEIGHT;
        }

        private void MenuItemOpenResourcesFolder_Click(object sender, RoutedEventArgs e)
        {
          
                string dir = FolderDialog.SelectFolder("D:\\workspace\\Workspace-perso\\CardsMaster\\data\\Resources\\Images");
                DirectoryInfo d = new DirectoryInfo(dir);
          
            if (d != null)
            {
                LoadImages(d);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void LoadImages(DirectoryInfo resourcesDirectory)
        {
            SourceImagesList.Clear();

            selState = SelectionState.NoSelection;
            CleanSelection();

            List<FileInfo> files = new List<FileInfo>();
            files.AddRange(resourcesDirectory.GetFiles("*.png"));
            files.AddRange(resourcesDirectory.GetFiles("*.jpg"));
            files.Sort((f1, f2) => f1.FullName.CompareTo(f2.FullName));

            foreach (FileInfo file in files)
            {
                SourceImagesList.Add(new CardImage(file));
            }

            DataContext = this;
        }

        private void LvwSourceImagesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count>0)
            {
                selectedItem = ((CardImage)(e.AddedItems[0]));
                imgSourceImage.Source = selectedItem.Image;
                imgSourceImage.UpdateLayout();

                this.selRectangle = new SelectRectangle(imgSourceImage, selectedItem.SourceImage, TARGET_WIDTH, TARGET_HEIGHT, TARGET_RESOLUTION);
                this.selRectangle.txt = sTxtSelInfos;

                selState = SelectionState.NoSelection;
                CleanSelection();
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (imgSourceImage.Source != null)
            {
                selState = SelectionState.SelectionStart;
                this.selRectangle.StartSelection(e.GetPosition(BlackPlane).X, e.GetPosition(BlackPlane).Y);
                DrawSelection(e.GetPosition(BlackPlane).X, e.GetPosition(BlackPlane).Y);
                selState = SelectionState.OnSelection;
            }

            TxtMouse.Text = "State=" + selState + "; X=" + e.GetPosition(BlackPlane).X + "; Y=" + e.GetPosition(BlackPlane).Y;
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if(selState == SelectionState.OnSelection)
            {
                DrawSelection(e.GetPosition(BlackPlane).X, e.GetPosition(BlackPlane).Y);
            }
            TxtMouse.Text = "State=" + selState + "; X=" + e.GetPosition(BlackPlane).X + "; Y=" + e.GetPosition(BlackPlane).Y;
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            selState = SelectionState.SelectionEnd;
            DrawSelection(e.GetPosition(BlackPlane).X, e.GetPosition(BlackPlane).Y);

            TxtMouse.Text = "State=" + selState + "; X=" + e.GetPosition(BlackPlane).X + "; Y=" + e.GetPosition(BlackPlane).Y;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            if (selState == SelectionState.OnSelection)
            {
                selState = SelectionState.OnSelectionOut;
            }
            TxtMouse.Text = "State=" + selState + "; X=" + e.GetPosition(BlackPlane).X + "; Y=" + e.GetPosition(BlackPlane).Y;
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            if (selState == SelectionState.OnSelectionOut)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    selState = SelectionState.OnSelection;
                }
                else
                {
                    selState = SelectionState.SelectionEnd;
                }

                DrawSelection(e.GetPosition(BlackPlane).X, e.GetPosition(BlackPlane).Y);
            }

            TxtMouse.Text = "State=" + selState + "; X=" + e.GetPosition(BlackPlane).X + "; Y=" + e.GetPosition(BlackPlane).Y;
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (selState == SelectionState.OnSelectionOut)
            {
                selState = SelectionState.SelectionEnd;
                DrawSelection(e.GetPosition(BlackPlane).X, e.GetPosition(BlackPlane).Y);
            }

            TxtMouse.Text = "State=" + selState + "; X=" + e.GetPosition(BlackPlane).X + "; Y=" + e.GetPosition(BlackPlane).Y;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (selState == SelectionState.OnSelectionOut)
            {
                DrawSelection(e.GetPosition(BlackPlane).X, e.GetPosition(BlackPlane).Y);
            }

            TxtMouse.Text = "State=" + selState + "; X=" + e.GetPosition(BlackPlane).X + "; Y=" + e.GetPosition(BlackPlane).Y;
        }


        private void DrawSelection(double mouseX, double mouseY)
        {
            bool canDrawSelection = false;
            bool canCleanSelection = false;

            imgSelection = null;

            if (selState == SelectionState.SelectionStart)
            {
                imgSelection = this.selRectangle.StartSelection(mouseX, mouseY);
                canDrawSelection = true;
            }
            else if ((selState == SelectionState.OnSelection) || (selState == SelectionState.OnSelectionOut))
            {
                imgSelection = this.selRectangle.GetSelection(mouseX, mouseY);
                canDrawSelection = true;
            }
            else if (selState == SelectionState.SelectionEnd)
            {
                imgSelection = this.selRectangle.GetSelection(mouseX, mouseY);
                canCleanSelection = ((imgSelection.SelRectangle.Width == 0) && (imgSelection.SelRectangle.Height == 0));
            }

            if (canDrawSelection)
            {
                MainSelRect.SetValue(Canvas.LeftProperty, (double)imgSelection.SelRectangle.X);
                MainSelRect.SetValue(Canvas.TopProperty, (double)imgSelection.SelRectangle.Y);
                MainSelRect.Width = imgSelection.SelRectangle.Width;
                MainSelRect.Height = imgSelection.SelRectangle.Height;

                LTCornerSelRect.SetValue(Canvas.LeftProperty, (double)(imgSelection.SelRectangle.X - 2));
                LTCornerSelRect.SetValue(Canvas.TopProperty, (double)(imgSelection.SelRectangle.Y - 2));

                RBCornerSelRect.SetValue(Canvas.LeftProperty, (double)(imgSelection.SelRectangle.Right - 2));
                RBCornerSelRect.SetValue(Canvas.TopProperty, (double)(imgSelection.SelRectangle.Bottom - 2));

                ShowSelection();
                ImgTarget.Source = imgSelection.SelBitmapImage;
            }
            else if (canCleanSelection)
            {
                CleanSelection();
            }

        }

        private void ShowSelection()
        {
            if (MainSelRect.Visibility != Visibility.Visible)
            {
                MainSelRect.Visibility = Visibility.Visible;
                LTCornerSelRect.Visibility = Visibility.Visible;
                RBCornerSelRect.Visibility = Visibility.Visible;
            }
        }

        private void CleanSelection()
        {
            if (MainSelRect.Visibility != Visibility.Collapsed)
            {
                MainSelRect.Visibility = Visibility.Collapsed;
                LTCornerSelRect.Visibility = Visibility.Collapsed;
                RBCornerSelRect.Visibility = Visibility.Collapsed;
                ImgTarget.Source = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((selectedItem != null) && (imgSelection != null) && (imgSelection.SelImage != null))
            {
                string filename = selectedItem.File.Name;
                string fileextension = selectedItem.File.Extension;
                string originalFilename = filename.Substring(0, filename.Length - fileextension.Length) + "-original" + fileextension;

                ImageFormat f = null;

                switch (fileextension.ToLower())
                {
                    case ".bmp":  f = ImageFormat.Bmp;  break;
                    case ".emf":  f = ImageFormat.Emf;  break;
                    case ".gif":  f = ImageFormat.Gif;  break;
                    case ".icn":  f = ImageFormat.Icon; break;
                    case ".jpg":  f = ImageFormat.Jpeg; break;
                    case ".jepg": f = ImageFormat.Jpeg; break;
                    case ".png":  f = ImageFormat.Png;  break;
                    case ".tiff": f = ImageFormat.Tiff; break;
                    case ".wmf":  f = ImageFormat.Wmf;  break;
                }

                if (f != null)
                {
                    selectedItem.File.CopyTo(Path.Combine(selectedItem.File.Directory.FullName, originalFilename), true);
                    this.imgSelection.SelImage.Save(selectedItem.File.FullName, f);
                }
            }

            
        }
    }
}
