using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLockerLog.Data.Utils
{
    public class StringUtils
    {
        public static string Repeat(string input, int amount)
        {
            return String.Concat(Enumerable.Repeat(input, amount));
        }
    }
}
