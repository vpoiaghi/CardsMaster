using CardMasterCommon.Dialog;
using CardMasterManager.Converters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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
        private const int TARGET_ROSULUTION = 300;

        private SelectRectangle selRectangle = null;

        public ObservableCollection<CardImage> SourceImagesList { get; set; } = new ObservableCollection<CardImage>();

        private SelectionState selState = SelectionState.NoSelection;
        private double offsetX = 0;
        private double offsetY = 0;
        private double screenImageWidth = 0;
        private double screenImageHeight = 0;

        private double sourceResolutionX = 0;
        private double sourceResolutionY = 0;


        private CardImage selectedItem = null;

        public MainWindow()
        {
            InitializeComponent();
            ImgTarget.Width = TARGET_WIDTH * ImgTarget.Height / TARGET_HEIGHT;
        }

        private void MenuItemOpenResourcesFolder_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo d = new DirectoryInfo("F:/Programmation/VB .Net/Cartes Bruno/CardsMaster/data/Resources/Images");

            if (!d.Exists)
            {
                d = FolderDialog.SelectFolder();
            }
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
            selectedItem = ((CardImage)(e.AddedItems[0]));
            imgSourceImage.Source = selectedItem.Image;
            imgSourceImage.UpdateLayout();

            screenImageWidth = selectedItem.SourceImage.Width * imgSourceImage.ActualHeight / selectedItem.SourceImage.Height;
            screenImageHeight = imgSourceImage.ActualHeight;

            UIElement parent = imgSourceImage.Parent as UIElement;
            System.Windows.Point location = imgSourceImage.TranslatePoint(new System.Windows.Point(0, 0), parent);
            this.offsetX = location.X;
            this.offsetY = location.Y;

            this.sourceResolutionX = ((BitmapImage)imgSourceImage.Source).DpiX;
            this.sourceResolutionY = ((BitmapImage)imgSourceImage.Source).DpiY;

            this.selRectangle = new SelectRectangle(imgSourceImage.ActualWidth, imgSourceImage.ActualHeight, TARGET_WIDTH, TARGET_HEIGHT);

            selState = SelectionState.NoSelection;
            CleanSelection();
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
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if(selState == SelectionState.OnSelection)
            {
                DrawSelection(e.GetPosition(BlackPlane).X, e.GetPosition(BlackPlane).Y);
            }
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            selState = SelectionState.SelectionEnd;
            DrawSelection(e.GetPosition(BlackPlane).X, e.GetPosition(BlackPlane).Y);
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            if (selState == SelectionState.OnSelection)
            {
                selState = SelectionState.OnSelectionOut;
            }
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
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (selState == SelectionState.OnSelectionOut)
            {
                selState = SelectionState.SelectionEnd;
                DrawSelection(e.GetPosition(BlackPlane).X, e.GetPosition(BlackPlane).Y);
            }
        }

        private void DrawSelection(double mouseX, double mouseY)
        {
            bool canDrawSelection = false;
            bool canCleanSelection = false;
            Rectangle selRectangle = Rectangle.Empty;

            if (selState == SelectionState.SelectionStart)
            {
                selRectangle = new Rectangle((int)mouseX, (int)mouseY, 0, 0);
                canDrawSelection = true;
            }
            else if ((selState == SelectionState.OnSelection) || (selState == SelectionState.OnSelectionOut))
            {
                selRectangle = this.selRectangle.GetSelection(mouseX, mouseY);
                canDrawSelection = true;
            }
            else if (selState == SelectionState.SelectionEnd)
            {
                selRectangle = this.selRectangle.GetSelection(mouseX, mouseY);
                canCleanSelection = ((selRectangle.Width == 0) && (selRectangle.Height == 0));
            }

            if (canDrawSelection)
            {
                MainSelRect.SetValue(Canvas.LeftProperty, (double)selRectangle.X);
                MainSelRect.SetValue(Canvas.TopProperty, (double)selRectangle.Y);
                MainSelRect.Width = selRectangle.Width;
                MainSelRect.Height = selRectangle.Height;

                LTCornerSelRect.SetValue(Canvas.LeftProperty, (double)(selRectangle.X - 2));
                LTCornerSelRect.SetValue(Canvas.TopProperty, (double)(selRectangle.Y - 2));

                RBCornerSelRect.SetValue(Canvas.LeftProperty, (double)(selRectangle.Right - 2));
                RBCornerSelRect.SetValue(Canvas.TopProperty, (double)(selRectangle.Bottom - 2));

                ShowSelection();
                ImgTarget.Source = GetImagePart(selRectangle);
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
        
        private BitmapImage GetImagePart(System.Drawing.Rectangle r)
        {
            BitmapImage result = null;

            if (r.Width > 0 && r.Height > 0)
            {
                System.Drawing.Bitmap srcImage = (Bitmap)selectedItem.SourceImage;

                int x = (int)((r.X - offsetX) * srcImage.Width / screenImageWidth);
                int y = (int)((r.Y - offsetY) * srcImage.Height / screenImageHeight);
                int w = (int)(r.Width * srcImage.Width / screenImageWidth);
                int h = (int)(r.Height * srcImage.Height / screenImageHeight);

                w = (int)(w * TARGET_ROSULUTION / sourceResolutionX);
                h = (int)(h * TARGET_ROSULUTION / sourceResolutionY);
                
                if (w > 0 && h > 0)
                {
                    if (x + w > selectedItem.Image.Width)
                    {
                        w = (int)selectedItem.Image.Width - x;
                        h = w * TARGET_HEIGHT / TARGET_WIDTH;
                    }

                    if (y + h > selectedItem.Image.Height)
                    {
                        h = (int)selectedItem.Image.Height - y;
                        w = h * TARGET_WIDTH / TARGET_HEIGHT;
                    }

                    System.Drawing.Bitmap targetImage = new Bitmap(w, h);
                    targetImage.SetResolution(300, 300);

                    Graphics g = Graphics.FromImage(targetImage);
                    g.Clear(System.Drawing.Color.Black);

                    g.DrawImage(srcImage.Clone(new System.Drawing.Rectangle(x, y, w, h), srcImage.PixelFormat), 0, 0, TARGET_WIDTH, TARGET_HEIGHT);

                    g.Dispose();
                    g = null;

                    DrawingImageToImageSourceConverter conv = new DrawingImageToImageSourceConverter();

                    result = (BitmapImage)conv.Convert(targetImage, null, null, null);
                }
            }

            return result;
        }
    }
}
