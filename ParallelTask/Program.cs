using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelTask
{
    public class Program
    {
        private static readonly Action<string> logger = Console.WriteLine;
        static void Main(string[] args)
        {
            logger("Main Current Begin Thread Id :" + Thread.CurrentThread.ManagedThreadId);
            //GetDayOfWeek();
            //TestExpression.StartsWith();
            // TestExpression.Range();
            //TestAwait();
            Task<int> lengthTask =GetPageLengthAsync("http://baidu.com");
            Console.WriteLine(lengthTask.Result);
            logger("Main Current End Thread Id :" + Thread.CurrentThread.ManagedThreadId);

            /*
            string text = @"Do you like green eggs and ham?
                            I do not like them, Sam-I-am.
                            I do not like green eggs and ham.";
            Dictionary<string, int> frequencies = CountWords(text);
            foreach (var entry in frequencies)
            {
                string word = entry.Key;
                int frequency = entry.Value;
                Console.WriteLine("{0}: {1}", word, frequency);
            }
            */
            Console.ReadKey();
        }

        static async Task<int> GetPageLengthAsync(string url)
        {
            using (var client = new HttpClient())
            {
                Task<string> fetchTextTask = client.GetStringAsync(url);
                return (await fetchTextTask).Length;
            }
        }

        static async Task TestAwait()
        {
            await Delay();
            Console.WriteLine("TestAwait Thread Id : " + Thread.CurrentThread.ManagedThreadId);
        }

        static async Task Delay()
        {
            // Delay 方法来自于.net 4.5
            await Task.Delay(3000);  // 返回值前面加 async 之后，方法里面就可以用await了
            Console.WriteLine("Delay Thread Id : " + Thread.CurrentThread.ManagedThreadId);
        }
        public static void GetDayOfWeek()
        {
            //运行在另外一个线程中
            var dayName = Task.Run<string>(() =>
            {
                logger("wating");
                Thread.Sleep(3000);
                return new DateTime().DayOfWeek.ToString();
            });
            logger("今天是: " + dayName.Result);
        }

        static Dictionary<string, int> CountWords(string text)
        {
            var frequencies = new Dictionary<string, int>();
            string[] words = Regex.Split(text, @"\W+");
            foreach (string word in words)
            {
                if (frequencies.ContainsKey(word))
                {
                    frequencies[word]++;
                }
                else
                {
                    frequencies[word] = 1;
                }
            }
            return frequencies;
        }
    }
}
