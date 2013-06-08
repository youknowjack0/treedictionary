/*
Copyright 2013 Jack Langman
All rights reserved.

Based on algorithms in  Cormen, Leiserson, Rivest & Stein, Introduction to 
Algorithms Third edition (MIT Press, 2009)  

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
                Left = -1;
                Right = -1;
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

        private int Find(int from, TKey key)
        {
            while (from != -1)
            {
                Node n = _nodes[from];
                int cmp = _comparer.Compare(key, _nodes[from].Key);
                if (cmp == -1)
                    from = _nodes[from].Left;
                else if (cmp == 1)
                    from = _nodes[from].Right;
                else
                    return from;
            }

            return -1;
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
            {
                _root = next;
                n.Color = Color.Black;
                _nodes[next] = n;
                return;
            }
            else if (cmp == -1)
                _nodes[y].Left = next;
            else
                _nodes[y].Right = next;

            _nodes[next] = n;
            Fixup(next);
            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Fixup(int z)
        {
            int zp;
            while ((zp = _nodes[z].Parent) != -1 && _nodes[zp].Color == Color.Red)
            {
                int zpp = _nodes[zp].Parent;
                if (zp == _nodes[zpp].Left)
                {
                    int y = _nodes[zpp].Right;
                    if (y != -1 && _nodes[y].Color == Color.Red)
                    {
                        _nodes[zp].Color = Color.Black;
                        _nodes[y].Color = Color.Black;
                        _nodes[zpp].Color = Color.Red;
                        z = zpp;
                    }
                    else
                    {
                        if (z == _nodes[zp].Right)
                        {
                            z = zp;
                            RotateLeft(z);
                            zp = _nodes[z].Parent;
                            zpp = _nodes[zp].Parent;
                        }
                        _nodes[zp].Color = Color.Black;
                        _nodes[zpp].Color = Color.Red;
                        RotateRight(zpp);
                    }
                }
                else
                {
                    int y = _nodes[zpp].Left;
                    if (y != -1 && _nodes[y].Color == Color.Red)
                    {
                        _nodes[zp].Color = Color.Black;
                        _nodes[y].Color = Color.Black;
                        _nodes[zpp].Color = Color.Red;
                        z = zpp;
                    }
                    else
                    {
                        if (z == _nodes[zp].Left)
                        {
                            z = zp;
                            RotateRight(z);
                            zp = _nodes[z].Parent;
                            zpp = _nodes[zp].Parent;
                        }
                        _nodes[zp].Color = Color.Black;
                        _nodes[zpp].Color = Color.Red;
                        RotateLeft(zpp);
                    }
                }
            }
            _nodes[_root].Color = Color.Black;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RotateRight(int x)
        {
            int y = _nodes[x].Left;
            int yr;
            _nodes[x].Left = (yr = _nodes[y].Right);
            if (yr != -1)
                _nodes[yr].Parent = x;
            int xp;
            _nodes[y].Parent = (xp = _nodes[x].Parent);
            if (xp == -1)
                _root = y;
            else if (x == _nodes[xp].Right)
                _nodes[xp].Right = y;
            else
                _nodes[xp].Left = y;

            _nodes[y].Right = x;
            _nodes[x].Parent = y;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RotateLeft(int x)
        {
            int y = _nodes[x].Right;
            int yl;
            _nodes[x].Right = (yl = _nodes[y].Left);
            if (yl != -1)
                _nodes[yl].Parent = x;
            int xp;
            _nodes[y].Parent = (xp = _nodes[x].Parent);
            if (xp == -1)
                _root = y;
            else if (x == _nodes[xp].Left)
                _nodes[xp].Left = y;
            else
                _nodes[xp].Right = y;

            _nodes[y].Left = x;
            _nodes[x].Parent = y;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TValue Find(TKey key)
        {
            int index = Find(_root, key);
            if (index == -1) 
                throw new KeyNotFoundException();
            return _nodes[index].Value;
        }

        public TValue this[TKey key]
        {
            get { return Find(key); }
            set { throw new System.NotImplementedException(); }
        }

        public ICollection<TKey> Keys { get; private set; }
        public ICollection<TValue> Values { get; private set; }
    }
}
