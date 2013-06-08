using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Langman.DataStructures.Test
{
    class Program
    {
        static void Main(string[] args)
        {

            Dictionary<int, int> pairs = new Dictionary<int, int>();
            int total = 5000000;

            Random r = new Random();
            for (int i = 0; i < total; i++)
            {
                pairs[r.Next()] = r.Next();
            }
            {
                SortedDictionary<int, int> ms = new SortedDictionary<int, int>();
                Stopwatch sw = new Stopwatch();
                sw.Start();
                foreach (var item in pairs)
                {
                    ms.Add(item.Key, item.Value);
                }
                foreach (var item in pairs)
                {
                    if(ms[item.Key] != item.Value)
                        throw new Exception();
                }
                sw.Stop();
                Console.WriteLine("System.Collections.Generic.SortedDictionary: " + sw.ElapsedMilliseconds);
            }
            {
                TreeDictionary<int, int> td = new TreeDictionary<int, int>();
                Stopwatch sw = new Stopwatch();
                sw.Start();
                foreach (var item in pairs)
                {
                    td.Add(item.Key, item.Value);
                }
                foreach (var item in pairs)
                {
                    if(td[item.Key] != item.Value)
                        throw new Exception();
                }
                sw.Stop();
                Console.WriteLine("Langman.DataStructures.TreeDictionary: " + sw.ElapsedMilliseconds);
            }

        }
    }
}
