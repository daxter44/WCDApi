using System;
using System.ComponentModel.DataAnnotations;

namespace WCDApi.DataBase.Entity
{
    public class MonitoredItem
    {

        [Key]
        public Guid MonitItemId { get; set; }
        public string Url { get; set; }
        public string ElementName { get; set; }
        public int Frequency { get; set; }
        public DateTime StartMonitDate { get; set; }
        public DateTime EndMonitDate { get; set; }
        public int ProcessId { get; set; }
        public bool isActive { get; set; }
        public virtual User User { get; set; }
    }
}
