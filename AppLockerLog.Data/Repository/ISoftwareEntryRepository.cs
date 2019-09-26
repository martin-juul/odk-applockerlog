using AppLockerLog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLockerLog.Data.Repository
{
    public interface ISoftwareEntryRepository : IBaseRepository<SoftwareEntry>, IDisposable
    {
        IQueryable<SoftwareEntry> Search(string query);
        IQueryable<SoftwareEntry> GetSoftware(string query);
        void UpdateReasoning(SoftwareEntry softwareEntry);
    }
}
