using System;
using System.Collections.Generic;
using System.Text;

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
            n = CrkManager.itemTypeConverter.ItemCount;

            for(i = 0; i < n; i++)
            {
                key = CrkManager.itemTypeConverter.GetItemTypeString(i);
                m_dict.Add(key, 0);
            }
        }

        public bool isValidTable()
        {
            foreach(int value in m_dict.Values)
            {
                if(value < 0)
                    return false;
            }
            
            return true;
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();

            output.Append("Table:\n");

            foreach(string str in m_dict.Keys)
            {
                if(m_dict[str] > 0)
                {
                    string viewName = CrkManager.itemTypeConverter.GetItemTypeViewName(str);
                    int count = m_dict[str];

                    output.AppendFormat("  {0}: {1}개\n", viewName, count);
                }
            }

            output.Remove(output.Length - 1, 1);

            return output.ToString();
        }
    }
}