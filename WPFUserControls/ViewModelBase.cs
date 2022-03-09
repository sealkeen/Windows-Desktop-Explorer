using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace UserControls
{
    public class ViewModelBase
    {
        public ViewModelBase()
        {
            
        }

        public IEnumerable UpdateWith(string directory = "")
        {
            return DirectoryFiles.List(directory);
        }
        public ExplorerLibrary.DirectoryFiles DirectoryFiles = new ExplorerLibrary.DirectoryFiles();

        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand; //?? (_clickCommand = new CommandHandler(() => MyAction(), () => CanExecute));
            }
        }
        public bool CanExecute
        {
            get
            {
                // check if executing is allowed, i.e., validate, check if a process is running, etc. 
                return true;
            }
        }

        public void MyAction()
        {
            if (DirectoryFiles.CurrentFileIconInfo.FileInfo.Attributes.HasFlag(FileAttributes.Directory))
                DirectoryFiles.List(DirectoryFiles.CurrentFileIconInfo.FileInfo.FullName);
            else
                System.Diagnostics.Process.Start(DirectoryFiles.CurrentFileIconInfo.FileInfo.FullName);
        }
    }
}
