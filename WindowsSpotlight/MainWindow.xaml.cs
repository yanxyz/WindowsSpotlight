using System;
using System.Collections.Generic;
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
using System.Diagnostics;
using Microsoft.Win32;

namespace WindowsSpotlight
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Img _img;
        private int _orientation = -1;
        private string _title = "Windows Spotlight";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _img = Utils.LoadData();
            }
            catch (Exception)
            {
                App.Alert("Could not load data");
                //throw;
                this.Close();
            }

            ToggleOrientation();
            _img.CreateDir();
        }

        private void SetImageSource(string imagePath)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(imagePath);
            image.EndInit();
            ImageBox.Source = image;
        }

        private bool PreferPortrait()
        {
            var w = SystemParameters.PrimaryScreenWidth;
            var h = SystemParameters.PrimaryScreenHeight;
            return w < h;
        }

        private void ToggleOrientation()
        {
            if (_orientation == -1)
                _orientation = PreferPortrait() ? 1 : 0;
            else if (_orientation == 0)
                _orientation = 1;
            else
                _orientation = 0;

            _img.Orientation = _orientation;
            this.Title = _title + " - " + _img.Info;
            OrientationMenu.Header = _orientation == 1 ? "Landscape Picture" : "Portrait Picture";
            SetImageSource(_img.Location);
        }

        private void SetBackground_Click(object sender, RoutedEventArgs e)
        {
            Utils.SetBackground(_img.Location);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                FileName = _img.Name,
                Filter = "Image file(*.jpg)|*.jpg",
                InitialDirectory = _img.SaveDirFav
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                System.IO.File.Copy(_img.Location, saveFileDialog.FileName);
            }
        }

        private void CopyAll_Click(object sender, RoutedEventArgs e)
        {
            Utils.CopyAll(_img);
        }

        private void Chrome_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Utils.OpenChrome(_img.Location);
            }
            catch (Exception ex)
            {
                App.Alert(ex.Message);
                //throw;
            }
        }

        private void Orientation_Click(object sender, RoutedEventArgs e)
        {
            ToggleOrientation();
        }

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(_img.SaveDir);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/yanxyz/WindowsSpotlight");
        }
    }
}
