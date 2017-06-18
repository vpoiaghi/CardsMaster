using CardMasterCommon.Dialog;
using CardMasterManager.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CardMasterImageClipping
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int TARGET_WIDTH = 628;
        private const int TARGET_HEIGHT = 445;

        public ObservableCollection<SmallImage> SourceImagesList { get; set; } = new ObservableCollection<SmallImage>();

        private int _isDragging = 0;
        private System.Windows.Point _anchorPoint = new System.Windows.Point();
        private int offsetX = 0;
        private int offsetY = 0;
        private double oldX = 0;
        private double oldY = 0;
        private double screenImageWidth = 0;
        private double screenImageHeight = 0;

        private SmallImage selectedItem = null;

        public MainWindow()
        {
            InitializeComponent();

            ImgTarget.Width = ImgTarget.Height * TARGET_WIDTH / TARGET_HEIGHT;
        }

        private void MenuItemOpenResourcesFolder_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo d = FolderDialog.SelectFolder();
            //DirectoryInfo d = new DirectoryInfo("F:/Programmation/VB .Net/Cartes Bruno/CardsMaster/data/Resources/Images");
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

            List<FileInfo> files = new List<FileInfo>();
            files.AddRange(resourcesDirectory.GetFiles("*.png"));
            files.AddRange(resourcesDirectory.GetFiles("*.jpg"));
            files.Sort((f1, f2) => f1.FullName.CompareTo(f2.FullName));

            foreach (FileInfo file in files)
            {
                SourceImagesList.Add(new SmallImage(file));
            }

            DataContext = this;
        }

        private void LvwSourceImagesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedItem = ((SmallImage)(e.AddedItems[0]));
            imgSourceImage.Source = selectedItem.Image;
            imgSourceImage.UpdateLayout();

            screenImageWidth = selectedItem.SourceImage.Width * imgSourceImage.ActualHeight / selectedItem.SourceImage.Height;
            screenImageHeight = imgSourceImage.ActualHeight;

            MainSelRect.Visibility = Visibility.Collapsed;
            LTCornerSelRect.Visibility = Visibility.Collapsed;
            RBCornerSelRect.Visibility = Visibility.Collapsed;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (imgSourceImage.Source != null)
            {
                _anchorPoint.X = e.GetPosition(BlackPlane).X;
                _anchorPoint.Y = e.GetPosition(BlackPlane).Y;

                oldX = _anchorPoint.X;
                oldY = _anchorPoint.Y;

                LTCornerSelRect.SetValue(Canvas.LeftProperty, _anchorPoint.X - 2);
                LTCornerSelRect.SetValue(Canvas.TopProperty, _anchorPoint.Y - 2);

                _isDragging = 1;
            }
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if(_isDragging == 1)
            {
                double x = e.GetPosition(BlackPlane).X;
                double y = e.GetPosition(BlackPlane).Y;

                double w = Math.Abs(x - _anchorPoint.X);
                double h = h = w * TARGET_HEIGHT / TARGET_WIDTH;

                y = y < _anchorPoint.Y ? _anchorPoint.Y - h : _anchorPoint.Y + h;

                double leftX = Math.Min(x, _anchorPoint.X);
                double topY = Math.Min(y, _anchorPoint.Y);

                if (leftX < 0 || topY < 0 || ((leftX + w) > imgSourceImage.ActualWidth) || ((topY + h) > imgSourceImage.ActualHeight))
                {
                    x = oldX;
                    y = oldY;

                    w = Math.Abs(x - _anchorPoint.X);
                    h = h = w * TARGET_HEIGHT / TARGET_WIDTH;

                    y = y < _anchorPoint.Y ? _anchorPoint.Y - h : _anchorPoint.Y + h;

                    leftX = Math.Min(x, _anchorPoint.X);
                    topY = Math.Min(y, _anchorPoint.Y);
                }
                else
                {
                    oldX = e.GetPosition(BlackPlane).X;
                    oldY = e.GetPosition(BlackPlane).Y;
                }

                MainSelRect.SetValue(Canvas.LeftProperty, leftX);
                MainSelRect.SetValue(Canvas.TopProperty, topY);
                MainSelRect.Width = w;
                MainSelRect.Height = h;

                RBCornerSelRect.SetValue(Canvas.LeftProperty, x - 2);
                RBCornerSelRect.SetValue(Canvas.TopProperty, y - 2);

                if (MainSelRect.Visibility != Visibility.Visible)
                {
                    MainSelRect.Visibility = Visibility.Visible;
                    LTCornerSelRect.Visibility = Visibility.Visible;
                    RBCornerSelRect.Visibility = Visibility.Visible;
                }

                System.Drawing.Rectangle r = new System.Drawing.Rectangle((int)leftX, (int)topY, (int)w, (int)h);
                r.Offset(offsetX, offsetY);

                ImgTarget.Source = GetImagePart(r);
            }
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = 0;

            double x = e.GetPosition(BlackPlane).X;
            double y = e.GetPosition(BlackPlane).Y;

            if ((_anchorPoint.X == x) && (_anchorPoint.Y == y))
            {
                MainSelRect.Visibility = Visibility.Collapsed;
                LTCornerSelRect.Visibility = Visibility.Collapsed;
                RBCornerSelRect.Visibility = Visibility.Collapsed;
            }
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            _isDragging = 2;
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (_isDragging == 2)
                {
                    _isDragging = 1;
                }
            }
            else
            {
                _isDragging = 0;
            }
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = 0;
        }

        private BitmapImage GetImagePart(System.Drawing.Rectangle r)
        {
            BitmapImage result = null;

            if (r.Width > 0 && r.Height > 0)
            {
                System.Drawing.Bitmap srcImage = (Bitmap)selectedItem.SourceImage;

                int x = (int)((r.X - 0) * srcImage.Width / screenImageWidth);
                int y = (int)((r.Y - ((imgSourceImage.ActualHeight - screenImageHeight) / 2)) * srcImage.Height / screenImageHeight);
                int w = (int)(r.Width * srcImage.Width / screenImageWidth);
                int h = (int)(r.Height * srcImage.Height / screenImageHeight);

                if (w > 0 && h > 0)
                {
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
