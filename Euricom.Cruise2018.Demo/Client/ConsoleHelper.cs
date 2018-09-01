using System.Collections.Generic;
using System.Text;
using System;

namespace Euricom.Cruise2018.Demo.Client
{
    internal class ConsoleHelper
    {
        private static readonly ConsoleColor _defaultColor = ConsoleColor.White;

        internal static void BuildMenu(string menuInstructions, Dictionary<int, string> menuDictionary)
        {
            ConsoleHelper.ChangeConsoleColor(ConsoleColor.Green);
            Console.WriteLine(menuInstructions);
            foreach (var pair in menuDictionary)
            {
                Console.WriteLine(string.Format("{0} - {1}", pair.Key, pair.Value));
            }
            Console.WriteLine();
            ConsoleHelper.ChangeConsoleColor(_defaultColor);
        }

        internal static void ChangeConsoleColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }
    }
}
