using System;
using System.Text;

namespace CRK2
{
    public static class CrkManager
    {
        public static ItemTypeTable itemTypeTable => m_itemTypeTable;
        public static CrkdatSerializer crkdatSerializer => m_crkdatSerializer;
        public static JsondatSerializer jsondatSerializer => m_jsondatSerializer;

        private static ItemTypeTable m_itemTypeTable;
        private static CrkdatSerializer m_crkdatSerializer;
        private static JsondatSerializer m_jsondatSerializer;

        private static Item[] m_items;

        public static void Start()
        {
            bool verification_hash_typedat;
            bool verification_hash_crkdat;
            bool verification_hash_jsondat;

            m_itemTypeTable = new ItemTypeTable();
            m_crkdatSerializer = new CrkdatSerializer();
            m_jsondatSerializer = new JsondatSerializer();

            verification_hash_typedat = m_itemTypeTable.CheckHash();
            verification_hash_crkdat = m_crkdatSerializer.CheckHash();
            verification_hash_jsondat = m_jsondatSerializer.CheckHash();

            if(!verification_hash_typedat)
            {
                Console.WriteLine("변경된 아이템 정보가 있습니다.");
            }

            if(verification_hash_crkdat && verification_hash_jsondat)
            {
                m_items = m_jsondatSerializer.ToItems();
            }
            else
            {
                Console.WriteLine("변경된 아이템 레시피 정보가 있습니다.");

                m_items = m_crkdatSerializer.ToItems();
                m_jsondatSerializer.SaveFile(JsondatSerializer.c_JSONDAT_FILE_PATH, m_items);
            }
        }

        public static int[] GetRequiredItems(int[] itemCounts, out bool canGetNext)
        {
            bool _canGetNext = false;
            int[] requiredItems;
            Recipe recipe;
            int i, j;
            int n, m;

            n = itemCounts.Length;

            if(CrkManager.itemTypeTable.itemCount != n) throw new ArgumentException("아이템 갯수 불일치");

            requiredItems = new int[n];

            for(i = 0; i < n; i++)
            {
                if(itemCounts[i] < 0)
                {
                    throw new ArgumentException("음수 불가능");
                }
                if(itemCounts[i] > 0)
                {
                    if(m_items[i] == null)
                    {
                        requiredItems[i] += itemCounts[i];
                    }
                    else
                    {
                        m = m_items[i].recipes.Length;

                        for(j = 0; j < m; j++)
                        {
                            recipe = m_items[i].recipes[j];
                            requiredItems[recipe.itemType] += recipe.count;

                            if(m_items[recipe.itemType] != null) _canGetNext = true;
                        }
                    }
                }
            }

            canGetNext = _canGetNext;
            return requiredItems;
        }

        public static string ItemsToString()
        {
            StringBuilder message;

            message = new StringBuilder();

            foreach(Item i in m_items)
            {
                if(i != null)
                {
                    message.AppendFormat("{0}", i.ToString());
                }
            }

            return message.ToString();
        }

        public static string ItemCountsToString(int[] itemCounts)
        {
            if(itemCounts.Length != CrkManager.itemTypeTable.itemCount)
                throw new ArgumentException("갯수 불일치");

            int n, i;
            StringBuilder message;

            n = itemCounts.Length;
            message = new StringBuilder();

            message.Append("Item Count Table:\n");

            for(i = 0; i < n; i++)
            {
                if(itemCounts[i] < 0)
                {
                    throw new ArgumentException("음수 불가능");
                }
                else if(itemCounts[i] > 0)
                {
                    message.AppendFormat("  {0}: {1}개\n", CrkManager.itemTypeTable[i], itemCounts[i]);
                }
            }

            message.Remove(message.Length - 1, 1);
            return message.ToString();
        }
    }
}