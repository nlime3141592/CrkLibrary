using System;
using System.Text;
using System.Text.RegularExpressions;

namespace CRK2
{
    public static class TypedatCodec
    {
        public static string GetTypedatLine(string str_itemType, string viewName = null)
        {
            StringBuilder contents;

            contents = new StringBuilder();

            if(StringUtility.Contains(str_itemType, ' ', ',', ':', '/'))
                throw new SyntaxException("문법 오류");
            
            contents.Append(str_itemType);

            if(viewName != null)
            {
                if(StringUtility.Contains(viewName, ' ', ',', ':', '/'))
                    throw new SyntaxException("문법 오류");

                contents.AppendFormat(": {0}", viewName);
            }

            return contents.ToString();
        }

        public static void GetItemType(string typedatLine, out string str_itemType, out string viewName)
        {
            string[] split;
            StringSplitOptions splitOption;

            splitOption = StringSplitOptions.RemoveEmptyEntries;

            // 0. 문자열 전처리
            typedatLine = StringUtility.RemoveWhiteSpace(typedatLine);

            // 1. 단락 갯수 구분
            int colonCount = Regex.Matches(typedatLine, ":").Count;

            if(colonCount > 1)
            {
                throw new SyntaxException("문법 오류");
            }
            else if(colonCount == 0)
            {
                if(StringUtility.Contains(typedatLine, ',', '/'))
                    throw new SyntaxException("문법 오류");

                str_itemType = typedatLine;
                viewName = null;
            }
            else
            {
                if(typedatLine[0] == ':' || typedatLine[typedatLine.Length - 1] == ':')
                    throw new SyntaxException("문법 오류");

                split = typedatLine.Split(':', splitOption);

                if(
                    StringUtility.Contains(split[0], ',', '/') ||
                    StringUtility.Contains(split[1], ',', '/')
                )
                {
                    throw new SyntaxException("문법 오류");
                }

                str_itemType = split[0];
                viewName = split[1];
            }
        }
    }
}