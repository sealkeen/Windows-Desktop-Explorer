using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Threading.Tasks;
using Win32Interop;

namespace Explorer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string SettingsPath = Path.Combine(Environment.CurrentDirectory, "Settings");

        public MainWindow()
        {
            InitializeComponent();
            App.MainWindow = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //SendWpfWindowBack(this);
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //e.Cancel = true;
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
                //Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Settings"));
                //StreamReader sR = new StreamReader(SettingsPath);
                //sR.ReadLine();

                explorerGrid.Background = new ImageBrush(new BitmapImage(new Uri(@"C:\Users\Sealkeen\Pictures\Wallpapper\#Wallpapers #Shutdown.jpg")));
                btnStart.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory+"/start.jpg")));
            } catch (Exception ex) {
                
            }
        }

        private void explorerGrid_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            User32.SendWpfWindowBack(App.MainWindow);
        }
    }
}
