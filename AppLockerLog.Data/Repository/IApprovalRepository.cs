using AppLockerLog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLockerLog.Data.Repository
{
    public interface IApprovalRepository : IBaseRepository<Approval>, IDisposable
    {
        void SetEditedBy(Approval approval);
        void UpdateReasoning(Approval approval);
        void UpdateComputerName(Approval approval);
        void AddGroup(Approval approval);
        void DeleteGroup(int id);
        IQueryable<Approval> Search(string query);
    }
}
