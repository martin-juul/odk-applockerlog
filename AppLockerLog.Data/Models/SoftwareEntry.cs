using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLockerLog.Data.Models
{
    public class SoftwareEntry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Vendor { get; set; }
        public string Reasoning { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

        public string CreatedBy { get; set; }
        public string State { get; set; }

        public virtual ICollection<LogEntry> LogEntry { get; set; }
    }
}
