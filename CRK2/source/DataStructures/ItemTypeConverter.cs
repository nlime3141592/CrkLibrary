using System;
using System.Collections.Generic;

namespace CRK2
{
    public class ItemTypeConverter
    {
        public int ItemCount => m_arr_int2str.Length;
        
        // int <-> str_itemType -> viewItemType
        private int m_itemTypeCount;
        private string[] m_arr_int2str;
        private string[] m_arr_int2viewName;
        private Dictionary<string, int> m_dict_str2int;

        public ItemTypeConverter(string[] itemTypes, string[] viewItemNames)
        {
            string str_itemType;
            string viewItemName;
            int i;
            int n, m;

            n = itemTypes.Length;
            m = viewItemNames.Length;

            if(n != m)
                throw new ArgumentException("아이템 갯수 불일치");

            m_itemTypeCount = n;
            m_arr_int2str = new string[n];
            m_arr_int2viewName = new string[n];
            m_dict_str2int = new Dictionary<string, int>();

            for(i = 0; i < n; i++)
            {
                str_itemType = itemTypes[i];
                viewItemName = viewItemNames[i];

                m_arr_int2str[i] = str_itemType;
                m_arr_int2viewName[i] = viewItemName;
                m_dict_str2int.Add(str_itemType, i);
            }
        }

        public int GetItemTypeInt32(string str_itemType)
        {
            try
            {
                int itemType = m_dict_str2int[str_itemType];
                return itemType;
            }
            catch(Exception)
            {
                throw new ArgumentException("존재하지 않는 아이템 이름");
            }
        }

        public string GetItemTypeString(int itemType)
        {
            if(itemType < 0 || itemType >= m_itemTypeCount)
                throw new ArgumentException("존재하지 않는 아이템 번호");

            return m_arr_int2str[itemType];
        }

        public string GetItemTypeViewName(string str_itemType)
        {
            int itemType = GetItemTypeInt32(str_itemType);

            return GetItemTypeViewName(itemType);
        }

        public string GetItemTypeViewName(int itemType)
        {
            if(itemType < 0 || itemType >= m_itemTypeCount)
                throw new ArgumentException("존재하지 않는 아이템 번호");

            return m_arr_int2viewName[itemType] ?? "NULL";
        }
    }
}