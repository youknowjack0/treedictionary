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


    }
}
