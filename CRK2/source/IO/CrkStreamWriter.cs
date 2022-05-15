using System;
using System.IO;
using System.Text;
 
namespace CRK2
{
    public class CrkStreamWriter
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
        private Encoding m_encoding;


        public CrkStreamWriter(string path, string encodingMode)
        {
            m_filePath = path;
            m_encodingMode = encodingMode;
            m_encoding = Encoding.GetEncoding(encodingMode);
        }

        public bool SaveFile(StringBuilder contents)
        {
            try
            {
                using(FileStream ostream = new FileStream(m_filePath, FileMode.Create))
                using(StreamWriter writer = new StreamWriter(ostream, m_encoding))
                {
                    writer.Write(contents);

                    m_fileContents = contents;
                    m_fileContentsString = contents.ToString();

                    return true;
                }
            }
            catch(Exception)
            {
                m_fileContents = null;
                m_fileContentsString = null;
            }

            return false;
        }
    }
}