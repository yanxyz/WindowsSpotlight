using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Diagnostics;
using fastJSON;
using System.IO;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;

namespace WindowsSpotlight
{
    public class Utils
    {
        public static Img LoadData()
        {
            var json = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Lock Screen\Creative", "CreativeJson", String.Empty);
            var data = JSON.ToDynamic(json.ToString());
            var assetFolderRootPath = data.cdm.assetFolderRootPath;
            var imagePairIndex = data.cdm.imagePairIndex;
            var landscape = data.assets[$"image_fullscreen_{imagePairIndex}_landscape"];
            var portrait = data.assets[$"image_fullscreen_{imagePairIndex}_portrait"];

            return new Img(assetFolderRootPath, 
                new string[] { landscape.u, landscape.w, landscape.h }, 
                new string[] { portrait.u, portrait.w, portrait.h });
        }

        public static void CopyAll(Img img)
        {
            int bytes = 200 * 1024;

            foreach (string fileName in Directory.EnumerateFiles(img.Dir))
            {
                var fileInfo = new FileInfo(fileName);
                if (fileInfo.Length < bytes) continue;

                using (var imageStream = File.OpenRead(fileName))
                {
                    var decoder = BitmapDecoder.Create(imageStream, BitmapCreateOptions.IgnoreColorProfile, BitmapCacheOption.Default);
                    var w = decoder.Frames[0].PixelWidth;
                    if (w >= img.PortraitWidth)
                    {
                        var destFileName = Path.Combine(img.SaveDirTemp, fileInfo.Name + ".jpg");
                        if (File.Exists(destFileName)) continue;
                        try
                        {
                            File.Copy(fileName, destFileName);
                        }
                        catch (Exception)
                        {
                            //throw;
                        }
                    }
                }
            }
        }

        public static void OpenChrome(string fileName)
        {
            var startInfo = new ProcessStartInfo
            {
                // Path or App Paths
                FileName = "chrome.exe",
                Arguments = fileName
            };
            Process.Start(startInfo);
        }

        internal sealed class NativeMethods
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            internal static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        }

        public static void SetBackground(string picturePath)
        {
            var wallpaperPath = Path.Combine(App.MyPictures, "SpotlightWallpaper.jpg");
            File.Copy(picturePath, wallpaperPath, true);

            const int SetDesktopBackground = 20;
            const int UpdateIniFile = 1;
            const int SendWindowsIniChange = 2;
            NativeMethods.SystemParametersInfo(SetDesktopBackground, 0, wallpaperPath, UpdateIniFile | SendWindowsIniChange);
        }
    }
}
