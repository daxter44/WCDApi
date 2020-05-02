using System;
namespace WCDApi.Model.MonitoredItems
{
    public class MonitoredItemModel
    {
        public Guid MonitItemId { get; set; }
        public string Url { get; set; }
        public string ElementName { get; set; }
        public int Frequency { get; set; }
        public DateTime StartMonitDate { get; set; }
        public DateTime EndMonitDate { get; set; }
    }
}
