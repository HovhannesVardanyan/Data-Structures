using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo
{
    /// <summary>
    /// 
    /// Implementation of Binary Search Tree
    /// 
    /// </summary>
    
    class BST<T> : IEnumerable<T> where T : IComparable<T>
    {
        public int Count { get; private set; }
        private Node<T> root = null;
        public T MaxValue => Maximum(root).Data;
        public T MinValue => Minimum(root).Data;
        public T Root { get {
                if (root == null)
                    throw new Exception("The BST is empty!");
                return root.Data;
            } }
        public BST()
        {

        }
        public void Add(T data)
        {
            if (root == null)
                root = new Node<T>(data, null);
            else
                Add(root, data);
            Count++;
        }
        public bool Exists(T data)
        {
            return Exists(root, data) != null;
        }
        public void Remove(T data)
        {
            Node<T> itemToRemove = Exists(root, data);
            if (itemToRemove == null)
                throw new Exception("The BST doesn't contain an item with the speicfied value");
            Remove(itemToRemove);
            Count--;
        }
        public T Select(int i)
        {
            if (i <= 0 || i > Count)
                throw new Exception("The number  'i' is not valid!");

            return Select(root,i);
        }
        public int Rank(T data)
        {
            return Rank(root, data, 0);
        }

        private static int Rank(Node<T> node,T data,int rank)
        {
            if(node == null)
                throw new Exception("Item not found in the BST");
            
            if (node.Data.CompareTo(data) == 0)
                return rank + Size(node.left) + 1;
            
            else if(data.CompareTo(node.Data) > 0)
            {
                rank += Size(node.left) + 1;
                return Rank(node.right, data, rank);
            }
            return Rank(node.left, data, rank);
        }
        private static int Size(Node<T> node)
        {
            return node != null ? node.Size : 0;
        }
        private static T Select(Node<T> node, int i)
        { 
            int leftPart = Size(node.left);      
            int rightPart = Size(node.right);

            if (leftPart + 1 == i)
                return node.Data;

            else if (i > leftPart + 1)
                return Select(node.right, i - leftPart - 1);
            else
                return Select(node.left, i);
        }
        private void Remove(Node<T> node)
        {
            if(node.left == null && node.right == null)
            {
                Node<T> parent = node.parent;
                if (parent == null)
                    root = null;
                else if (parent.left == node)
                    parent.left = null;
                else
                    parent.right = null;
                RefreshSizes(parent);
            }
            else if(node.left == null || node.right == null)
            {
                Node<T> l = (node.left == null) ? node.right : node.left;
                Node<T> parent = node.parent;

                if (parent == null)
                    root = l;
                else if (parent.left == node)
                    parent.left = l;
                else
                    parent.right = l;

                l.parent = parent;

                RefreshSizes(parent);
            }
            else
            {
                Node<T> elemToReplace = Minimum(node.right);
                SwapValues(elemToReplace,node);
                Remove(elemToReplace);
            }
        }
        private static Node<T> Minimum(Node<T> node)
        {
            if (node == null)
                throw new Exception("The given BST is empty");
            if (node.left == null)
                return node;
            else
                return Minimum(node.left);
            
        }
        private static Node<T> Maximum(Node<T> node)
        {
            if (node == null)
                throw new Exception("The given BST is empty");

            if (node.right == null)
                return node;
            else
                return Maximum(node.right);
        }
        private static void Add(Node<T> node,T data)
        {
            node.Size++;
            if (data.CompareTo(node.Data) > 0)
            {
                if (node.right == null)
                    node.right = new Node<T>(data, node);
                else
                    Add(node.right, data);
            }
            else
            {
                if (node.left == null)
                    node.left = new Node<T>(data, node);
                else
                    Add(node.left, data);
            }
        }
        private void Traverse(Node<T> node,ref StringBuilder sb)
        {
            if (node == null)
                return;
            Traverse(node.left,ref sb);
            sb.Append(node.Data + " ");
            Traverse(node.right, ref sb);
        }
        private void Traverse(Node<T> node, ref LinkedList<T> list)
        {
            if (node == null)
                return;
            Traverse(node.left, ref list);
            list.AddLast(node.Data);
            Traverse(node.right, ref list);
        }
        private static Node<T> Exists(Node<T> node,T data)
        {
            if (node == null)
                return null;

            if (node.Data.CompareTo(data) == 0)
                return node;

            if (data.CompareTo(node.Data) > 0)
                return Exists(node.right, data);
            else
                return Exists(node.left, data);
        }
        private void SwapValues(Node<T> a, Node<T> b)
        {
            T temp = a.Data;
            a.Data = b.Data;
            b.Data = temp;
        }
        private static void RefreshSizes(Node<T> node)
        {
            while(node != null)
            {
                node.Size--;
                node = node.parent;
            }
        }

        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Traverse(root, ref sb);
            return sb.ToString();
        }
        public IEnumerator<T> GetEnumerator()
        {
            LinkedList<T> list = new LinkedList<T>();
            Traverse(root, ref list);
            return list.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this as IEnumerable<T>).GetEnumerator();
        }

        private class Node<E>
        {
            public E Data;
            public Node<E> left;
            public Node<E> right;
            public Node<E> parent;
            public int Size = 1;
            public Node(E data, Node<E> p)
            {
                Data = data;
                parent = p;
            }
        }
    }
    
    
}
