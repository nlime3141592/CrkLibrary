namespace CRK2
{
    public static class CrkManager
    {
        public static ItemTypeMaker itemTypeMaker => m_itemTypeMaker;
        public static CrkdatSerializer crkdatSerializer => m_crkdatSerializer;

        private static ItemTypeMaker m_itemTypeMaker;
        private static CrkdatSerializer m_crkdatSerializer;

        public static void Start()
        {
            m_itemTypeMaker = new ItemTypeMaker();
            m_crkdatSerializer = new CrkdatSerializer();

            m_itemTypeMaker.Start();
            m_crkdatSerializer.Start();
        }
    }
}