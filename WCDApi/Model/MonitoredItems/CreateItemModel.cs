using System;
namespace WCDApi.Model.MonitoredItems
{
    public class CreateItemModel
    {
        public string Url { get; set; }
        public string ElementName { get; set; }
        public int Frequency { get; set; }
        public DateTime StartMonitDate { get; set; }
        public DateTime EndMonitDate { get; set; }
    }
}
