using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CRK2
{
    public class ItemTypeMaker
    {
        private static readonly string c_FILE_PATH = MainClass.ProgramPath + "/dats/ItemTypes.txt";

        private Dictionary<string, int> m_str2intDic;
        private string[] m_int2strArr;

        public int this[string type_str]
        {
            get => m_str2intDic[type_str];
        }

        public string this[int type_int]
        {
            get => m_int2strArr[type_int];
        }

        public void Start()
        {
            string loadedTypes = LoadTypes(c_FILE_PATH);
            m_int2strArr = Tokenize(loadedTypes);
            m_str2intDic = MakeDictionary(m_int2strArr);
        }

        private string LoadTypes(string path)
        {
            StringBuilder builder;
            FileStream istream;
            StreamReader reader;
            string line;

            builder = new StringBuilder();
            istream = new FileStream(path, FileMode.Open);
            reader = new StreamReader(istream);

            while(!reader.EndOfStream)
            {
                line = reader.ReadLine();
                line = line.Replace(" ", "");
                line = line.Replace("\t", "");

                if(line != string.Empty)
                {
                    builder.Append(line);
                    builder.Append(';');
                }
            }

            reader.Close();
            istream.Close();
            reader.Dispose();
            istream.Dispose();

            return builder.ToString();
        }

        private string[] Tokenize(string loadedTypes)
        {
            string[] tokens = loadedTypes.Split(";", StringSplitOptions.RemoveEmptyEntries);

            return tokens;
        }

        private Dictionary<string, int> MakeDictionary(string[] tokenizedTypes)
        {
            int i, n;
            Dictionary<string, int> dict;

            n = tokenizedTypes.Length;
            dict = new Dictionary<string, int>();

            for(i = 0; i < n; i++)
                dict.Add(tokenizedTypes[i], i);

            return dict;
        }

        public void PrintArr()
        {
            int i, n;

            n = m_int2strArr.Length;

            for(i = 0; i < n; i++)
            {
                Console.WriteLine("{0}: {1}", i, m_int2strArr[i]);
            }
        }

        public void PrintDict()
        {
            foreach(string key in m_str2intDic.Keys)
            {
                Console.WriteLine("{0}: {1}", key, m_str2intDic[key]);
            }
        }
    }
}