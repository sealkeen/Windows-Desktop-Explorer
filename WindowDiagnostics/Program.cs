using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WindowDiagnostics
{
    public class Program
    {
        public ObservableCollection<Process> Processes { get; set; }

        public void ListProcesses() 
        {
            Processes = new ObservableCollection<Process>(SystemProcesses.GetPocesses().OrderByDescending(x => x.MainWindowTitle).ThenBy(x => x.Id));
        }
    }
}
