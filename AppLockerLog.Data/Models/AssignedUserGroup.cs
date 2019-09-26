using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLockerLog.Data.Models
{
    public class AssignedUserGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ApprovalID { get; set; }
        public string Group { get; set; }

        public virtual Approval Approval { get; set; }
    }
}
