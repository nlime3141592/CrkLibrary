using System.Security.Cryptography;
using System.Text;

namespace CRK2
{
    public class HashGenerator
    {
        public int HashSize => m_hashAlgorithm.HashSize;


        private static HashAlgorithm[] m_hashAlgorithms;
        private HashAlgorithm m_hashAlgorithm;


        static HashGenerator()
        {
            m_hashAlgorithms = new HashAlgorithm[3];
            m_hashAlgorithms[0] = SHA256.Create();
            m_hashAlgorithms[1] = SHA384.Create();
            m_hashAlgorithms[2] = SHA512.Create();
        }

        public HashGenerator(HashAlgorithmType type)
        {
            int _type = (int)type;

            m_hashAlgorithm = m_hashAlgorithms[_type];
        }

        public byte[] ComputeHash(string contents, Encoding encoding)
        {
            byte[] hash;

            hash = m_hashAlgorithm.ComputeHash(encoding.GetBytes(contents));

            return hash;
        }

        public byte[] ComputeHash(StringBuilder contents, Encoding encoding)
        {
            byte[] hash;

            hash = this.ComputeHash(contents.ToString(), encoding);

            return hash;
        }
    }
}