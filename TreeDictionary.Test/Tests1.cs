using System;
using System.Collections;
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
        
        public bool IsEqual<TKey, TValue>(IDictionary<TKey, TValue> d1, IDictionary<TKey, TValue> d2)
        {
            if (d1.Count != d2.Count)
                return false;

            IComparer keyComparer = Comparer<TKey>.Default;
            IComparer valueComparer = Comparer<TValue>.Default;

            var e1 = d1.GetEnumerator();
            var e2 = d2.GetEnumerator();
            while(e1.MoveNext())
            {
                e2.MoveNext();
                if (keyComparer.Compare(e1.Current.Key,e2.Current.Key)!= 0)
                    return false;
                if ((valueComparer.Compare(e1.Current.Value, e2.Current.Value) != 0))
                    return false;
            }
            if (e2.MoveNext())
                return false;
            return true;
        }

        [Test]
        public void Removal()
        {
            TreeDictionary<int, int> mine = new TreeDictionary<int, int>();
            SortedDictionary<int, int> knownGood = new SortedDictionary<int, int>();

            List<KeyValuePair<int,int>> list = new List<KeyValuePair<int, int>>();

            for (int i = 0; i < 500; i++)
                list.Add(new KeyValuePair<int, int>(i, i));

            Assert.True(IsEqual(mine, knownGood));

            foreach (var item in list)
            {
                mine.Add(item);
                knownGood.Add(item.Key, item.Value);
            }

            Assert.True(IsEqual(mine, knownGood));

            Random r = new Random();
            for (int i = 0; i < 10000; i++)
            {
                int x = r.Next();

                Assert.True(knownGood.Remove(x%550) == mine.Remove(x%550));
                

                Assert.True(IsEqual(mine, knownGood));

            }

            mine.Clear();
            knownGood.Clear();

            Assert.True(IsEqual(mine, knownGood));
        }


    }
}
