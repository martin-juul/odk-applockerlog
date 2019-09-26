using AppLockerLog.Data.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLockerLog.Data.Logging
{
    public class EventLogger
    {
        protected EventLog eventLog;
        protected string sSource = string.Empty;
        protected string sLog = string.Empty;
        const string _NEWLINE_ = "\r\n";

        public EventLogger()
        {
            this.eventLog = new EventLog();
            this.sSource = "AppLockerLog";
        }

        public void EventLogInformation(string msg, int eventId)
        {
            pushEntry(msg,
                EventLogEntryType.Information,
                eventId);
        }

        /*
        * Formats logging entry.
        * use before insertion to pushEntry, if you want an object formatted nicely.
        * i.e EventLogger.FormatEntry(entity)
        * */
        public static string FormatEntry(Object obj, int newLines = 1)
        {
            Dictionary<string, object> dict = dumpObjectToDict(obj);
            string s = string.Join(StringUtils.Repeat(_NEWLINE_, newLines), dict.Select(x => x.Key + " = " + x.Value).ToArray());

            return s;
        }

        private static Dictionary<string, object> dumpObjectToDict(Object obj)
        {
            Dictionary<string, object> objectDic = new Dictionary<string, object>();

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(obj);
                objectDic.Add(name, value);
            }

            return objectDic;
        }

        private void pushEntry(string msg, EventLogEntryType eventType, int eventId)
        {
            eventLog.Source = sSource;
            eventLog.WriteEntry(msg, eventType, eventId);
            eventLog.Close();
        }
    }
}
