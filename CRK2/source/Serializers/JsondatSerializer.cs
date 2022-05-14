using System;
using System.Text;
using System.Text.Json;
using System.IO;

#nullable enable

namespace CRK2
{
    public class JsondatSerializer : SerializerBase
    {
        private static readonly string c_JSONDAT_FILE_PATH = MainClass.ProgramPath + "/dats/recipes.jsondat";
        private static readonly string c_HASH_FILE_PATH = MainClass.ProgramPath + "/dats/jsondat.HASH";

        public Item?[]? items => m_items;

        private StringBuilder? m_fileContents;
        private byte[]? m_jsondat_hash;
        private byte[]? m_verification_hash;
        private Item?[]? m_items;

        public override bool Start()
        {
            string[] lines;
            int i;
            bool verification;

            // 파일 로드
            m_fileContents = LoadFile(c_JSONDAT_FILE_PATH);

            // 해시 검증
            m_jsondat_hash = base.GetHash(m_fileContents);
            m_verification_hash = base.LoadHashFile(c_HASH_FILE_PATH);
            verification = VerificateHash(m_jsondat_hash, m_verification_hash);

            if(!verification)
            {
                Console.WriteLine("Jsondat 해시 검증 실패");
                SaveHash(c_HASH_FILE_PATH, m_jsondat_hash);
            }
            else
            {
                Console.WriteLine("Jsondat 해시 검증 성공");
            }

            // Deserialize
            lines = m_fileContents.ToString().Split(';', StringSplitOptions.RemoveEmptyEntries);
            m_items = new Item[lines.Length];

            for(i = 0; i < lines.Length; i++)
                m_items[i] = JsondatSerializer.Deserialize(lines[i]);

            return true;
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
            return JsonSerializer.Serialize(item, typeof(Item));
        }

        public static Item? Deserialize(string jsonLine)
        {
            return (Item?)JsonSerializer.Deserialize(jsonLine, typeof(Item));
        }
    }
}
