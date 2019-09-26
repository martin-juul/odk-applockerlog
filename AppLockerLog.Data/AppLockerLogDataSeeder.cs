using AppLockerLog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLockerLog.Data.Repository
{
    class AppLockerLogDataSeeder
    {
        ALLContext _context;
        public AppLockerLogDataSeeder(ALLContext context)
        {
            this._context = context;
        }

        public void Seed()
        {
           if (this._context.LogEntries.Count() <= 0)
            {
                for (int i = 0; i < logEntries.Length; i++)
                {
                    var entryInfo = SplitValue(logEntries[i]);
                    var entry = new LogEntry
                    {
                        UserName = entryInfo[0],
                        ComputerName = entryInfo[1],
                        Ip = entryInfo[2],
                        ProgramDescription = entryInfo[3],
                        RapportDescription = entryInfo[4],
                        TimeStamp = DateTime.Today,
                        Note = null,
                        EditedBy = null
                    };

                    this._context.LogEntries.Add(entry);
                }
            }

            if (this._context.Approvals.Count() <= 0)
            {
                for (int i = 0; i < approvals.Length; i++)
                {
                    var entryInfo = SplitValue(approvals[i]);
                    var entry = new Approval
                    {
                        UserName = entryInfo[0],
                        Reasoning = entryInfo[1],
                        TimeStamp = DateTime.Now,
                        Approver = entryInfo[2]
                    };

                    this._context.Approvals.Add(entry);
                    this._context.SaveChanges();

                    var groupEntry = new AssignedUserGroup
                    {
                        ApprovalID = entry.ID,
                        Group = entryInfo[3]
                    };
                    this._context.AssignedUserGroups.Add(groupEntry);
                    this._context.SaveChanges();
                }
            }

            if (this._context.SoftwareEntries.Count() <= 0)
            {
                for (int i = 0; i < softwareEntries.Length; i++)
                {
                    var entryInfo = SplitValue(softwareEntries[i]);
                    var entry = new SoftwareEntry
                    {
                        Name = entryInfo[0],
                        Vendor = entryInfo[1],
                        Reasoning = entryInfo[2],
                        TimeStamp = DateTime.Now,
                        CreatedBy = entryInfo[3],
                        State = entryInfo[4]
                    };
                    this._context.SoftwareEntries.Add(entry);
                    this._context.SaveChanges();
                }
            }

        }

        private static string[] SplitValue(string val)
        {
            return val.Split(',');
        }

        static string[] softwareEntries =
        {
            // Format: Name, Vendor, Reasoning, Created By, State
            "Chrome,Google Inc.,Godkendt browser,marjc,approved",
            "Firefox,Mozilla,Godkendt browser,marjc,approved",
            "Chrome Canary,Google Inc.,Afvist browser - ustabil betasoftware,marjc,denied",
            "Opera,Opera Software,Afvist - kinesisk spyware,marjc,denied"
        };

        static string[] logEntries =
        {
            // Format: Username, Ip, Program Description, Rapport Description
            "marjc,C21294,10.1.139.53,%SystemRoot%\\System32\\drivers\\etc\\rnsom.ps1,Programmet stoppede med at virke..",
            "logiw,C21245,10.1.129.54,%SystemRoot%\\System32\\drivers\\etc\\rnsom.COM,Programmet stoppede med at virke..",
            "niraf,C21233,10.1.119.55,%SystemRoot%\\System32\\drivers\\etc\\rnsom.exe,Programmet stoppede med at virke..",
            "putio,C21212,10.1.109.56,%SystemRoot%\\System32\\drivers\\etc\\rnsom.bat,Programmet stoppede med at virke..",
            "kdawr,C21247,10.1.149.57,%SystemRoot%\\System32\\drivers\\etc\\rnsom.sh,Programmet stoppede med at virke..",
            "vnnam,C21274,10.1.199.58,%SystemRoot%\\System32\\drivers\\etc\\rnsom.pem,Programmet stoppede med at virke..",
            "marjc,C21294,10.1.139.53,%SystemRoot%\\System32\\drivers\\etc\\rnsom.ps1,Programmet stoppede med at virke..",
            "logiw,C21245,10.1.129.54,%SystemRoot%\\System32\\drivers\\etc\\rnsom.COM,Programmet stoppede med at virke..",
            "niraf,C21233,10.1.119.55,%SystemRoot%\\System32\\drivers\\etc\\rnsom.exe,Programmet stoppede med at virke..",
            "putio,C21212,10.1.109.56,%SystemRoot%\\System32\\drivers\\etc\\rnsom.bat,Programmet stoppede med at virke..",
            "kdawr,C21247,10.1.149.57,%SystemRoot%\\System32\\drivers\\etc\\rnsom.sh,Programmet stoppede med at virke.."

        };

        static string[] approvals =
        {
            // Format: Username, Reasoning, Approver, Group
            "marjc,Dette er en begrundelse..,anduq,ODK-W7-SHAREPOINT-MGMT",
            "marjc,Dette er en begrundelse..,anduq,ODK-W10-O365-FLOW-DIRECTOR",
            "marjc,Dette er en begrundelse..,anduq,ODK-RADIUS-GUEST-WIFI",
            "marjc,Dette er en begrundelse..,anduq,ODK-BOYD-EMPLOYMENT-WIFI-ACCESS",
            "marjc,Dette er en begrundelse..,anduq,ODK-LDAP-MGMT-ACCESS",
            "marjc,Dette er en begrundelse..,anduq,ODK-APPLOCKER-LOCKDOWN",
        };
    }
}
