/*
Copyright 2013 Jack Langman
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met: 

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer. 
2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Langman.DataStructures
{
    /// <summary>
    /// A generic dictionary implemented as a Red-Black Tree     
    /// Emphasis on performance 
    /// 
    /// based on algorithms in  Cormen, Leiserson, Rivest & Stein, Introduction to Algorithms Third edition (MIT Press, 2009)  
    /// </summary>    
    public sealed class TreeDictionary<TKey, TValue> : IDictionary<TKey,TValue>
    {

        private Node[] _nodes;
        private int _nodeCount = 0;
        private int[] _freeSlots = new int[0];
        private int _freeSlotsCount = 0;
        private int _nextFree = 0;
        private int _root = -1;
        private IComparer<TKey> _comparer;

        public TreeDictionary(int initSize = 4, IComparer<TKey> comparer = null)
        {
            if (comparer == null)
                _comparer = Comparer<TKey>.Default;
            else
                _comparer = comparer;

            _nodes = new Node[initSize];
        }

        private enum Color
        {
            Red=0,
            Black=-1
        }

        private struct Node
        {
            public Node(TKey key, TValue value) : this()
            {
                Key = key;
                Value = value;
            }

            public int Left;
            public int Right;
            public int Parent;
            public Color Color;
            public TKey Key;
            public TValue Value;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {            
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
            if (_freeSlots.Length != 0)
            {
                _freeSlots[0] = 0;
            }

        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new System.NotImplementedException();
        }

        public int Count { get; private set; }
        public bool IsReadOnly { get; private set; }
        public bool ContainsKey(TKey key)
        {
            throw new System.NotImplementedException();
        }

        public void Add(TKey key, TValue value)
        {
            Node n = new Node(key, value);
            int next;
            if (_freeSlotsCount != 0)
                next = _freeSlots[--_freeSlotsCount];
            else
            {
                if (_nodeCount == _nodes.Length)
                {                        
                    Expand();
                }
                next = _nodeCount++;                                    
            }
            
            //if(_comparer.Compare(key, key.))
            int x = _root;
            int y = -1;
            int cmp = 0; //initialization not actually required
            while (x != -1)
            {
                y = x;                    
                cmp = _comparer.Compare(key, _nodes[x].Key);
                if (cmp == -1)
                    x = _nodes[x].Left;
                else if (cmp == 1)
                    x = _nodes[x].Right;
                else
                    throw new ArgumentException("An element with the same key already exists in the dictionary");
            }

            n.Parent = y;

            if (y == -1)
                _root = next;
            else if (cmp == -1)
                _nodes[y].Left = next;
            else
                _nodes[y].Right = next;

            n.Left = -1;
            n.Right = -1;
           
            

            Fixup(ref n);
            _nodes[next] = n;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Fixup(ref Node node)
        {            
            for(int n = node.Parent; )
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Expand()
        {
            Array.Resize(ref _nodes, _nodes.Length*2);
        }

        public bool Remove(TKey key)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            throw new System.NotImplementedException();
        }

        public TValue this[TKey key]
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public ICollection<TKey> Keys { get; private set; }
        public ICollection<TValue> Values { get; private set; }
    }
}
