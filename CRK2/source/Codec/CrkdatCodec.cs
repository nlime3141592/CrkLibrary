using System;
using System.Text;

namespace CRK2
{
    public static class CrkdatCodec
    {
        public static string GetCrkdatLine(Item item)
        {
            if(item == null)
                throw new NullReferenceException("아이템 참조 정보 없음");

            StringBuilder contents;
            string str_itemType;
            int recipeCount;
            int requiresCount;
            int i;

            contents = new StringBuilder();

            // 1. 아이템 타입 병합
            str_itemType = CrkManager.itemTypeConverter.GetItemTypeString(item.itemType);
            contents.AppendFormat("{0}", str_itemType);

            // 2. 레시피 병합
            recipeCount = item.recipes.Length;

            for(i = 0; i < recipeCount; i++)
            {
                str_itemType = CrkManager.itemTypeConverter.GetItemTypeString(item.recipes[i].itemType);
                requiresCount = item.recipes[i].count;

                if(i == 0)
                    contents.Append(": ");
                else
                    contents.Append(", ");

                contents.AppendFormat("{0}/{1}", str_itemType, requiresCount);
            }

            return contents.ToString();
        }

        public static Item GetItem(string crkdatLine)
        {
            string[] split_type;
            string[] split_recipeCount;
            string[] split_recipeData;
            StringSplitOptions splitOption = StringSplitOptions.RemoveEmptyEntries;

            string str_itemType;
            int recipeCount;
            int requiresCount;
            int i;

            Item item;

            item = new Item();
            item.itemType = -1;
            item.recipes = null;

            // 0. 문자열 전처리
            crkdatLine = StringUtility.RemoveWhiteSpace(crkdatLine);

            // 1. 아이템 타입 분리
            split_type = crkdatLine.Split(':', splitOption);

            if(
                (split_type == null || split_type[0] == "") ||
                (split_type.Length > 2)
            )
            {
                throw new SyntaxException("문법 오류");
            }
            else
            {
                str_itemType = split_type[0];
                item.itemType = CrkManager.itemTypeConverter.GetItemTypeInt32(str_itemType);
            }

            // 1-1. 레시피 없이 아이템 이름만 존재하는 경우
            if(split_type.Length == 1)
            {
                if(StringUtility.Contains(split_type[0], ',', '/'))
                {
                    throw new SyntaxException("문법 오류");
                }
                
                return item;
            }

            // 2. 레시피 갯수 파악
            split_recipeCount = split_type[1].Split(',', splitOption);
            recipeCount = split_recipeCount.Length;

            if(split_recipeCount == null)
                throw new SyntaxException("문법 오류");
            else if(recipeCount == 0)
                return item;

            item.recipes = new Recipe[recipeCount];

            // 3. 개별 레시피 제작
            for(i = 0; i < recipeCount; i++)
            {
                split_recipeData = split_recipeCount[i].Split('/', splitOption);

                if(split_recipeData.Length != 2)
                    throw new SyntaxException("문법 오류");

                item.recipes[i] = new Recipe();

                str_itemType = split_recipeData[0];
                requiresCount = int.Parse(split_recipeData[1]);

                item.recipes[i].itemType = CrkManager.itemTypeConverter.GetItemTypeInt32(str_itemType);
                item.recipes[i].count = requiresCount;
            }

            // 4. 완료
            return item;
        }
    }
}