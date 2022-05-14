using System;

namespace CRK2
{
    public struct Recipe
    {
        public int itemType { get; set; }
        public int count { get; set ;}

        public override string ToString()
        {
            return string.Format("{0}: {1}", CrkManager.itemTypeTable[itemType], count);
        }
    }
}