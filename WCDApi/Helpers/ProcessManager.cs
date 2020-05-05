using System;
using System.Diagnostics;

namespace WCDApi.Helpers
{
    public class ProcessManager
    { 
        public static int StartCommand()
        {

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "./Worker/Worker", // or whatever shell you use
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
    }
}
