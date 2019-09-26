using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppLockerLog.Data.Models;
using AppLockerLog.Data.Logging;
using System.ComponentModel;
using System.Diagnostics;
using AppLockerLog.Data.Utils;

namespace AppLockerLog.Data.Repository
{
    public class LogEntryRepository : ILogEntryRepository
    {
        ALLContext context;
        private const string _NEWLINE_ = "\r\n";

        public LogEntryRepository(ALLContext context)
        {
            this.context = context;
        }

        public IQueryable<LogEntry> All
        {
            get { return context.LogEntries; }
        }

        public LogEntry Find(int? id)
        {
            return context.LogEntries.Where(e => e.Id == id).FirstOrDefault();
        }

        public IQueryable<LogEntry> Search(string query)
        {
            return context.LogEntries.Where(x =>
            x.UserName.Contains(query)
            || x.ComputerName.Contains(query)
            || x.Ip.Contains(query)
            );
        }

        public void InsertOrUpdate(LogEntry logEntry)
        {

            if (logEntry.Id == default(int))
            {
                // New entity
                context.LogEntries.Add(logEntry);
                Save();
                PushEventLog(
                    $"Added new entry{StringUtils.Repeat(_NEWLINE_, 2) + EventLogger.FormatEntry(logEntry)}",
                    EventLogEntryType.Information,
                    666);
            }
            else
            {
                // Existing entity
                context.Entry(logEntry).State = System.Data.Entity.EntityState.Modified;
                Save();
                PushEventLog(
                    $"Updated entry | ID: {logEntry.Id}" +
                    $"{_NEWLINE_}Edited by: {logEntry.EditedBy}",
                    EventLogEntryType.Information,
                    667);
            }
        }

        public void SetEditedBy(LogEntry logEntry)
        {
            var original = context.LogEntries.FirstOrDefault(l => l.Id == logEntry.Id);
            original.EditedBy = logEntry.EditedBy;
            context.Entry(original).State = System.Data.Entity.EntityState.Modified;
            Save();
            PushEventLog(
                $"Approved entry | ID: {logEntry.Id}",
                EventLogEntryType.Information,
                201);
        }

        public void SetDeniedBy(LogEntry logEntry)
        {
            var original = context.LogEntries.FirstOrDefault(l => l.Id == logEntry.Id);
            original.DeniedBy = logEntry.DeniedBy;
            context.Entry(original).State = System.Data.Entity.EntityState.Modified;
            Save();
            PushEventLog(
                $"Denied entry | ID: {logEntry.Id}",
                EventLogEntryType.Information,
                202);
        }

        public void UpdateNote(LogEntry logEntry)
        {
            var original = context.LogEntries.FirstOrDefault(l => l.Id == logEntry.Id);
            original.Note = logEntry.Note;
            context.Entry(original).State = System.Data.Entity.EntityState.Modified;
            Save();
            /*PushEventLog(
                $"Updated notes on entry | ID: {logEntry.Id}",
                EventLogEntryType.Information,
                203);*/
        }

        public void UpdateSoftware(LogEntry logEntry)
        {
            var original = context.LogEntries.FirstOrDefault(l => l.Id == logEntry.Id);
            original.Software = logEntry.Software;
            context.Entry(original).State = System.Data.Entity.EntityState.Modified;
            Save();
        }

        public void Delete(int id)
        {
            var logEntry = context.LogEntries.Find(id);

            context.LogEntries.Remove(logEntry);
            Save();
            PushEventLog(
                $"Deleted entry | ID: {logEntry.Id}",
                EventLogEntryType.Information,
                300);
        }

        public void Save()
        {
            context.SaveChanges();
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
