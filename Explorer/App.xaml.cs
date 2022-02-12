using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Explorer
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MainWindow MainWindow;

        static public string Directory { get; set; }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Directory = "";
            string [] args = e.Args;
            if (args != null && args.Length > 0 && System.IO.Directory.Exists(args[0]))
            {
                Directory = args[0];
            }
        }
    }
}
