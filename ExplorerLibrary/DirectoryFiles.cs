using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using ObjectModelExtensions;

namespace ExplorerLibrary
{
    public class DirectoryFiles
    {
        public DirectoryFiles(string directory = "")
        {
            if (Directory.Exists(directory))
                DirectoryInfo = new DirectoryInfo(directory);
            FileIconInfos = new ConcurrentObservableCollection<FileIconInfo>();
        }

        public ConcurrentObservableCollection<FileIconInfo> FileIconInfos { get; set; }
        public FileIconInfo CurrentFileIconInfo { get { return FileIconInfos[SelectedIndex]; } }
        public DirectoryInfo DirectoryInfo { get; set; }

        public int SelectedIndex { get; set; }
        public ObservableCollection<FileIconInfo> List(string directory = "")
        {
            FileIconInfo.MaxItemIndex = 0;
            if (Directory.Exists(directory))
                DirectoryInfo = new DirectoryInfo(directory);
            else
                DirectoryInfo = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));

            try {
                    FileIconInfos.Clear();
                    FileIconInfos.AddRange(FileIconInfo.ToFileIconInfos(DirectoryInfo.GetFileSystemInfos()));
            } catch {  }
            return FileIconInfos;
        }
    }
}
