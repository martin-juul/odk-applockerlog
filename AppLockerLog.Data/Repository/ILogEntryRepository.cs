using AppLockerLog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLockerLog.Data.Repository
{
    public interface ILogEntryRepository : IBaseRepository<LogEntry>, IDisposable
    {
        void SetEditedBy(LogEntry logEntry);
        void SetDeniedBy(LogEntry logEntry);
        void UpdateNote(LogEntry logEntry);
        void UpdateSoftware(LogEntry logEntry);
        IQueryable<LogEntry> Search(string query);
    }
}
