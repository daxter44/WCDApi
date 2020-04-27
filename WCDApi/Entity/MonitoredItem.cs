using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WCDApi.Entity
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
        public virtual User User { get; set; }
    }
}
