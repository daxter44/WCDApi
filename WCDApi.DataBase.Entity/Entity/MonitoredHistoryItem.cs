using System;
using HtmlAgilityPack;

namespace WCDApi.DataBase.Entity
{
    public class MonitoredHistoryItem
    { 
        Guid MonitoredItemId { get; set; }
        int Type { get; set; }
        string Message { get; set; }
        DateTime Date { get; set; }
        HtmlDocument OldWebPage;
        HtmlDocument NewWebPage;
        HtmlNode OldNode;
        HtmlNode NewNode;
        public MonitoredHistoryItem(Guid monitoredItemId)
        {
            MonitoredItemId = monitoredItemId;
            Date = DateTime.Now;
        }
        public MonitoredHistoryItem(Guid monitoredItemId, HtmlDocument oldWebPage, HtmlDocument newWebPage)
        {
            MonitoredItemId = monitoredItemId;
            Date = DateTime.Now;
            OldWebPage = oldWebPage;
            NewWebPage = newWebPage;
        }
        public MonitoredHistoryItem(Guid monitoredItemId, HtmlNode oldNode, HtmlNode newNode)
        {
            MonitoredItemId = monitoredItemId;
            Date = DateTime.Now;
            OldNode = oldNode;
            NewNode = newNode;

        }
        public void SetTypeError(string message) { Type = HistoryItemType.Error; Message = message; }

        public void SetTypeNothingChanges(string message) { Type = HistoryItemType.NothingChanges; Message = message; }

        public void SetTypeWebHasChanges(string message) { Type = HistoryItemType.WebHasChanges; Message = message; }


    }
}
