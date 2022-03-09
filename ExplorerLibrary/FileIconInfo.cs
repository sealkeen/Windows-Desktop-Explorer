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
using System.ComponentModel;

namespace ExplorerLibrary
{
    public class FileIconInfo : INotifyPropertyChanged
    {
        FileIconInfo()
        {
            ItemIndex = MaxItemIndex++;
            IconHeight = _iconHeight;
            IconWidth = _iconWidth;
        }

        public static UInt32 _iconHeight;
        public static UInt32 _iconWidth;
        public UInt32 IconHeight {get;set;} //{ get { return _iconHeight; } set { _iconHeight = value; OnPropertyChanged("IconHeight"); } }
        public UInt32 IconWidth {get;set;} //{ get { return _iconWidth; } set { _iconWidth = value; OnPropertyChanged("IconWidth"); } }
        public Icon Icon {get;set;}
        public FileSystemInfo FileInfo { get; set; }
        public ImageSource IconSource {get;set;}
        public int ItemIndex { get; set; }
        public static int MaxItemIndex = 0;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void OpenFile()
        {
            if (FileInfo.Attributes.HasFlag(FileAttributes.Directory))
                
            System.Diagnostics.Process.Start(FileInfo.FullName);
        }

        public static FileIconInfo[] ToFileIconInfos(FileSystemInfo[] infos)
        {
            FileIconInfo[] result = new FileIconInfo[infos.Length];
            try
            {
                for (int i = 0; i < infos.Length; i++)
                {
                    result[i] = new FileIconInfo();
                    result[i].FileInfo = infos[i];
                }
                var folderIcon = DefaultIcons.FolderLarge;
                var imageSource = Imaging.CreateBitmapSourceFromHIcon(
                        folderIcon.Handle,
                        Int32Rect.Empty,
                        System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                foreach (var fi in result)
                {
                    if (fi.FileInfo.Attributes.HasFlag(FileAttributes.Directory))
                    {
                        fi.Icon = folderIcon;
                        fi.IconSource = imageSource;
                        continue;
                    }
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
