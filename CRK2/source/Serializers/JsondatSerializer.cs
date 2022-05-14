using System;
using System.Text;
using System.Text.Json;
using System.IO;

#nullable enable

namespace CRK2
{
    public class JsondatSerializer : SerializerBase
    {
        public static readonly string c_JSONDAT_FILE_PATH = MainClass.ProgramPath + "/dats/programDatas/recipes.jsondat";
        public static readonly string c_HASH_FILE_PATH = MainClass.ProgramPath + "/dats/hashes/jsondat.HASH";

        private StringBuilder m_fileContents;

        public JsondatSerializer()
        {
            m_fileContents = LoadFile(c_JSONDAT_FILE_PATH);
        }

        public bool CheckHash()
        {
            byte[] jsondat_hash;
            byte[] verification_hash;
            bool verification;

            jsondat_hash = base.GetHash(m_fileContents);
            verification_hash = base.LoadHashFile(c_HASH_FILE_PATH);
            verification = VerificateHash(jsondat_hash, verification_hash);

            if(!verification)
            {
                SaveHash(c_HASH_FILE_PATH, jsondat_hash);
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
                item = JsondatSerializer.Deserialize(lines[i]);

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
                            writer.WriteLine(JsondatSerializer.Serialize(items[i]));
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
            return JsonSerializer.Serialize(item, typeof(Item));
        }

        public static Item? Deserialize(string jsonLine)
        {
            return (Item?)JsonSerializer.Deserialize(jsonLine, typeof(Item));
        }
    }
}
