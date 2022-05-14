using System;

namespace CRK2
{
    public static class CrkManager
    {
        public static ItemTypeMaker itemTypeMaker => m_itemTypeMaker;
        public static CrkdatSerializer crkdatSerializer => m_crkdatSerializer;
        public static JsondatSerializer jsondatSerializer => m_jsondatSerializer;

        private static ItemTypeMaker m_itemTypeMaker;
        private static CrkdatSerializer m_crkdatSerializer;
        private static JsondatSerializer m_jsondatSerializer;

        public static void Start()
        {
            m_itemTypeMaker = new ItemTypeMaker();
            m_crkdatSerializer = new CrkdatSerializer();
            m_jsondatSerializer = new JsondatSerializer();

            Item[] items;

            m_itemTypeMaker.Start();
            if(!m_crkdatSerializer.Start())
            {
                items = m_crkdatSerializer.items;
            }
            else
            {
                m_jsondatSerializer.Start();
                items = m_jsondatSerializer.items;
            }

            foreach(Item i in items)
            {
                Console.WriteLine("{0}", i.ToString());
            }
        }
    }
}