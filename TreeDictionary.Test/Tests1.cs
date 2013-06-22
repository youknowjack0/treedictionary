using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace Langman.DataStructures.Test
{
    [TestFixture]
    public class Tests1
    {

        [Test]
        public void Insertion1()
        {
            int total = 5000000;

            TreeDictionary<int, int> d = new TreeDictionary<int, int>();

            Dictionary<int,int> pairs = new Dictionary<int, int>();

            Random r = new Random();
            for (int i = 0; i < total; i++)
            {
                pairs[r.Next()] = r.Next();
            }

            foreach (var item in pairs)
            {
                d.Add(item.Key,item.Value);
            }

            foreach (var item in pairs)
            {
                Assert.True(d[item.Key] == item.Value);
            }



        }

        [Test]
        public void InOrderWalk()
        {
            TreeDictionary<int, int> dict = new TreeDictionary<int, int>();
            SortedDictionary<int, int> reference = new SortedDictionary<int, int>();

            Random r = new Random();
            for (int i = 0; i < 500; i++)
            {
                dict.Add(i, i);
                reference.Add(i, i);
            }

            var eDict = dict.GetEnumerator();
            var eReference = reference.GetEnumerator();

            while (eReference.MoveNext())
            {
                bool next = eDict.MoveNext();
                Assert.True(next);
                Assert.True(eDict.Current.Key == eReference.Current.Key && eDict.Current.Value == eReference.Current.Value);
            }

            Assert.False(eDict.MoveNext());
            Assert.False(eDict.MoveNext());
            Assert.False(eDict.MoveNext());
            
        }


    }
}
