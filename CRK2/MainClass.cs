using System;
using System.IO;
using System.Text;
using System.Text.Json;

#nullable enable

namespace CRK2
{
    public class MainClass
    {
        public static string ProgramPath = Directory.GetCurrentDirectory();

        static void Main(string[] args)
        {
            ItemCountTable itemCountTable;
            string finalResult;

            CrkManager.Start();

            itemCountTable = new ItemCountTable();
            itemCountTable["Focaccia"] = 1;

            finalResult = GetFinalResult(itemCountTable);

            Console.WriteLine(finalResult);
        }

        static void Pause(string message)
        {
            Console.WriteLine(message ?? "아무 키나 입력하세요..");
            Console.ReadKey();
        }

        static string GetFinalResult(ItemCountTable table)
        {
            StringBuilder resultMessage;
            int[] itemCounts;
            bool canGetNextTable;

            resultMessage = new StringBuilder();
            itemCounts = table.ToArray();

            resultMessage.AppendFormat("{0}\n", CrkManager.ItemCountsToString(itemCounts));

            do
            {
                itemCounts = CrkManager.GetRequiredItems(itemCounts, out canGetNextTable);
                resultMessage.AppendFormat("{0}\n", CrkManager.ItemCountsToString(itemCounts));
            }while(canGetNextTable);

            resultMessage.Remove(resultMessage.Length - 1, 1);
            return resultMessage.ToString();
        }
    }
}