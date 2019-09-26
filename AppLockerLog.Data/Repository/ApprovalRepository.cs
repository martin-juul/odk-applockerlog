using AppLockerLog.Data.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using AppLockerLog.Data.Models;
using AppLockerLog.Data.Utils;

namespace AppLockerLog.Data.Repository
{
    public class ApprovalRepository : IApprovalRepository
    {
        ALLContext context;
        private const string _NEWLINE_ = "\r\n";

        public ApprovalRepository(ALLContext context)
        {
            this.context = context;
        }

        public IQueryable<Approval> All
        {
            get { return context.Approvals.Include(n => n.AssignedUserGroups); }
        }

        public Approval Find(int? id)
        {
            return context.Approvals.Where(x => x.ID == id).FirstOrDefault();
        }

        public IQueryable<Approval> Search(string query)
        {

            return context.Approvals.Include(n => n.AssignedUserGroups).Where(x => x.UserName.Contains(query));
        }

        public void InsertOrUpdate(Approval approval)
        {
            if (approval.ID == default(int))
            {
                // New entity
                context.Approvals.Add(approval);
                Save();
                PushEventLog(
                    $"Added new entry{StringUtils.Repeat(_NEWLINE_, 2) + EventLogger.FormatEntry(approval)}",
                    EventLogEntryType.Information,
                    220);
            }
            else
            {
                // Existing entity
                context.Entry(approval).State = System.Data.Entity.EntityState.Modified;
                Save();
                PushEventLog(
                    $"Updated entry | ID: {approval.ID}" +
                    $"{_NEWLINE_}Edited by: {approval.Approver}",
                    EventLogEntryType.Information,
                    221);
            }
        }

        public void Delete(int id)
        {
            var approval = context.Approvals.FirstOrDefault(x => x.ID == id);

            context.Approvals.Remove(approval);
            Save();
            PushEventLog(
                $"Deleted entry | ID: {approval.ID}",
                EventLogEntryType.Information,
                320);
        }

        public void DeleteGroup(int id)
        {
            var group = context.AssignedUserGroups.FirstOrDefault(g => g.ID == id);

            context.AssignedUserGroups.Remove(group);
            Save();
            PushEventLog(
            $"Deleted Group | ID: {group.ID}",
            EventLogEntryType.Information,
            321);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void SetEditedBy(Approval approval)
        {
            var original = context.Approvals.FirstOrDefault(l => l.ID == approval.ID);
            original.Approver = approval.Approver;
            context.Entry(original).State = System.Data.Entity.EntityState.Modified;
            Save();
            PushEventLog(
                $"Updated editedBy on entry | ID: {approval.ID}",
                EventLogEntryType.Information,
                201);
        }

        public void UpdateReasoning(Approval approval)
        {
            var original = context.Approvals.FirstOrDefault(l => l.ID == approval.ID);
            original.Reasoning = approval.Reasoning;
            context.Entry(original).State = System.Data.Entity.EntityState.Modified;
            Save();
            PushEventLog(
                $"Updated reasoning on entry | ID: {approval.ID}",
                EventLogEntryType.Information,
                222);
        }

        public void AddGroup(Approval approval)
        {
            var original = context.Approvals.FirstOrDefault(l => l.ID == approval.ID);
            original.AssignedUserGroups = approval.AssignedUserGroups;
            context.Entry(original).State = System.Data.Entity.EntityState.Modified;
            Save();
            PushEventLog(
                $"Added group on entry | ID: {approval.ID}",
                EventLogEntryType.Information,
                205);
        }

        public void UpdateComputerName(Approval approval)
        {
            var original = context.Approvals.FirstOrDefault(l => l.ID == approval.ID);
            original.ComputerName = approval.ComputerName;
            context.Entry(original).State = System.Data.Entity.EntityState.Modified;
            Save();
            PushEventLog(
                $"Added or modified ComputerName on User Approval  | ID: {approval.ID}",
                EventLogEntryType.Information,
                206);
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

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
