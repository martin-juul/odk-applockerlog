using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLockerLog.Data.Models
{
    public class Approval
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string UserName { get; set; }
        public string ComputerName { get; set; }
        public string Reasoning { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Approver { get; set; }

        public virtual ICollection<AssignedUserGroup> AssignedUserGroups { get; set; }
    }
}
