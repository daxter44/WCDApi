using System;
using System.ComponentModel.DataAnnotations;
using HtmlAgilityPack;

namespace WCDApi.DataBase.Entity
{
    public class MonitoredHistoryItem
    {
        [Key]
        public Guid MonitoredHistoryItemId { get; set; }
        public Guid MonitoredItemId { get; set; }
        public int Type { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public HtmlDocument OldWebPage;
        public HtmlDocument NewWebPage;
        public HtmlNode OldNode;
        public HtmlNode NewNode;
        public virtual MonitoredItem MonitoredItem { get; set; }

        public MonitoredHistoryItem()
        {

        }

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
