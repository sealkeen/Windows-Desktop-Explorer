using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFUserControls
{
    public class ProcessesViewModel
    {
        public ProcessesViewModel()
        {
            DiagnosticsProgram.ListProcesses();
        }
        public WindowDiagnostics.Program DiagnosticsProgram = new WindowDiagnostics.Program();
    }
}
