using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CRK2
{
    public class ItemTypeTable : SerializerBase
    {
        public static readonly string c_ITEM_TYPE_FILE_PATH = MainClass.ProgramPath + "/dats/programDatas/types.txt";
        public static readonly string c_HASH_FILE_PATH = MainClass.ProgramPath + "/dats/hashes/type.HASH";

        private StringBuilder m_fileContents;

        public string this[int type_int] => m_int2strArr[type_int];
        private string[] m_int2strArr;

        public int this[string type_str] => m_str2intDic[type_str];
        private Dictionary<string, int> m_str2intDic;

        public int itemCount => m_itemCount;
        private int m_itemCount;

        public ItemTypeTable()
        {
            m_fileContents = LoadFile(c_ITEM_TYPE_FILE_PATH);
            m_int2strArr = ToStrings(m_fileContents);
            m_str2intDic = ToDictionary(m_int2strArr);

            m_itemCount = m_int2strArr.Length;
        }

        public bool CheckHash()
        {
            byte[] type_hash;
            byte[] verification_hash;
            bool verification;

            type_hash = base.GetHash(m_fileContents);
            verification_hash = base.LoadHashFile(c_HASH_FILE_PATH);
            verification = VerificateHash(type_hash, verification_hash);

            if(!verification)
            {
                SaveHash(c_HASH_FILE_PATH, type_hash);
            }

            return verification;
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

        public string ToStringTable()
        {
            StringBuilder message;
            int n, i;
            
            message = new StringBuilder();
            n = m_str2intDic.Count;

            message.AppendFormat("Registered Item Types:\n");

            for(i = 0; i < n; i++)
            {
                message.AppendFormat("  {0}: {1}", i, m_int2strArr[i]);

                if(i < n - 1)
                    message.Append('\n');
            }

            return message.ToString();
        }

        public static string[] ToStrings(StringBuilder fileContents)
        {
            string[] tokens = fileContents.ToString().Split(";", StringSplitOptions.RemoveEmptyEntries);

            return tokens;
        }

        public static Dictionary<string, int> ToDictionary(string[] typeStrings)
        {
            Dictionary<string, int> dict;
            int n, i;

            dict = new Dictionary<string, int>();
            n = typeStrings.Length;

            for(i = 0; i < n; i++)
                dict.Add(typeStrings[i], i);

            return dict;
        }
    }
}