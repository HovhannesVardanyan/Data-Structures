using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace Algo
{
    /// <summary>
    /// 
    /// Implementation of Red Black Tree
    /// 
    /// </summary>
    
    class RedBlackTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        public int Count { get; private set; }
        private Node<T> root = null;
        public T MaxValue => Maximum(root).Data;
        public T MinValue => Minimum(root).Data;
        public T Root
        {
            get
            {
                if (root == null)
                    throw new Exception("The BST is empty!");
                return root.Data;
            }
        }

        public void Add(T data)
        {
            if (root == null)
            {
                root = new Node<T> (data, null);
                root.Color = true;
            }
            else
            {
                Node<T> node = Add(root, data);
                ReBalanceTree(node);
            }

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
        

        private void ReBalanceTree(Node<T> node)
        {
            if (node.parent == null)
            {
                node.Color = true;
                return; 
            }

            if (node.parent.Color)
                return;

            Node<T> parent = node.parent;
            Node<T> grandParent = parent.parent;
            Node<T> uncle = parent.Data.CompareTo(grandParent.Data) > 0 ? grandParent.left : grandParent.right;
            if (uncle == null || uncle.Color)
            {
                int a = grandParent.Data.CompareTo(parent.Data);
                int b = parent.Data.CompareTo(node.Data);
                if (( a  > 0 && b > 0)  || (a <= 0 && b <= 0) ) // line
                {
                    Rotate(grandParent, parent);
                    parent.Color = true;
                    grandParent.Color = false;
                }
                else // triangle
                {
                    Rotate(parent, node);
                    ReBalanceTree(parent);
                }
            }
            else if (!uncle.Color)
            {
                grandParent.Color = false;
                parent.Color = true;
                uncle.Color = true;
                ReBalanceTree(grandParent);
            }
            

        } 
        private void Remove(Node<T> node)
        {
            if (node.left == null && node.right == null)
            {
                Node<T> parent = node.parent;
                if (parent == null)
                    root = null;
                else if (parent.left == node)
                    parent.left = null;
                else
                    parent.right = null;
                ReBalanceTree(node, null);
            }
            else if (node.left == null || node.right == null)
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
                ReBalanceTree(node, l);
            }
            else
            {
                Node<T> elemToReplace = Minimum(node.right);
                SwapValues(elemToReplace, node);
                Remove(elemToReplace);
            }
        }
        private void ReBalanceTree(Node<T> itemRemoved, Node<T> itemInPlace)
        {
            if(itemRemoved.Color)
            {
                if (Black(itemInPlace))
                {
                    Node<T> parent = itemRemoved.parent;

                    if (parent == null)
                        Case1();
                    else
                    {
                        Node<T> sibling = (parent.right == itemInPlace) ? parent.left : parent.right;
                        
                        if (Black(parent) && !sibling.Color && Black(sibling.left) && Black(sibling.right))
                            Case2(parent, sibling);

                        else if (Black(parent) && Black(sibling) && Black(sibling.left) && Black(sibling.right))
                            Case3(parent, sibling);

                        else if (!parent.Color && Black(sibling) && Black(sibling.right) && Black(sibling.left))
                            Case4(parent, sibling);

                        else if (Black(parent) && Black(sibling) && Black(sibling.right) && !sibling.left.Color)
                            Case5(parent, sibling);

                        else if (Black(sibling) && !sibling.right.Color)
                            Case6(parent, sibling);
                    }
                }

                else if (!itemInPlace.Color)
                    itemInPlace.Color = true;
            }
        }

        private bool Black(Node<T> node)
        {
            return node == null || node.Color;
        }
        private void Case1() // termination case
        {
        }
        private void Case2(Node<T> parent, Node<T> sibling)
        {
            Node<T> itself = parent.right == sibling ? parent.left : parent.right;

            Rotate(parent, sibling);
            sibling.Color = true;
            parent.Color = false;

            Node<T> temp = new Node<T>(default(T), parent);
            temp.Color = true;
            ReBalanceTree(temp,itself);
        }
        private void Case3(Node<T> parent, Node<T> sibling)
        {
            sibling.Color = false;
            ReBalanceTree(parent,parent);
        }
        private void Case4(Node<T> parent, Node<T> sibling) // termination case
        {
            parent.Color = true;
            sibling.Color = false;
        }
        private void Case5(Node<T> parent, Node<T> sibling)
        {
            sibling.Color = false;
            sibling.left.Color = true;
            Rotate(sibling,sibling.left);
            Case6(sibling.parent.parent,sibling.parent);
        }
        private void Case6(Node<T> parent, Node<T> sibling) // termination case
        {
            Rotate(parent, sibling);
            sibling.Color = parent.Color;
            parent.Color = true;
            sibling.right.Color = true;
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
        private static void Add(Node<T> node, Node<T> n)
        {
            if (n == null)
                return;
            if (n.Data.CompareTo(node.Data) > 0)
            {
                if (node.right == null)
                {
                    node.right = n;
                    n.parent = node;
                }
                else
                    Add(node.right, n);
            }
            else
            {
                if (node.left == null)
                {
                    node.left = n;
                    n.parent = node;
                }
                else
                    Add(node.left, n);
            }
        }
        private static Node<T> Add(Node<T> node, T data)
        {
            if (data.CompareTo(node.Data) > 0)
            {
                if (node.right == null)
                    return node.right = new Node<T>(data, node);

                return Add(node.right, data);
            }
            else
            {
                if (node.left == null)
                    return node.left = new Node<T>(data, node);

                return Add(node.left, data);
            }
        }
        private void Traverse(Node<T> node, ref StringBuilder sb)
        {
            if (node == null)
                return;
            string color = node.Color ? "B" : "R";
            sb.Append(node.Data + "" + color + " ");
            Traverse(node.left, ref sb);
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
        private static Node<T> Exists(Node<T> node, T data)
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
        private void Rotate(Node<T> parent, Node<T> child)
        {
            if (child.parent != parent)
                throw new Exception("The given nodes are not in parent-child relationship!");

            Node<T> A = child.Data.CompareTo(parent.Data) > 0  ? parent.left : parent.right;
            
            Node<T> B = child.left;

            Node<T> C = child.right;

            Node<T> grandParent = parent.parent;
            if (grandParent != null)
            {
                if (child.Data.CompareTo(grandParent.Data) > 0)
                    grandParent.right = child;

                else
                    grandParent.left = child;

            }
            else
                root = child;

            child.parent = grandParent;
            
            child.left = child.right = null;
            parent.left = parent.right = null;

            Add(child, parent);

            Add(child, A);
            Add(child, B);
            Add(child, C);    
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
            public bool Color = false; // false - red, true - black
            public Node(E data, Node<E> p)
            {
                Data = data;
                parent = p;
            }
        }
    }
}
