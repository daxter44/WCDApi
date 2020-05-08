using Timer = System.Timers.Timer;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WCDApi.DataBase.Entity;
using HtmlAgilityPack;
using System.Timers;
using WCDApi.Worker.HTML;
using Microsoft.Extensions.DependencyInjection;
using WCDApi.DataBase.Data;
using Microsoft.Extensions.Options;

namespace WCDApi.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private MonitoredItem _item;
        private MailSettings _mailSettings;
        HtmlDocument _oldWebPage;
        HtmlDocument _newWebPage;
        HtmlNode _oldNode;
        HtmlNode _newNode;
        Timer _timer;

        public Worker(ILogger<Worker> logger, MonitoredItem item, IServiceScopeFactory serviceScopeFactory, IOptions<MailSettings> mailSettings)
        {
            _logger = logger;
            _item = item;
            _serviceScopeFactory = serviceScopeFactory;
            _mailSettings = mailSettings.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            PrepareDetection();
            StartDetection();
        }

        private bool PrepareDetection()
        {
            _oldWebPage = new HtmlDocument();
            _oldWebPage.LoadHtml(HTMLMenager.GetHtmlPage(_item.Url));
            if (_item.ElementName != "")
            {
                _oldNode = _oldWebPage.GetElementbyId(_item.ElementName);
                if (_oldNode == null)
                {

                    MonitoredHistoryItem historyitem = new MonitoredHistoryItem(_item.MonitItemId);
                    historyitem.SetTypeError("Cant find element on website ");
                    SaveToDatabase(historyitem);
                    // HistoryChange(this, myArg);
                    return false;
                }
            }
            return true;
        }
        private void StartDetection()
        {
            _timer = new Timer();
            _timer.Elapsed += timer_Elapsed;
            _timer.Interval = _item.Frequency * 60000;
            _timer.Start();
        }

        private  void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _newWebPage = new HtmlDocument();
            _newWebPage.LoadHtml(HTMLMenager.GetHtmlPage(_item.Url));
            MonitoredHistoryItem historyItem = CompareWebPages();
             SaveToDatabase(historyItem);
            
        }
        private MonitoredHistoryItem CompareWebPages()
        {
            if (_item.ElementName == "")
            {
                MonitoredHistoryItem historyItem = new MonitoredHistoryItem(_item.MonitItemId, _oldWebPage, _newWebPage);
                HTMLCompareResult compareResult = HTMLMenager.HTMLDocumentCompare(_oldWebPage, _newWebPage);

                if (compareResult.Type == 1) { historyItem.SetTypeError(compareResult.Message); };
                if (compareResult.Type == 2)
                {
                    historyItem.SetTypeError(compareResult.Message);
                    _oldWebPage = _newWebPage;
                };
                if (compareResult.Type == 3) { historyItem.SetTypeNothingChanges(compareResult.Message); };
                return historyItem;
            }
            else
            {
                _newNode = _newWebPage.GetElementbyId(_item.ElementName);
                MonitoredHistoryItem historyItem = new MonitoredHistoryItem(_item.MonitItemId, _oldNode, _newNode);
                HTMLCompareResult compareResult = HTMLMenager.HTMLNodeCompare(_oldNode, _newNode);

                if (compareResult.Type == 1) { historyItem.SetTypeError(compareResult.Message); };
                if (compareResult.Type == 2)
                {
                    historyItem.SetTypeError(compareResult.Message);
                    _oldNode = _newNode;
                };
                if (compareResult.Type == 3) { historyItem.SetTypeNothingChanges(compareResult.Message); };
                return historyItem;
            }
        }
        private async void SaveToDatabase(MonitoredHistoryItem item)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            await  dbContext.MonitoredHistory.AddAsync(item);
            await dbContext.SaveChangesAsync();
        }

        

    }
}
