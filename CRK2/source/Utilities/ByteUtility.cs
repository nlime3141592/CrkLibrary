public static class ByteUtility
{
    public static byte[] DeepCopy(byte[] source)
    {
        if(source == null) return null;
        
        int n = source.Length;

        byte[] desti = new byte[n];

        for(int i = 0; i < n; i++)
            desti[i] = source[i];

        return desti;
    }

    public static bool Equals(byte[] a, byte[] b)
    {
        int la, lb;

        if(a == null && b == null) return true;
        else if(a == null || b == null) return false;

        la = a.Length;
        lb = b.Length;

        if(la != lb) return false;

        for(int i = 0; i < la; i++)
            if(a[i] != b[i]) return false;

        return true;
    }
}