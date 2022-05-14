using System;
using System.Text;

namespace CRK2
{
    public class Item
    {
        public int itemType { get; set; }
        public Recipe[] recipes { get; set; }

        public override string ToString()
        {
            StringBuilder contents;
            int n;
            int i;

            contents = new StringBuilder();

            contents.AppendFormat("Item: {0}\n", CrkManager.itemTypeTable[itemType]);
            contents.AppendFormat("  Recipes:\n");

            n = recipes.Length;

            for(i = 0; i < n; i++)
            {
                contents.AppendFormat("    {0}", recipes[i].ToString());

                if(i < n - 1)
                    contents.Append("\n");
            }

            return contents.ToString();
        }
    }
}