using System;
using System.Collections.Generic;

namespace CRK2
{
    public class ItemCountTable
    {
        public int this[string key]
        {
            get
            {
                if(m_dict.ContainsKey(key))
                {
                    return m_dict[key];
                }
                else
                {
                    return -1;
                }
            }

            set
            {
                if(m_dict.ContainsKey(key))
                {
                    m_dict[key] = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("올바르지 않은 키 값");
                }
            }
        }

        private Dictionary<string, int> m_dict;

        public ItemCountTable()
        {
            int n, i;
            string key;

            m_dict = new Dictionary<string, int>();
            n = CrkManager.itemTypeTable.itemCount;

            for(i = 0; i < n; i++)
            {
                key = CrkManager.itemTypeTable[i];
                m_dict.Add(key, 0);
            }
        }

        public int[] ToArray()
        {
            int n;
            int[] arr;
            int idx;

            n = CrkManager.itemTypeTable.itemCount;
            arr = new int[n];

            foreach(string str in m_dict.Keys)
            {
                idx = CrkManager.itemTypeTable[str];
                arr[idx] = m_dict[str];
            }

            return arr;
        }
    }
}