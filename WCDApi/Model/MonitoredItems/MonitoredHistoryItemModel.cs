using System;
namespace WCDApi.Model.MonitoredItems
{
    public class MonitoredHistoryItemModel
    {
        public int Type { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}
