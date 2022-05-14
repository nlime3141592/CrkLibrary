using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CRK2
{
    public abstract class SerializerBase
    {
        public abstract bool Start();
        
        protected abstract StringBuilder LoadFile(string path);

        protected byte[] GetHash(StringBuilder fileContents)
        {
            SHA256 hashCreator;
            Encoding encoder_UNICODE;
            byte[] hash;

            hashCreator = SHA256.Create();
            encoder_UNICODE = Encoding.Unicode;
            hash = hashCreator.ComputeHash(encoder_UNICODE.GetBytes(fileContents.ToString()));

            return hash;
        }

        protected void SaveHash(string path, byte[] hash)
        {
            try
            {
                using(FileStream ostream = new FileStream(path, FileMode.Create))
                using(BinaryWriter writer = new BinaryWriter(ostream))
                {
                    writer.Write(hash);
                }
            }
            catch(Exception)
            {

            }
        }

        protected byte[] LoadHashFile(string path)
        {
            byte[] hash;

            try
            {
                using(FileStream istream = new FileStream(path, FileMode.Open))
                using(BinaryReader reader = new BinaryReader(istream))
                {
                    hash = reader.ReadBytes((int)istream.Length);
                }

                return hash;
            }
            catch(Exception)
            {
                return null;
            }
        }

        protected bool VerificateHash(byte[] hash1, byte[] hash2)
        {
            int i;
            int n, m;

            if(hash1 == null || hash2 == null) return false;

            n = hash1.Length;
            m = hash2.Length;

            if(n != m) return false;

            for(i = 0; i < n; i++)
                if(hash1[i] != hash2[i]) return false;

            return true;
        }
    }
}