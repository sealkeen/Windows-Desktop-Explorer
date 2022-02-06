using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;

namespace ExplorerLibrary
{
    public class DirectoryFiles
    {
        public ObservableCollection<FileIconInfo> FileIconInfos { get; set; }

        public void List()
        {
            DirectoryInfo dI = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
            FileIconInfos = new ObservableCollection<FileIconInfo>(
                FileIconInfo.ToFileIconInfos(dI.GetFiles())
                );
            foreach (FileIconInfo fi in FileIconInfos)
            {
                fi.Icon = (System.Drawing.Icon.ExtractAssociatedIcon(fi.FileInfo.FullName));
            }
        }
    }
}
