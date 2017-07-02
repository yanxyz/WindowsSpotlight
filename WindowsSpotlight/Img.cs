using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsSpotlight
{
    public class Img
    {
        public string Dir { get; }
        public string[] Landscape { get; }
        public string[] Portrait { get; }
        public int PortraitWidth { get; }

        public int Orientation = 0;

        public string Name
        {
            get => Orientation == 0 ? Landscape[0] : Portrait[0];
        }

        public string Location
        {
            get => Path.Combine(Dir, Name);
        }

        public string Info
        {
            get
            {
                var shortName = Name.Substring(0, 7);
                var dimensions = Orientation == 0 ? Landscape[1] + "x" + Landscape[2] : Portrait[1] + "x" + Portrait[2];
                return shortName + " " + dimensions;
             }
        }

        public string SaveDir
        {
            get => Path.Combine(App.MyPictures, "WindowsSpotlight");
        }

        public string SaveDirFav
        {
            get => Path.Combine(SaveDir, "Fav");
        }

        public string SaveDirTemp
        {
            get => Path.Combine(SaveDir, "Temp");
        }

        public Img(string dir, string[] landscape, string[] portrait)
        {
            Dir = dir;
            Landscape = landscape;
            Portrait = portrait;
            if (int.TryParse(portrait[1], out int w) == false) w = 1080;
            PortraitWidth = w;
        }

        public void CreateDir()
        {
            try
            {
                Directory.CreateDirectory(SaveDirFav);
                Directory.CreateDirectory(SaveDirTemp);
            }
            catch (Exception)
            {
                App.Alert("Create SaveDir failed");
                //throw;
            }
        }
    }
}
