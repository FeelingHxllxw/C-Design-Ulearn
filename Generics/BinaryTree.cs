using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Generics.BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        public RootOfTree<T> Root;

        public T Value => Root.value;
        public BinaryTree<T> Left => Root.Left.ToBinaryTree();
        public BinaryTree<T> Right => Root.Right.ToBinaryTree();

        public void Add(T newValue)
        {
            if (Root != null)
                Root.Add(newValue);
            else
                Root = new RootOfTree<T>(newValue);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ReturnInOrder(Root).GetEnumerator();
        }

        private IEnumerable<T> ReturnInOrder(RootOfTree<T> root)
        {
            if (root == null)
                yield break;

            foreach (var item in ReturnInOrder(root.Left))
                yield return item;

            yield return root.value;

            foreach (var item in ReturnInOrder(root.Right))
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class RootOfTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        public T value { get; }
        public RootOfTree<T> Parent { get; set; }
        public RootOfTree<T> Left { get; set; }
        public RootOfTree<T> Right { get; set; }

        public RootOfTree(T value)
        {
            this.value = value;
        }

        public void Add(T newValue)
        {
            if (newValue.CompareTo(value) <= 0)
            {
                if (Left != null)
                    Left.Add(newValue);
                else
                    Left = new RootOfTree<T>(newValue) { Parent = this };
            }
            else
            {
                if (Right != null)
                    Right.Add(newValue);
                else
                    Right = new RootOfTree<T>(newValue) { Parent = this };
            }
        }

        public BinaryTree<T> ToBinaryTree()
        {
            return new BinaryTree<T> { Root = this };
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (value == null)
                yield break;
            yield return value;
            if (Left != null)
            {
                foreach (var item in Left)
                    yield return item;
            }
            if (Right != null)
            {
                foreach (var item in Right)
                    yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public static class BinaryTree
    {
        public static BinaryTree<int> Create(params int[] items)
        {
            BinaryTree<int> binaryTree = new BinaryTree<int>();
            foreach (int item in items)
                binaryTree.Add(item);
            return binaryTree;
        }
    }
}
