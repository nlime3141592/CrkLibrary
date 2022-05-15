using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable enable

namespace CRK2
{
    public class MainClass
    {
        public static string ProgramPath = Directory.GetCurrentDirectory();

        static void Main(string[] args)
        {
            CrkManager.Start();

            List<ItemCountTable> tables = new List<ItemCountTable>();

            ItemCountTable table = new ItemCountTable();
            tables.Add(table);

            bool canGetNext = false;
            int i = 0;

            // NOTE: 테스트 테이블 생성. 키 타입은 CRK2/dats/programDatas/types.txt 참조.
            table["Focaccia"] = 3;

            // 연산
            do
            {
                tables.Add(CrkManager.Calculate(tables[i++], out canGetNext));
            }while(canGetNext);

            // 출력
            for(i = tables.Count - 1; i >= 0; i--)
            {
                Console.WriteLine("{0}", tables[i].ToString());
            }

            Pause("종료하려면 아무 키나 입력하세요.");
        }

        static void Pause(string? message = null)
        {
            Console.WriteLine(message ?? "아무 키나 입력하세요..");
            Console.ReadKey();
        }
    }
}