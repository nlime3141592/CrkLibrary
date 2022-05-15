namespace CRK2
{
    public static class Verification
    {
        public static bool CheckHash(byte[] hash_a, byte[] hash_b)
        {
            return ByteUtility.Equals(hash_a, hash_b);
        }
    }
}