using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ObjectModelExtensions;

namespace WindowDiagnostics
{
    public class Program
    {
        public ConcurrentObservableCollection<Process> Processes { get; set; }

        public void ListProcesses() 
        {
            Processes = new ConcurrentObservableCollection<Process>(SystemProcesses.GetPocesses().OrderByDescending(x => x.MainWindowTitle).ThenBy(x => x.Id));
        }
    }
}
