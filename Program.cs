using System;
using mmrcalc.Helpers;
using mmrcalc.IO;
using mmrcalc.Services;

namespace mmrcalc
{
    static class Program
    {
        static void Main()
        {
            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Console.Title = "F01000 // MMR Tracker v0.2";

                var fileManager = new FileManager();
                var mmrService = new MmrService(fileManager);

                bool running = true;

                while (running)
                {
                    PrintTitle();
                    PrintMenu();

                    string? choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1": mmrService.RunSession(); break;
                        case "2": mmrService.ShowLog(); break;
                        case "3": mmrService.ResetStartMMR(); break;
                        case "0": running = false; break;
                        default:
                            ConsoleHelper.WriteLineColor("Invalid command", ConsoleColor.Red);
                            ConsoleHelper.Pause();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nERROR: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ResetColor();
                Console.ReadKey();
            }
        }

        static void PrintTitle()
        {
            Console.Clear();
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("                                                                   ");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("        ▄████████  ▄██████▄  ▄█   ▄███████▄   ▄██████▄   ▄██████▄  ");
            Console.WriteLine("       ███    ███ ███    ███ ███ ▄██▀▀▀▀██▄ ███    ███ ███    ███ ");
            Console.WriteLine("       ███    █▀  ███    ███ ███ ███    ███ ███    ███ ███    ███ ");
            Console.WriteLine("      ▄███▄▄▄     ███    ███ ███ ███    ███ ███    ███ ███    ███ ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("     ▀▀███▀▀▀     ███    ███ ███ ███    ███ ███    ███ ███    ███ ");
            Console.WriteLine("       ███        ███    ███ ███ ███    ███ ███    ███ ███    ███ ");
            Console.WriteLine("       ███        ███    ███ ███ ███    ███ ███    ███ ███    ███ ");
            Console.WriteLine("       ███         ▀██████▀  █▀  ▀██████▀   ▀██████▀   ▀██████▀  ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("               F01000 // MMR Tracker v0.2");
            Console.ResetColor();
        }

        static void PrintMenu()
        {
            Console.WriteLine();

            ConsoleHelper.WriteColor("  ╭─ ", ConsoleColor.DarkGray);
            ConsoleHelper.WriteColor("[1]", ConsoleColor.Green);
            ConsoleHelper.WriteColor(" ▶ ", ConsoleColor.DarkGray);
            Console.WriteLine("New session");

            ConsoleHelper.WriteColor("  ├─ ", ConsoleColor.DarkGray);
            ConsoleHelper.WriteColor("[2]", ConsoleColor.Yellow);
            ConsoleHelper.WriteColor(" 📄 ", ConsoleColor.DarkGray);
            Console.WriteLine("View log");

            ConsoleHelper.WriteColor("  ├─ ", ConsoleColor.DarkGray);
            ConsoleHelper.WriteColor("[3]", ConsoleColor.Blue);
            ConsoleHelper.WriteColor(" 🔁 ", ConsoleColor.DarkGray);
            Console.WriteLine("Reset Start MMR");

            ConsoleHelper.WriteColor("  ╰─ ", ConsoleColor.DarkGray);
            ConsoleHelper.WriteColor("[0]", ConsoleColor.Red);
            ConsoleHelper.WriteColor(" ⏻ ", ConsoleColor.DarkGray);
            Console.WriteLine("Exit");

            ConsoleHelper.DrawSeparator('─', 42);
            ConsoleHelper.WriteColor(">>> Enter command: ", ConsoleColor.Cyan);
        }
    }
}