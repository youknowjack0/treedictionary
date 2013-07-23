using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.AccessControl;

namespace Langman.DataStructures.Test
{
    class Program
    {
        static Stopwatch sw = new Stopwatch();

        static void Main(string[] args)
        {
            
            Dictionary<int, int> pairs = new Dictionary<int, int>();
            const int testDataSetSize = 5000000;            
            Random r = new Random();
            for (int i = 0; i < testDataSetSize; i++)
            {
                pairs[r.Next()] = r.Next();
            }

            Console.WriteLine("<table>");
            Console.WriteLine("<tr><th>Implementation</th><th>Insertion</th><th>Query</th><th>In-order Enumeration</th><th>Removal</th></tr>");

            IDictionary<string, IDictionary<int, int>> candidates = new TreeDictionary<string, IDictionary<int, int>>
                {
                    {"1. System.Collections.Generic.SortedDictionary", new SortedDictionary<int, int>()},
                    {"2. <strong>Langman.DataStructures.TreeDictionary</strong>", new TreeDictionary<int, int>()}
                };

            foreach (var candidate in candidates)
            {
                IDictionary<int, int> dict = candidate.Value;
                string name = candidate.Key;
                BeginRow();                
                WriteCell(name);
                GC.Collect();
                TestAdd(dict, pairs);
                TestQuery(dict, pairs);
                TestEnum(dict, pairs);
                TestRemove(dict, pairs);
                EndRow();
            }
            
            Console.WriteLine("</table>");

        }

        static private void Start()
        {
            sw.Restart();
        }

        static DataStructures.TreeDictionary<string, double> _references = new TreeDictionary<string, double>();

        static private void Stop( int actionCount, string reference)
        {
            sw.Stop();
            double time = sw.ElapsedMilliseconds / (double)(actionCount) * 1000;
            Debug.WriteLine(reference);
            double rVal;
            bool hasReference;
            if (!(hasReference = _references.TryGetValue(reference, out rVal)))
            {
                _references.Add(reference, time);
            }
            if(hasReference)
                WriteCell(string.Format("{0:0.000}&#956;s ({1:0.00}%)", time, time / rVal * 100));
            else
                WriteCell(string.Format("{0:0.000}&#956;s", time));
        }

        static private void WriteCell(string str)
        {
            Console.WriteLine("<td>{0}</td>",str);
        }


        static private void BeginRow()
        {
            Console.WriteLine("<tr>");
        }

        static private void EndRow()
        {
            Console.WriteLine("</tr>");
        }

        static private void TestAdd<TKey,TValue>(IDictionary<TKey,TValue> testee, ICollection<KeyValuePair<TKey,TValue>> items)
        {
            Start();
            foreach (var item in items)
            {
                testee.Add(item);
            }
            Stop(items.Count, "add");
        }

        static private void TestQuery<TKey, TValue>(IDictionary<TKey, TValue> testee, ICollection<KeyValuePair<TKey, TValue>> items)
            where TValue : IEquatable<TValue>
        {
            Start();
            int queryPasses = 2;
            for (int i = 0; i < queryPasses; i++)
                foreach (var item in items)
                {
                    if (!testee[item.Key].Equals(item.Value))
                        throw new Exception();
                }
            Stop(queryPasses*items.Count, "query");
        }

        static private void TestEnum<TKey, TValue>(IDictionary<TKey, TValue> testee, ICollection<KeyValuePair<TKey, TValue>> items)
        {
            Start();
            const int enumPasses = 10;
            TValue x;
            for (int i = 0; i < enumPasses; i++)
                foreach (var item in testee)
                {
                    x = item.Value;
                }

            Stop(enumPasses * items.Count, "enum");
        }

        static private void TestRemove<TKey, TValue>(IDictionary<TKey, TValue> testee,
                                             ICollection<KeyValuePair<TKey, TValue>> items)
        {
            Start();
            foreach (var item in items)
            {
                testee.Remove(item.Key);
            }
            Stop(items.Count, "remove");
        }


    }
}
