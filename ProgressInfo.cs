using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dirHash
{
    public class ProgressInfo
    {
        private readonly int _totalNumberOfItems;
        private int _count;
        private readonly DateTime _start = DateTime.Now;

        public ProgressInfo(int totalNumberOfItems)
        {
            _totalNumberOfItems = totalNumberOfItems;
        }

        public void Increment()
        {
            if (_count != 0)
            {
                Console.CursorTop = Console.CursorTop - 1;
            }
            _count++;

            var percentage = (double) _count / _totalNumberOfItems * 100;
            var toGo = ((100 - percentage) / percentage * (DateTime.Now - _start).TotalSeconds).ToString("#.");
            var toLog =_count + " of " + _totalNumberOfItems + " (" + percentage.ToString("0.00") + "%) " + toGo + " Seconds to go     ";

            toLog += new string(' ', Console.WindowWidth - toLog.Length);

            Console.WriteLine(toLog);
        }
    }
}
