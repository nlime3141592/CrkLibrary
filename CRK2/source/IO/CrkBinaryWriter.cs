using System;
using System.IO;

namespace CRK2
{
    public class CrkBinaryWriter
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


        public CrkBinaryWriter(string path)
        {
            m_filePath = path;
        }

        public bool SaveFile(byte[] contents)
        {
            try
            {
                using(FileStream ostream = new FileStream(m_filePath, FileMode.Create))
                using(BinaryWriter writer = new BinaryWriter(ostream))
                {
                    writer.Write(contents);

                    m_fileContents = ByteUtility.DeepCopy(contents);

                    return true;
                }
            }
            catch(Exception)
            {
                m_fileContents = null;
            }

            return false;
        }
    }
}