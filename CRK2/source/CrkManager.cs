using System;
using System.Text;

namespace CRK2
{
    public static class CrkManager
    {
        public static ItemTypeConverter itemTypeConverter => s_m_itemTypeConverter;

        private static readonly string c_TYPEDAT_FILE_PATH = MainClass.ProgramPath + "/dats/programDatas/types.txt";
        private static readonly string c_TYPEDAT_HASH_PATH = MainClass.ProgramPath + "/dats/hashes/types.HASH";

        private static readonly string c_CRKDAT_FILE_PATH = MainClass.ProgramPath + "/dats/programDatas/crecipes.crkdat";
        private static readonly string c_CRKDAT_HASH_PATH = MainClass.ProgramPath + "/dats/hashes/crecipes.HASH";

        private static readonly string c_JSONDAT_FILE_PATH = MainClass.ProgramPath + "/dats/programDatas/jrecipes.jsondat";
        private static readonly string c_JSONDAT_HASH_PATH = MainClass.ProgramPath + "/dats/hashes/jrecipes.HASH";

        private static CrkStreamReader s_m_typedatFile;
        private static CrkStreamReader s_m_crkdatFile;
        private static CrkStreamReader s_m_jsondatFile;

        private static CrkBinaryReader s_m_typedatHash;
        private static CrkBinaryReader s_m_crkdatHash;
        private static CrkBinaryReader s_m_jsondatHash;

        private static HashGenerator s_m_hashGenerator;
        private static HashAlgorithmType s_m_hashAlgorithmType = HashAlgorithmType.SHA_512;

        private static bool s_mb_verification_hash_typedat;
        private static bool s_mb_verification_hash_crkdat;
        private static bool s_mb_verification_hash_jsondat;

        private static Encoding s_m_encoding;
        private static string s_m_encodingMode = "utf-8";

        private static ItemTypeConverter s_m_itemTypeConverter;

        private static int s_m_itemCount;
        private static Item[] s_m_items;

        public static void Start()
        {
            FileVerification();
        }

        private static void FileVerification()
        {
            byte[] typedat_hash;
            byte[] crkdat_hash;
            byte[] jsondat_hash;

            s_m_hashGenerator = new HashGenerator(s_m_hashAlgorithmType);
            s_m_encoding = Encoding.GetEncoding(s_m_encodingMode);

            // 1. type ?????? ??????
            s_m_typedatFile = new CrkStreamReader(c_TYPEDAT_FILE_PATH);
            s_m_typedatHash = new CrkBinaryReader(c_TYPEDAT_HASH_PATH);
            typedat_hash = s_m_hashGenerator.ComputeHash(s_m_typedatFile.Contents, s_m_encoding);
            s_mb_verification_hash_typedat = Verification.CheckHash(typedat_hash, s_m_typedatHash.Contents);

            if(!s_mb_verification_hash_typedat)
            {
                Console.WriteLine("????????? ?????? ????????? ??????????????????.");

                SaveHash(c_TYPEDAT_HASH_PATH, typedat_hash);

                Console.WriteLine("????????? ?????? ?????? ????????? ??????????????????.");
            }

            s_m_itemTypeConverter = GetItemTypeConverter(s_m_typedatFile.Contents);

            // 2. crkdat ?????? ??????
            s_m_crkdatFile = new CrkStreamReader(c_CRKDAT_FILE_PATH);
            s_m_crkdatHash = new CrkBinaryReader(c_CRKDAT_HASH_PATH);
            crkdat_hash = s_m_hashGenerator.ComputeHash(s_m_crkdatFile.Contents, s_m_encoding);
            s_mb_verification_hash_crkdat = Verification.CheckHash(crkdat_hash, s_m_crkdatHash.Contents);

            // 3. jsondat ?????? ??????
            s_m_jsondatFile = new CrkStreamReader(c_JSONDAT_FILE_PATH);
            s_m_jsondatHash = new CrkBinaryReader(c_JSONDAT_HASH_PATH);
            jsondat_hash = s_m_hashGenerator.ComputeHash(s_m_jsondatFile.Contents, s_m_encoding);
            s_mb_verification_hash_jsondat = Verification.CheckHash(jsondat_hash, s_m_jsondatHash.Contents);

            // 4. ????????? ??????
            s_m_itemCount = s_m_itemTypeConverter.ItemCount;
            s_m_items = CrkdatToItems(s_m_itemTypeConverter, s_m_crkdatFile.Contents);

            // 5. ?????? ?????? ?????? ??? ?????????
            if(!s_mb_verification_hash_crkdat)
            {
                Console.WriteLine("????????? ?????? ????????? ??????????????????.");

                SaveHash(c_CRKDAT_HASH_PATH, crkdat_hash);

                Console.WriteLine("????????? ????????? ???????????? ???????????????.");

                ItemsToJsondat(c_JSONDAT_FILE_PATH, s_m_items, s_m_encodingMode);

                Console.WriteLine("????????? ?????? ?????? ????????? ??????????????????.");
            }
            else if(!s_mb_verification_hash_jsondat)
            {
                Console.WriteLine(".jsondat ????????? ????????? ????????? ?????? ????????? ???????????????.");

                ItemsToJsondat(c_JSONDAT_FILE_PATH, s_m_items, s_m_encodingMode);

                s_m_jsondatFile = new CrkStreamReader(c_JSONDAT_FILE_PATH);
                s_m_jsondatHash = new CrkBinaryReader(c_JSONDAT_HASH_PATH);
                jsondat_hash = s_m_hashGenerator.ComputeHash(s_m_jsondatFile.Contents, s_m_encoding);
                
                SaveHash(c_JSONDAT_HASH_PATH, jsondat_hash);

                Console.WriteLine(".jsondat ?????? ????????? ?????????????????????.");
            }
        }

        private static void SaveHash(string path, byte[] hash)
        {
            CrkBinaryWriter writer = new CrkBinaryWriter(path);
            writer.SaveFile(hash);
        }

        private static ItemTypeConverter GetItemTypeConverter(string typedatFileContents)
        {
            ItemTypeConverter converter;
            string contents = StringUtility.RemoveWhiteSpace(typedatFileContents);
            string[] lines;
            string[] str_itemTypes;
            string[] viewNames;
            int itemTypeCount;

            lines = contents.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            itemTypeCount = lines.Length;

            str_itemTypes = new string[itemTypeCount];
            viewNames = new string[itemTypeCount];

            for(int i = 0; i < itemTypeCount; i++)
            {
                TypedatCodec.GetItemType(lines[i], out str_itemTypes[i], out viewNames[i]);
            }

            converter = new ItemTypeConverter(str_itemTypes, viewNames);

            return converter;
        }

        private static Item[] CrkdatToItems(ItemTypeConverter converter, string crkdatFileContents)
        {
            Item[] items;
            Item item;
            string contents = StringUtility.RemoveWhiteSpace(crkdatFileContents);
            string[] lines;
            int recipeCount;
            int itemCount;
            int itemType;
            bool hasRecipe;

            lines = contents.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            recipeCount = lines.Length;
            itemCount = converter.ItemCount;
            items = new Item[itemCount];

            for(int i = 0; i < recipeCount; i++)
            {
                item = CrkdatCodec.GetItem(lines[i]); 

                itemType = item.itemType;
                hasRecipe = (item.recipes != null);

                if(items[itemType] == null)
                    items[itemType] = item;
                else
                    throw new RecipeException("????????? ??????");
            }

            return items;
        }

        private static void ItemsToJsondat(string path, Item[] items, string encodingMode)
        {
            StringBuilder contents = new StringBuilder();
            CrkStreamWriter writer = new CrkStreamWriter(path, encodingMode);

            for(int i = 0; i < items.Length; i++)
            {
                if(items[i] != null)
                {
                    contents.AppendFormat("{0}\n", JsondatCodec.GetJsondatLine(items[i]));
                }
            }

            writer.SaveFile(contents);
        }

        public static ItemCountTable Calculate(ItemCountTable table, out bool canGetNext)
        {
            if(!table.isValidTable())
                throw new ArgumentException("????????? ??? ?????? ??????");

            ItemCountTable newTable = new ItemCountTable();
            
            int i, j;
            bool _canGetNext = false;

            for(i = 0; i < s_m_itemCount; i++)
            {
                string str_itemType = CrkManager.itemTypeConverter.GetItemTypeString(i);
                newTable[str_itemType] = 0;

                if(s_m_items[i] == null)
                {
                    str_itemType = CrkManager.itemTypeConverter.GetItemTypeString(i);
                    newTable[str_itemType] += table[str_itemType];
                }
                else if(table[str_itemType] > 0)
                {
                    int recipeCount = s_m_items[i].recipes.Length;
                    int productCount = table[str_itemType];

                    for(j = 0; j < recipeCount; j++)
                    {
                        Recipe recipe = s_m_items[i].recipes[j];
                        str_itemType = CrkManager.itemTypeConverter.GetItemTypeString(recipe.itemType);

                        newTable[str_itemType] += (recipe.count * productCount);
                        if(s_m_items[recipe.itemType] != null) _canGetNext = true;
                    }
                }
            }

            canGetNext = _canGetNext;
            return newTable;
        }
    }
}