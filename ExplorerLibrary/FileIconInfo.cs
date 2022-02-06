using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Interop;

namespace ExplorerLibrary
{
    public class FileIconInfo
    {
        public Icon Icon {get;set;}
        public FileInfo FileInfo {get;set;}
        public ImageSource IconSource {get;set;}

        public static FileIconInfo[] ToFileIconInfos(FileInfo[] infos)
        {
            FileIconInfo[] result = new FileIconInfo[infos.Length];
            try
            {
                for (int i = 0; i < infos.Length; i++)
                {
                    result[i] = new FileIconInfo();
                    result[i].FileInfo = infos[i];
                }
                foreach (var fi in result)
                {
                    fi.Icon = (System.Drawing.Icon.ExtractAssociatedIcon(fi.FileInfo.FullName));
                    fi.IconSource = Imaging.CreateBitmapSourceFromHIcon(
                        fi.Icon.Handle, 
                        Int32Rect.Empty, 
                        System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                }
            }
            catch { }
            return result;
        }

    }
}
