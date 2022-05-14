using System;
using System.Collections.Generic;

#nullable enable

namespace CRK2
{
    public class Item
    {
        public int itemType { get; set; }
        public Recipe[]? recipes { get; set; }

        public void Print()
        {
            Console.WriteLine("Item: {0}", itemType);
            Console.WriteLine("  Recipes:");

            foreach(Recipe r in recipes)
            {
                r.Print();
            }
        }
    }

    public struct Recipe
    {
        public int itemType { get; set; }
        public int count { get; set ;}

        public void Print()
        {
            Console.WriteLine("    {0}: {1}", itemType, count);
        }
    }
}