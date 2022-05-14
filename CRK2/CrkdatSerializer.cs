using System;
using System.Text;
using System.IO;

#nullable enable

namespace CRK2
{
    public class CrkdatSerializer
    {
        private static readonly string c_FILE_PATH = @"./recipes.crkdat";

        public void Start()
        {

        }

        public Item[]? LoadItems(string path)
        {
            
        }

        private Item? Parse(string line)
        {
            string[] split_1;
            string[] split_2;
            Item? item;

            split_1 = line.Split(':', StringSplitOptions.RemoveEmptyEntries);

            if(split_1.Length != 2) throw new Exception("오류");
            
            int itemType = CrkManager.itemTypeMaker[split_1[0]];

            split_1 = split_1[1].Split(',', StringSplitOptions.RemoveEmptyEntries);
            
            int i;

            item = new Item();

            item.itemType = itemType;
            item.recipes = new Recipe[split_1.Length];

            for(i = 0; i < split_1.Length; i++)
            {
                split_2 = split_1[i].Split('/', StringSplitOptions.RemoveEmptyEntries);

                if(split_2.Length != 2) throw new Exception("오류");

                item.recipes[i] = new Recipe();
                item.recipes[i].itemType = CrkManager.itemTypeMaker[split_2[0]];
                item.recipes[i].count = int.Parse(split_2[1]);
            }

            return item;
        }
    }
}
