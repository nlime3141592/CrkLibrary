using System.Collections.Generic;
using System.Text;

namespace CRK2
{
    public static class StringUtility
    {
        public static string RemoveWhiteSpace(string str)
        {
            str = str.Replace(" ", "").Replace("\t", "");

            return str;
        }

        public static bool Contains(string str, params char[] delimiters)
        {
            if(delimiters == null || delimiters.Length == 0)
                return false;

            for(int i = 0; i < delimiters.Length; i++)
                if(str.Contains(delimiters[i]))
                    return true;

            return false;
        }

        public static StringBuilder RemoveWhiteSpace(StringBuilder contents)
        {
            contents.Replace(" ", "");
            contents.Replace("\t", "");

            return contents;
        }

        private static int[] GetSymmetricArray(string value)
        {
            int n;
            int i, j;
            int[] symmetricArray;

            n = value.Length;
            j = 0;
            symmetricArray = new int[n];

            symmetricArray[0] = 0;

            for(i = 1; i < n; i++)
            {
                while(j > 0 && value[i] != value[j])
                    j = symmetricArray[j - 1];

                if(value[i] == value[j])
                    symmetricArray[j] = ++j;
            }

            return symmetricArray;
        }

        public static int[] KMP(string source, string value)
        {
            int i, j;
            int n, m;
            int[] symmetricArray;
            List<int> kmpList;

            j = 0;
            n = source.Length;
            m = value.Length;
            symmetricArray = GetSymmetricArray(value);
            kmpList = new List<int>();

            for(i = 0; i < n; i++)
            {
                while(j > 0 && source[i] != value[j])
                    j = symmetricArray[j - 1];

                if(source[i] == value[j])
                {
                    if(j == m - 1)
                    {
                        kmpList.Add(i - m + 1);
                        j = symmetricArray[j];
                    }
                    else
                    {
                        j++;
                    }
                }
            }

            if(kmpList.Count > 0) return kmpList.ToArray();
            else return null;
        }
    }
}