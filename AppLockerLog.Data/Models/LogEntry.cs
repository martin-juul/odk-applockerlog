using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLockerLog.Data.Models
{
    [Table("AppLockerLogEntries")]
    public class LogEntry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserName { get; set; }
        public string ComputerName { get; set; }
        public string Ip { get; set; }
        public string ProgramDescription { get; set; }
        public string RapportDescription { get; set; }
        public string Note { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

        public string EditedBy { get; set; }
        public string DeniedBy { get; set; }

        public virtual SoftwareEntry Software { get; set; }
    }
}
