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
                sw.Stop();
                Console.WriteLine("Add {0} items to System.Collections.Generic.SortedDictionary: {1}", total, sw.ElapsedMilliseconds);
                sw.Restart();
                for (int i = 0; i < 10;i++ )
                    foreach (var item in pairs)
                    {
                        if(ms[item.Key] != item.Value)
                            Trace.Assert(false);
                    }
                Console.WriteLine("Query {0} items from System.Collections.Generic.SortedDictionary: {1}", total*10, sw.ElapsedMilliseconds);
                sw.Stop();
            }
            {
                TreeDictionary<int, int> td = new TreeDictionary<int, int>();
                Stopwatch sw = new Stopwatch();
                sw.Start();
                foreach (var item in pairs)
                {
                    td.Add(item.Key, item.Value);
                }
                sw.Stop();
                Console.WriteLine("Add {0} items to Langman.DataStructures.TreeDictionary: {1}", total, sw.ElapsedMilliseconds);
                sw.Restart();
                for (int i = 0; i < 10; i++)
                    foreach (var item in pairs)
                    {
                        if(td[item.Key] != item.Value)
                            Trace.Assert(false);
                    }
                sw.Stop();
                Console.WriteLine("Query {0} items from Langman.DataStructures.TreeDictionary: {1}", total * 10, sw.ElapsedMilliseconds);
            }

        }
    }
}
