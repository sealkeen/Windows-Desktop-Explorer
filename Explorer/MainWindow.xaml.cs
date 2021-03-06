using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xaml;
using System.Xml;
using System.Xml.Linq;
using Win32Interop;
using WPFUserControls;
using System.Windows.Input;
using UserControls;
using ExplorerLibrary;
using ObjectModelExtensions;

namespace Explorer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string SettingsFolderPath = Path.Combine(Environment.CurrentDirectory, "Settings");
        private static string SettingsFilePath = Path.Combine(SettingsFolderPath, "settings.xml");
        private static string _dateTime;
        private static bool _closed = false;
        public static ProcessesViewModel ProcessesViewModel { get; set; }
        public MainWindow()
        {
            FileIconInfo._iconHeight = 40;
            FileIconInfo._iconWidth = 40;
            InitializeComponent();
            App.MainWindow = this;
            ProcessesViewModel = new ProcessesViewModel();
            this.lstProcesses.DataContext = ProcessesViewModel.DiagnosticsProgram;
            
            Thread timeThread = new Thread(UpdateTime);
            timeThread.Start();

            ucFiles.UpdateViewModelDataContext(App.Directory);
            //Task.Factory.StartNew(() => 
            ucFiles.GetBindingExpression(UserControls.FileStackPanel.ItemsSourceProperty).UpdateTarget();
            //);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //SendWpfWindowBack(this);
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _closed = true;
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            User32.SendWpfWindowBack(this);
            //var tsk = Task.Factory.StartNew( delegate {
                User32.SendWpfWindowBack(this);
                ReadSettings();
            //});
        }

        private void ReadSettings()
        {
            try
            {
                if (File.Exists(Environment.CurrentDirectory + "/start.jpg"))
                    btnStart.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "/start.jpg")));

                if (File.Exists(SettingsFilePath))
                {
                    //File.SetAttributes(SettingsFilePath, FileAttributes.Normal);
                    FileStream sR = new FileStream(SettingsFilePath, FileMode.OpenOrCreate);
                    string imageName = SettingsReader.ReadLastElement(sR);
                    explorerGrid.Background = new ImageBrush(new BitmapImage(new Uri(imageName)));
                    sR.Close();
                }
            } catch (Exception ex) {
                
            }
        }

        private void WriteSettings()
        {
            try {
                //StreamWriter sR = new StreamWriter(SettingsFolderPath);
                //XElement backGroundImage = new XElement("image");
                //backGroundImage.Value = new XText();
            }
            catch (Exception ex) { 
                
            }
        }

        private void explorerGrid_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            explorerGrid_ShowWindows();
        }

        private void explorerGrid_ShowWindows()
        {
            User32.SendWpfWindowBack(App.MainWindow);
            ProcessesViewModel.DiagnosticsProgram.ListProcesses();
            lstProcesses.GetBindingExpression(System.Windows.Controls.ListView.ItemsSourceProperty).UpdateTarget();
        }

        private void btnChangeBackground_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fileData = Plugin.FilePicker.CrossFilePicker.Current.PickFile();
                SetBackground(fileData.Result.FilePath);
                if (!Directory.Exists(SettingsFolderPath))
                    Directory.CreateDirectory(SettingsFolderPath);
                if(!File.Exists(SettingsFilePath))
                    File.Create(SettingsFilePath);

                XDocument xD; XElement root;
                var elements = SettingsReader.GetElements(SettingsFilePath);
                if (elements.Count() == 0)
                {
                    root = new XElement("BackgroundImages");
                    xD = new XDocument();
                    xD.Add(root);
                } else {
                    xD = XDocument.Load(SettingsFilePath);
                    root = xD.Elements().First();
                }
                XElement backGroundImage = new XElement("image");
                backGroundImage.Value = fileData.Result.FilePath;
                root.Add(backGroundImage);
                xD.Save(SettingsFilePath);
                
                //sR.WriteLine(backGroundImage);
                //sR.Close();
            } catch (Exception ex) {
                
            }
        }

        private void btnCloseDesktop_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SetBackground(string fileName)
        {
            try
            {
                explorerGrid.Background = new ImageBrush(
                    new BitmapImage(
                    new Uri(fileName)));
            } catch (Exception ex) { 

            }
        }

        private void btnOpenExplorer_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\Windows\SysWOW64\explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
        }

        private void btnMinimizeAll_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew( delegate {
                User32.CascadeWindows();
                Thread.Sleep(75);
                explorerGrid_ShowWindows();
                }
            );
            //WindowDiagnostics.SystemProcesses.
            //WindowDiagnostics.SystemProcesses.MinimizeAll()

        }        
        
        private void btnOpenCMD_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("cmd.exe");
        }

        private void lstProcesses_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                var proc = (lstProcesses.SelectedItem as System.Diagnostics.Process).MainWindowHandle;
                
                Win32Interop.User32.SwitchToThisWindow(proc);
            }
            catch { }
        }

        private void lstProcesses_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //User32.SendWpfWindowBack(App.MainWindow);
            ProcessesViewModel.DiagnosticsProgram.ListProcesses();
            lstProcesses.GetBindingExpression(System.Windows.Controls.ListView.ItemsSourceProperty).UpdateTarget();
        }

        private void btnOpenTaskManager_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("taskmgr");
        }

        private void btnShutdown_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("shutdown", "/s /f /t 0"); 
            //Process.Start("shutdown", "/r /t 0");  // shutdown and restart
            //Process.Start("shutdown", "/h /f");    // hibernate
        }

        private void btnFullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (App.Current.MainWindow.WindowState != WindowState.Maximized)
                App.Current.MainWindow.WindowState = WindowState.Maximized;
            else
                App.Current.MainWindow.WindowState = WindowState.Normal;
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("shutdown", "/r /t 0");  // shutdown and restart
        }

        private void btnHibernate_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("shutdown", "/h /f");    // hibernate
        }

        public void UpdateTime()
        {
            while(true && !_closed)
            {
                _dateTime = DateTime.Now.ToShortDateString() + Environment.NewLine + DateTime.Now.ToShortTimeString();
                this.Dispatcher.BeginInvoke(new Action(delegate
                {
                    lblDateTime.Text = _dateTime;
                }));
                Thread.Sleep(500);
            }
        }

        private void btnLogoff_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("shutdown.exe", "-l");
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            User32.IncreaseForegroundWindow();
        }

        private void StackPanel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                BrushConverter bc = new BrushConverter();
                (sender as StackPanel).Background = (Brush)bc.ConvertFrom("#00808F80");
            }
            catch { }
        }

        private void StackPanel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                BrushConverter bc = new BrushConverter();
                (sender as StackPanel).Background = (Brush)bc.ConvertFrom("#11808F80");
            }
            catch { }
        }

        private void btnIconButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender.GetType();
        }

        private void icFiles_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }

        private void fileStackPanel_Loaded(object sender, EventArgs e)
        {

        }

        private void btnToggleTransparency_Click(object sender, RoutedEventArgs e)
        {
            explorerGrid.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

            XDocument xD; XElement root;
            var elements = SettingsReader.GetElements(SettingsFilePath);
            if (elements.Count() == 0)
            {
                root = new XElement("BackgroundImages");
                xD = new XDocument();
                xD.Add(root);
            }
            else
            {
                xD = XDocument.Load(SettingsFilePath);
                root = xD.Elements().First();
            }

            var trnsp = root.Elements().Where(elm => elm.Name == "Transparent");
            if (trnsp.Count() != 0)
            {
                if (trnsp.First().Value == "true")
                    trnsp.First().Value = "false";
                else
                    trnsp.First().Value = "true";
            }
            else
            {
                root.Add(new XElement("Transparent", "true"));
            }
            xD.Save(SettingsFilePath);
        }

        private void btnDesktopBackground_Click_Click(object sender, RoutedEventArgs e)
        {
            var fileData = Plugin.FilePicker.CrossFilePicker.Current.PickFile();
            if (fileData.Result != null)
                if(File.Exists(fileData.Result.FilePath))
                    Wallpaper.Set(new Uri(fileData.Result.FilePath), Wallpaper.Style.Tiled);

        }

        private void icFiles_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
                return;

            if (e.Delta > 0)
                ZoomIn();

            else if (e.Delta < 0)
                ZoomOut();
        }

        private void ZoomIn()
        {
            EnumerableExtensions.ForEach<FileIconInfo>(FileStackPanel.ViewModel.DirectoryFiles.FileIconInfos, ZoomIn);
            ucFiles.OnItemsSourceChanged(FileStackPanel.ViewModel.DirectoryFiles.FileIconInfos,
                FileStackPanel.ViewModel.DirectoryFiles.FileIconInfos);
            ucFiles.GetBindingExpression(UserControls.FileStackPanel.ItemsSourceProperty).UpdateTarget();
        }

        private void ZoomIn(FileIconInfo fi)
        {
            FileIconInfo._iconHeight += 1;
            FileIconInfo._iconWidth += 1;
        }

        private void ZoomOut()
        {
            EnumerableExtensions.ForEach<FileIconInfo>(FileStackPanel.ViewModel.DirectoryFiles.FileIconInfos, ZoomOut);
            ucFiles.OnItemsSourceChanged(FileStackPanel.ViewModel.DirectoryFiles.FileIconInfos, 
                FileStackPanel.ViewModel.DirectoryFiles.FileIconInfos);
            ucFiles.GetBindingExpression(UserControls.FileStackPanel.ItemsSourceProperty).UpdateTarget();
        }

        private void ZoomOut(FileIconInfo fi)
        {
            FileIconInfo._iconHeight -= 1;
            FileIconInfo._iconWidth -= 1;
        }
    }
}
