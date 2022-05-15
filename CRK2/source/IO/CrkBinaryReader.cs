using System;
using System.IO;

namespace CRK2
{
    public class CrkBinaryReader
    {
        public string FilePath => m_filePath;

        public byte[] Contents
        {
            get
            {
                return ByteUtility.DeepCopy(m_fileContents);
            }
        }


        private string m_filePath;
        private byte[] m_fileContents;


        public CrkBinaryReader(string path)
        {
            m_filePath = path;

            LoadFile(out m_fileContents);
        }

        private bool LoadFile(out byte[] bytes)
        {
            try
            {
                using(FileStream istream = new FileStream(m_filePath, FileMode.Open))
                using(BinaryReader reader = new BinaryReader(istream))
                {
                    bytes = reader.ReadBytes((int)istream.Length);
                }

                return true;
            }
            catch(Exception)
            {
                bytes = null;
            }

            return false;
        }
    }
}