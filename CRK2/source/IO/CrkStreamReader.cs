using System;
using System.IO;
using System.Text;

namespace CRK2
{
    public class CrkStreamReader
    {
        public string FilePath => m_filePath;

        public string Contents
        {
            get
            {
                if(m_fileContentsString == null)
                    m_fileContentsString = m_fileContents.ToString();

                return m_fileContentsString;
            }
        }

        public string EncodingMode => m_encodingMode;


        private string m_filePath;
        private StringBuilder m_fileContents;
        private string m_fileContentsString;
        private string m_encodingMode;


        public CrkStreamReader(string path)
        {
            m_filePath = path;

            LoadFile(out m_fileContents, out m_encodingMode);
        }

        private bool LoadFile(out StringBuilder fileContents, out string encodingMode)
        {
            try
            {
                using(FileStream istream = new FileStream(m_filePath, FileMode.Open))
                using(StreamReader reader = new StreamReader(istream))
                {
                    fileContents = new StringBuilder(reader.ReadToEnd());
                    encodingMode = reader.CurrentEncoding.WebName;
                }

                return true;
            }
            catch(Exception)
            {
                fileContents = null;
                encodingMode = null;
            }

            return false;
        }
    }
}