using System;
using System.Text.Json;

namespace CRK2
{
    public static class JsondatCodec
    {
        public static string GetJsondatLine(Item item)
        {
            if(item == null)
                throw new NullReferenceException("아이템 참조 정보 없음");

            return JsonSerializer.Serialize(item, typeof(Item));
        }

        public static Item GetItem(string jsondatLine)
        {
            try
            {
                jsondatLine = StringUtility.RemoveWhiteSpace(jsondatLine);
                Item item = (Item)JsonSerializer.Deserialize(jsondatLine, typeof(Item));

                return item;
            }
            catch(Exception)
            {
                throw new SyntaxException("문법 오류");
            }
        }
    }
}