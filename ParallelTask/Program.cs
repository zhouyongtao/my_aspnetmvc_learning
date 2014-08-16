using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelTask
{
    public class Program
    {
        private static readonly Action<string> logger = Console.WriteLine;
        static void Main(string[] args)
        {
            GetDayOfWeek();
            Console.ReadKey();
        }
        public static void GetDayOfWeek()
        {
            //运行在另外一个线程中
            var dayName = Task.Run<string>(() =>
            {
                logger("wating");
                Thread.Sleep(2000);
                return new DateTime().DayOfWeek.ToString();
            });
            logger("今天是: " + dayName.Result);
        }
    }
}
