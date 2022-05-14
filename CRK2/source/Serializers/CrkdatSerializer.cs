using System;
using System.Text;
using System.IO;

#nullable enable

namespace CRK2
{
    public class CrkdatSerializer : SerializerBase
    {
        public static readonly string c_CRKDAT_FILE_PATH = MainClass.ProgramPath + "/dats/programDatas/recipes.crkdat";
        public static readonly string c_HASH_FILE_PATH = MainClass.ProgramPath + "/dats/hashes/crkdat.HASH";

        private StringBuilder m_fileContents;

        public CrkdatSerializer()
        {
            m_fileContents = LoadFile(c_CRKDAT_FILE_PATH);
        }

        public bool CheckHash()
        {
            byte[] crkdat_hash;
            byte[] verification_hash;
            bool verification;

            crkdat_hash = base.GetHash(m_fileContents);
            verification_hash = base.LoadHashFile(c_HASH_FILE_PATH);
            verification = VerificateHash(crkdat_hash, verification_hash);

            if(!verification)
            {
                SaveHash(c_HASH_FILE_PATH, crkdat_hash);
            }

            return verification;
        }

        public Item?[] ToItems()
        {
            Item? item;
            Item?[] items;
            string[] lines;
            int n, m;
            int i;
            int itemType;

            lines = m_fileContents.ToString().Split(';', StringSplitOptions.RemoveEmptyEntries);
            n = CrkManager.itemTypeTable.itemCount;
            m = lines.Length;
            items = new Item[n];

            for(i = 0; i < m; i++)
            {
                item = CrkdatSerializer.Deserialize(lines[i]);

                if(item == null) throw new NullReferenceException("문법 오류");
                itemType = item.itemType;
                if(items[itemType] != null) throw new Exception("아이템의 레시피 중복");

                items[itemType] = item;
            }

            return items;
        }

        public StringBuilder LoadFile(string path)
        {
            StringBuilder fileContents;
            StringBuilder lineContents;

            fileContents = new StringBuilder();
            lineContents = new StringBuilder();

            using(FileStream istream = new FileStream(path, FileMode.Open))
            using(StreamReader reader = new StreamReader(istream))
            {
                while(!reader.EndOfStream)
                {
                    lineContents.Clear();
                    lineContents.Append(reader.ReadLine());

                    RemoveWhiteSpace(lineContents);

                    fileContents.Append(lineContents);
                    fileContents.Append(';');
                }
            }

            return fileContents;
        }

        public bool SaveFile(string path, Item?[]? items)
        {
            if(items == null)
                return false;

            try
            {
                using(FileStream ostream = new FileStream(path, FileMode.Create))
                using(StreamWriter writer = new StreamWriter(ostream))
                {
                    int n, i;
                    n = items.Length;

                    for(i = 0; i < n; i++)
                    {
                        if(items[i] != null)
                        {
                            writer.WriteLine(CrkdatSerializer.Serialize(items[i]));
                        }
                    }
                }

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public static string Serialize(Item item)
        {
            StringBuilder lineContents;
            string itemType;
            string recipeItemType;
            int count;
            int i;
            int n;

            lineContents = new StringBuilder();

            itemType = CrkManager.itemTypeTable[item.itemType];
            lineContents.AppendFormat("{0}:", itemType);

            n = item.recipes.Length;

            recipeItemType = CrkManager.itemTypeTable[item.recipes[0].itemType];
            count = item.recipes[0].count;

            lineContents.AppendFormat(" {0}/{1}", recipeItemType, count);

            for(i = 0; i < n; i++)
            {
                recipeItemType = CrkManager.itemTypeTable[item.recipes[i].itemType];
                count = item.recipes[i].count;

                if(i != 0)
                    lineContents.Append(',');

                lineContents.AppendFormat(" {0}/{1}", recipeItemType, count);
            }

            return lineContents.ToString();
        }

        public static Item? Deserialize(string crkdatLine)
        {
            string[] split_type;
            string[] split_recipes;
            string[] split_recipeData;
            int itemType;
            int i;
            Item? item;

            try
            {
                if(crkdatLine.Contains(';')) throw new Exception("문법 오류");

                item = new Item();

                // ItemType
                split_type = crkdatLine.Split(':', StringSplitOptions.RemoveEmptyEntries);
                if(split_type.Length != 2) throw new Exception("문법 오류");
                itemType = CrkManager.itemTypeTable[split_type[0]];
                item.itemType = itemType;

                split_recipes = split_type[1].Split(',', StringSplitOptions.RemoveEmptyEntries);
                item.recipes = new Recipe[split_recipes.Length];

                // Recipes
                for(i = 0; i < split_recipes.Length; i++)
                {
                    split_recipeData = split_recipes[i].Split('/', StringSplitOptions.RemoveEmptyEntries);

                    if(split_recipeData.Length != 2) throw new Exception("문법 오류");

                    // 1 Recipe
                    item.recipes[i] = new Recipe();
                    item.recipes[i].itemType = CrkManager.itemTypeTable[split_recipeData[0]];
                    item.recipes[i].count = int.Parse(split_recipeData[1]);
                }

                return item;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
