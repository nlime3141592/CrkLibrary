using System;
using System.Text;
using System.IO;

#nullable enable

namespace CRK2
{
    public class CrkdatSerializer : SerializerBase
    {
        private static readonly string c_CRKDAT_FILE_PATH = MainClass.ProgramPath + "/dats/recipes.crkdat";
        private static readonly string c_HASH_FILE_PATH = MainClass.ProgramPath + "/dats/crkdat.HASH";

        public Item?[]? items => m_items;

        private StringBuilder? m_fileContents;
        private byte[]? m_crkdat_hash;
        private byte[]? m_verification_hash;
        private Item?[]? m_items;

        public override bool Start()
        {
            string[] lines;
            int i;
            bool verification;

            // 파일 로드
            m_fileContents = LoadFile(c_CRKDAT_FILE_PATH);

            // 해시 검증
            m_crkdat_hash = base.GetHash(m_fileContents);
            m_verification_hash = base.LoadHashFile(c_HASH_FILE_PATH);
            verification = VerificateHash(m_crkdat_hash, m_verification_hash);

            if(!verification)
            {
                Console.WriteLine("Crkdat 해시 검증 실패");
                SaveHash(c_HASH_FILE_PATH, m_crkdat_hash);

                // Deserialize
                lines = m_fileContents.ToString().Split(';', StringSplitOptions.RemoveEmptyEntries);
                m_items = new Item[lines.Length];

                for(i = 0; i < lines.Length; i++)
                    m_items[i] = CrkdatSerializer.Deserialize(lines[i]);

                return false;
            }
            else
            {
                Console.WriteLine("Crkdat 해시 검증 성공");
                return true;
            }
        }

        protected override StringBuilder LoadFile(string path)
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
                    fileContents.Append(";");
                }
            }

            return fileContents;
        }

        private StringBuilder RemoveWhiteSpace(StringBuilder contents)
        {
            contents.Replace(" ", "");
            contents.Replace("\t", "");
            contents.Replace("\r", "");
            contents.Replace("\n", "");

            return contents;
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

            itemType = CrkManager.itemTypeMaker[item.itemType];
            lineContents.AppendFormat("{0}:", itemType);

            n = item.recipes.Length;

            recipeItemType = CrkManager.itemTypeMaker[item.recipes[0].itemType];
            count = item.recipes[0].count;

            lineContents.AppendFormat(" {0}/{1}", recipeItemType, count);

            for(i = 0; i < n; i++)
            {
                recipeItemType = CrkManager.itemTypeMaker[item.recipes[i].itemType];
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

                split_type = crkdatLine.Split(':', StringSplitOptions.RemoveEmptyEntries);
                if(split_type.Length != 2) throw new Exception("문법 오류");
                itemType = CrkManager.itemTypeMaker[split_type[0]];
                item.itemType = itemType;

                split_recipes = split_type[1].Split(',', StringSplitOptions.RemoveEmptyEntries);
                item.recipes = new Recipe[split_recipes.Length];

                for(i = 0; i < split_recipes.Length; i++)
                {
                    split_recipeData = split_recipes[i].Split('/', StringSplitOptions.RemoveEmptyEntries);

                    if(split_recipeData.Length != 2) throw new Exception("문법 오류");

                    item.recipes[i] = new Recipe();
                    item.recipes[i].itemType = CrkManager.itemTypeMaker[split_recipeData[0]];
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
