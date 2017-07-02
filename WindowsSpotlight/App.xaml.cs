using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WindowsSpotlight
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string MyPictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        public static void Alert(string message)
        {
            MessageBox.Show(message);
        }
    }
}
