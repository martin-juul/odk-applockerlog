using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppLockerLog.Data.Models;
using AppLockerLog.Data.Logging;
using System.Diagnostics;
using AppLockerLog.Data.Utils;
using System.Data.Entity;

namespace AppLockerLog.Data.Repository
{
    public class SoftwareEntryRepository : ISoftwareEntryRepository
    {
        ALLContext context;
        private const string _NEWLINE_ = "\r\n";

        public SoftwareEntryRepository(ALLContext context)
        {
            this.context = context;
        }

        public IQueryable<SoftwareEntry> All
        {
            get { return context.SoftwareEntries.Include(x => x.LogEntry); }
        }

        public IQueryable<SoftwareEntry> Search(string query)
        {
            return context.SoftwareEntries.Where(x =>
                   x.Name.Contains(query)
                || x.Vendor.Contains(query)
            );
        }

        public IQueryable<SoftwareEntry> GetSoftware(string query)
        {
            return from s in context.SoftwareEntries
                   where query.Contains(s.Name)
                   select s;
        }

        public void Delete(int id)
        {
            var entry = context.SoftwareEntries.FirstOrDefault(x => x.Id == id);

            context.SoftwareEntries.Remove(entry);
            Save();
            PushEventLog(
                $"Deleted Software entry | ID: {entry.Id}",
                EventLogEntryType.Information,
                321);
        }

        public SoftwareEntry Find(int? id)
        {
            return context.SoftwareEntries.Where(x => x.Id == id).FirstOrDefault();
        }

        public void InsertOrUpdate(SoftwareEntry entity)
        {
            if (entity.Id == default(int))
            {
                // New entity
                context.SoftwareEntries.Add(entity);
                Save();
                PushEventLog(
                    $"Added new entry{StringUtils.Repeat(_NEWLINE_, 2) + EventLogger.FormatEntry(entity)}",
                    EventLogEntryType.Information,
                    220);
            }
            else
            {
                // Existing entity
                context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                Save();
                PushEventLog(
                    $"Updated entry | ID: {entity.Id}",
                    EventLogEntryType.Information,
                    221);
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateReasoning(SoftwareEntry entity)
        {
            var original = context.SoftwareEntries.FirstOrDefault(x => x.Id == entity.Id);
            original.Reasoning = entity.Reasoning;
            context.Entry(original).State = System.Data.Entity.EntityState.Modified;
            Save();
            PushEventLog(
                $"Updated reasoning on entry | ID: {entity.Id}",
                EventLogEntryType.Information,
                222);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        private void PushEventLog(string message, EventLogEntryType type, int eventId)
        {
            EventLogger eventLogger = new EventLogger();

            switch (type)
            {
                case (EventLogEntryType.Information):
                    eventLogger.EventLogInformation(message, eventId);
                    break;
                default:
                    throw new ArgumentException("Argument missing or not valid.");
            }
        }
    }
}
