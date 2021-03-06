﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
            new List<string>
            {
                "http://163.com",
                "http://sina.com"
            }.Select(async url =>
            {
                using (var client = new HttpClient())
                {
                    await Task.Delay(2000);
                    Console.WriteLine("Task CurrentThread {0}", Thread.CurrentThread.ManagedThreadId);
                    return await client.GetStringAsync(url);
                }
            }).ToList().ForEach(item => Console.WriteLine("tasks...  count={0} CurrentThread {1}", item.Result, Thread.CurrentThread.ManagedThreadId));

            //GetDayOfWeek();
            //TestExpression.StartsWith();
            // TestExpression.Range();
            //TestAwait();
            // Task<int> lengthTask =GetPageLengthAsync("http://baidu.com");
            //Console.WriteLine(lengthTask.Result);

            //  CatchMultipleExceptions();
            // ReadFile();
            Func<int, Task<int>> function = async x =>
            {
                Console.WriteLine("Starting... x={0} CurrentThread {1}", x, Thread.CurrentThread.ManagedThreadId);
                await Task.Delay(x * 1000);
                Console.WriteLine("Finished... x={0} CurrentThread {1}", x, Thread.CurrentThread.ManagedThreadId);
                return x * 2;
            };
            Task<int> first = function(5);
            Task<int> second = function(6);
            Console.WriteLine("First result: {0} CurrentThread {1}", first.Result, Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Second result: {0}  CurrentThread {1}", second.Result, Thread.CurrentThread.ManagedThreadId);
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

        static async Task ReadFile()
        {
            Task<string> task = ReadFileAsync(@"D:\config.xml");
            try
            {
                string text = await task;
                Console.WriteLine("Thread.CurrentThread Id: {0} ReadFileAsync File contents:  {1} ", Thread.CurrentThread.ManagedThreadId, text);
            }
            catch (IOException e)
            {
                Console.WriteLine("Caught IOException: {0}", e.Message);
            }
        }
        static async Task<string> ReadFileAsync(string filename)
        {
            using (var reader = File.OpenText(filename))
            {
                return await reader.ReadToEndAsync();
            }
        }

        private async static Task CatchMultipleExceptions()
        {
            Task task1 = Task.Run(() =>
            {
                throw new Exception("Message 1");
            });
            Task task2 = Task.Run(() =>
            {
                throw new Exception("Message 2");
            });
            try
            {
                await Task.WhenAll(task1, task2);
            }
            catch (AggregateException e)
            {
                Console.WriteLine("Caught {0} exceptions: {1}",
                e.InnerExceptions.Count,
                string.Join(", ",
                e.InnerExceptions.Select(x => x.Message)));
            }
        }
        static async Task<string> GetPageLengthAsync(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    Task<string> fetchTextTask = client.GetStringAsync(url);
                    return await fetchTextTask;
                }
            }
            catch (WebException exception)
            {
                // TODO: Logging, update statistics etc.
                return string.Empty;
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
