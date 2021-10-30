using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public class LRUCache
    {
        public Dictionary<string, string> dictionary { get; set; }
        public LinkedList<string> list { get; set; }

        public int capacity { get; set; }

        public LRUCache()
        {
            dictionary = new Dictionary<string, string>();
            list = new LinkedList<string>();
            capacity = 5;
        }

        public void add(string key, string value)
        {
            if (list.Count == 5)
            {
                dictionary.Remove(list.Last.Value);
                list.RemoveLast();
            }
            list.AddFirst(key);
            dictionary.Add(key, value);
        }

        public string get(string key)
        {
            return dictionary[key];
        }

        public bool containsKey(string key)
        {
            return dictionary.ContainsKey(key);
        }
    }
}
