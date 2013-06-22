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
using System.ComponentModel;
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
            return new Enumerator(this);
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

        /// <summary>
        /// returns 0 if found
        /// 
        /// when found, index will point to the found node
        /// 
        /// otherwise, index will point to the parent where the node could be created
        /// and return will be -1 for left, 1 for right
        /// </summary>        
        private int Find(ref int index, TKey key)
        {
            int last;
            int cmp;
            do
            {
                last = index;
                cmp = _comparer.Compare(key, _nodes[index].Key);
                if (cmp == -1)
                    index = _nodes[index].Left;
                else if (cmp == 1)
                    index = _nodes[index].Right;
                else
                    return 0;
            } while (index != -1);

            index = last;
            return cmp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="index">-1 if the tree is empty, otherwise index of the parent</param>
        /// <param name="side">-1 for left, 1 for right</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Add(TKey key, TValue value, int index, int side)
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

            n.Parent = index;

            if (index == -1)
            {
                _root = next;
                n.Color = Color.Black;
                _nodes[next] = n;
                return;
            }
            else if (side == -1)
                _nodes[index].Left = next;
            else
                _nodes[index].Right = next;

            _nodes[next] = n;
            Fixup(next);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(TKey key, TValue value)
        {
            int index = _root;
            int cmp;
            if (_root == -1)
                Add(key, value, -1, 0);
            else if ((cmp = Find(ref index, key)) == 0)
                throw new ArgumentException("An element with the same key already exists in the dictionary");
            else
                Add(key, value, index, cmp);
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
            int index = _root;            
            if (Find(ref index, key) != 0) 
                throw new KeyNotFoundException();
            return _nodes[index].Value;
        }

        public TValue this[TKey key]
        {
            get { return Find(key); }
            set
            {
                int index = _root;
                int cmp;
                if ((cmp=Find(ref index, key)) == 0)
                    _nodes[index].Value = value;
                else
                    Add(key, value, index, cmp); 
            }
        }

        private void RemoveImpl(int index)
        {
            throw new NotImplementedException();
        }

        public ICollection<TKey> Keys { get; private set; }
        public ICollection<TValue> Values { get; private set; }

        internal sealed class Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private int _version; //todo
            private TreeDictionary<TKey, TValue>_tree;

            private int _index = -1;
            private Node[] _nodes;
            private int _direction;
            //private int _direction;

            public Enumerator(TreeDictionary<TKey, TValue> tree)
            {
                _tree = tree;
                _index = tree._root;
                _nodes = tree._nodes;
            }

            public void Dispose()
            {
                
            }

            public bool MoveNext()
            {
                if (_index == -1) return false;
                int left;
                int right;
                int parent;

                if (_direction == -1)
                {
                    if ((right = _nodes[_index].Right) != -1)
                    {
                        _index = right;
                        _direction = 0;
                    }
                    else
                        _direction = 1;
                }

                while (_direction == 1) 
                {                    
                    parent = _nodes[_index].Parent;
                    if (parent == -1) 
                        return false;
                    if (_nodes[parent].Left == _index)
                    {
                        _direction = -1;
                        _index = parent;
                        return true;
                    }                    
                    _index = parent;
                }

                while ((left = _nodes[_index].Left) != -1)
                    _index = left;

                _direction = -1;

                return true;
            }

            public void Reset()
            {                
                _index = _tree._root;
            }

            public KeyValuePair<TKey, TValue> Current { 
                get
                {
                    if (_index == -1) throw new InvalidOperationException();
                    else
                        return new KeyValuePair<TKey, TValue>(_nodes[_index].Key, _nodes[_index].Value);
                } 
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }
    }
}
