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
    /// Implementation of HashTable Data Structure
    /// 
    /// </summary>
    
    class HashTable<TKey,TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private static readonly int MinLength = 13;
        public LinkedList<KeyValuePair<TKey,TValue>>[] items = new LinkedList<KeyValuePair<TKey,TValue>>[MinLength];
        public int Count { get; private set; }
        
        public HashTable()
        {

        }
        public HashTable(int numOfElems)
        {
            if(numOfElems >= MinLength)
                Resize(2 * numOfElems);
        }

        public TValue this[TKey key] {
            get {
                var pair = ItemWithKey(key);
                if(pair == null)
                    throw new Exception("There is no element with the specified key!");
                return pair.Value;
            }
            set
            {
                var pair = ItemWithKey(key);
                if (pair == null)
                    throw new Exception("There is no element with the specified key!");
                pair.Value = value;
            }
        }

        public void Add(TKey key, TValue value)
        {
            int index = Hash(key);
            if (items[index] == null)
            {
                items[index] = new LinkedList<KeyValuePair<TKey, TValue>>();
                items[index].AddFirst(new KeyValuePair<TKey, TValue>(key,value));
            }
            else
            {
                foreach (var KeyValuePair in items[index])
                    if (KeyValuePair.Key.Equals(key))
                        throw new ArgumentException("The given key already exists in the hashtable!");

                items[index].AddFirst(new KeyValuePair<TKey, TValue>(key,value));
            }
            Count++;
            
            AdjustSize(true);
        }
        public void Remove(TKey key)
        {
            var pair = ItemWithKey(key);
            if (pair == null)
                throw new ArgumentException("An item with the specified key does not exist in this hashtable!");

            int index = Hash(key);

            items[index].Remove(pair);

            Count--;

            AdjustSize(false);
        }
        public bool ContainsKey(TKey key)
        {
            return ItemWithKey(key) != null;
        }

        private KeyValuePair<TKey,TValue> ItemWithKey(TKey key)
        {
            int index = Hash(key);

            if (items[index] == null)
                return null;

            foreach (var kvp in items[index])
                if (kvp.Key.Equals(key))
                    return kvp;
                

            return null;
        }
        private void AdjustSize(bool enlarge)
        {
            if (enlarge && Count >=  items.Length / 2)
                Resize(Count * 4);
            else if (!enlarge && items.Length > MinLength && Count <= items.Length / 16)
                Resize(Count * 4);
        }
        private int Hash(TKey key)
        {
            return Math.Abs((key.GetHashCode())) % items.Length;
        }
        private void Resize(int n)
        {
            var hist = items;
            if (n % 2 == 0)
                n++;

            while (!Prime(n))
                n += 2;
            
            if (n < MinLength)
                n = MinLength;

            items = new LinkedList<KeyValuePair<TKey, TValue>>[n];

            foreach (var slot in hist)
            {
                if (slot != null)
                    foreach (var item in slot)
                    {
                        this.Add(item.Key, item.Value);
                        Count--;
                    }
            }
            
                
            
        }
        private bool Prime(int number)
        {
            for (int i = 2; i <= Math.Sqrt(number); i++)
                if (number % i == 0)
                    return false;
            return true;
        }

        public IEnumerator<KeyValuePair<TKey,TValue>> GetEnumerator()
        {
            foreach (var list in items)
            {
                if (list != null)
                    foreach (var item in list)
                        yield return item;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this as IEnumerable<KeyValuePair<TKey, TValue>>).GetEnumerator();
        }
    }
    public class KeyValuePair<K, V>
    {
        public K Key { get; set; }
        public V Value { get; set; }
        public KeyValuePair(K k, V v)
        {
            Key = k;
            Value = v;
        }
    }
}
