using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPLEmu
{
    public class SinglyLinkedList<T> : IEnumerable<T>
    {
        public class Node<E>
        {
            public Node(E value)
            {
                this.value = value;
            }
            // link to next Node in list
            public Node<E> next = null;
            // value of this Node
            public E value;
        }

        private Node<T> root = null;

        public Node<T> First { get { return root; } }

        public Node<T> Last
        {
            get
            {
                Node<T> curr = root;
                //if (curr == null)
                //    return null;
                while (curr?.next != null)
                    curr = curr.next;
                return curr;
            }
        }

        public void Add(T value)
        {
            Node<T> n = new Node<T>(value);
            if (root == null)
                root = n;
            else
                Last.next = n;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node<T> current = root;
            while (current?.next != null)
            {
                yield return current.value;
                current = current.next;
            }
            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
