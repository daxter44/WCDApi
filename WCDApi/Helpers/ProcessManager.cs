using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using WCDApi.DataBase.Entity;

namespace WCDApi.Helpers
{
    public class ProcessManager
    { 
        public static int StartCommand(MonitoredItem monitoredItem)
        {
            string args = PrepareArgs(monitoredItem);
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "./Worker/WCDApi.Worker",
                    Arguments = args,// or whatever shell you use
                    UseShellExecute = false,
                    CreateNoWindow = true   
                }
            };
            process.OutputDataReceived += (sender, args) => Console.WriteLine("received output: {0}", args.Data);
            process.Start();
            return process.Id;
        }

        public static bool StopCommand(int processID)
        {
            var process = Process.GetProcessById(processID);
            process.Kill();
            return true;
        }

        private static string PrepareArgs (MonitoredItem monitoredItem)
        {
            return "-u " + monitoredItem.Url + " -e " + monitoredItem.ElementName + " -f " + monitoredItem.Frequency.ToString() + " -i " + monitoredItem.MonitItemId.ToString();
                    
        }
    }
}
