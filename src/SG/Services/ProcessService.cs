using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using SG.Models;

namespace SG.Services
{
    public sealed class ProcessService
    {
        public IReadOnlyList<ProcessRow> GetProcesses(string filter)
        {
            var rows = new List<ProcessRow>();
            foreach (var p in Process.GetProcesses().OrderBy(x => x.ProcessName))
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(filter) &&
                        p.ProcessName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) < 0 &&
                        p.Id.ToString() != filter) continue;
                    rows.Add(new ProcessRow { Id=p.Id, Name=p.ProcessName, Title=Safe(() => p.MainWindowTitle),
                        Memory=SafeMemory(p), Architecture=GetArchitecture(p), Status="Running" });
                }
                catch { }
                finally { p.Dispose(); }
            }
            return rows;
        }

        public bool RequestClose(int pid)
        {
            try {
                using (var p = Process.GetProcessById(pid)) {
                    if (p.HasExited) return true;
                    if (!p.CloseMainWindow()) return false;
                    return p.WaitForExit(2000) || p.HasExited;
                }
            } catch { return false; }
        }

        private static string Safe(Func<string> f) { try { return f() ?? ""; } catch { return ""; } }
        private static string SafeMemory(Process p) {
            try { return (p.WorkingSet64 / 1024d / 1024d).ToString("0.0") + " MB"; } catch { return "-"; }
        }
        private static string GetArchitecture(Process p) {
            try {
                if (!Environment.Is64BitOperatingSystem) return "x86";
                bool wow64;
                if (!IsWow64Process(p.Handle, out wow64)) return "?";
                return wow64 ? "x86" : "x64";
            } catch { return "Protected"; }
        }
        [DllImport("kernel32.dll", SetLastError=true)]
        private static extern bool IsWow64Process(IntPtr hProcess, out bool wow64Process);
    }
}