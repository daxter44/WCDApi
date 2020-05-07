using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WCDApi.DataBase.Data;
using WCDApi.DataBase.Entity;

namespace WCDApi.Worker
{
    public class Program
    {
        static MonitoredItem _item;
        public static void Main(string[] args)
        {
            ConvertArgsOnMonitoredItem(args);
            CreateHostBuilder(args).Build().Run();
        }

        private static void ConvertArgsOnMonitoredItem(string[] args)
        {
             _item = new MonitoredItem();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-u")) { _item.Url = args[i + 1]; }
                if (args[i].Equals("-e")) { _item.ElementName = args[i + 1]; }
                if (args[i].Equals("-f")) { _item.Frequency = Int32.Parse(args[i + 1]); }
                if (args[i].Equals("-i")) { _item.MonitItemId = Guid.Parse(args[i + 1]); }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<DataContext>();
                    services.AddSingleton(_item);
                    services.AddHostedService<Worker>();
                });
    }
}
