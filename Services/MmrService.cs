using System;
using mmrcalc.Helpers;
using mmrcalc.IO;
using mmrcalc.Models;

namespace mmrcalc.Services
{
    public class MmrService
    {
        private readonly FileManager _fileManager;
        private readonly MmrSession _session;

        public MmrService(FileManager fileManager)
        {
            _fileManager = fileManager ?? throw new ArgumentNullException(nameof(fileManager));
            _session = new MmrSession
            {
                StartMMR = _fileManager.LoadStartMMR()
            };
        }

        public void RunSession()
        {
            const int TitanMMR = 5620;
            const int AvgGain = 25;

            _session.StartMMR = _fileManager.LoadStartMMR();

            Console.WriteLine($"Start MMR: {_session.StartMMR}");
            Console.Write("Enter your current MMR: ");

            int currentMMR;
            while (!int.TryParse(Console.ReadLine(), out currentMMR))
            {
                Console.Write("Invalid input. Enter a number: ");
            }

            _session.CurrentMMR = currentMMR;

            Console.WriteLine($"\nYou gained: {_session.Gained} MMR");

            int mmrLeft = TitanMMR - currentMMR;
            int gamesLeft = (int)Math.Ceiling(Math.Abs(mmrLeft) / (double)AvgGain);

            if (mmrLeft > 0)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\nTo Titan: {mmrLeft} MMR ≈ {gamesLeft} games");
            }
            else if (mmrLeft < 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"\nAbove Titan: {Math.Abs(mmrLeft)} MMR ≈ {gamesLeft} games ago");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nYou are exactly at Titan!");
            }

            Console.ResetColor();

            _fileManager.AppendLog($"MMR: {_session.StartMMR} → {_session.CurrentMMR} (Δ{_session.Gained})");
            _fileManager.SaveStartMMR(_session.CurrentMMR);
            ConsoleHelper.Pause();
        }

        public void ShowLog()
        {
            Console.WriteLine("\n=== Last 5 matches ===");

            string[] lines = _fileManager.ReadLog();

            if (lines.Length == 0)
            {
                Console.WriteLine("Log is empty.");
            }
            else
            {
                var lastLines = lines.Reverse().Take(5).Reverse();

                foreach (string line in lastLines)
                {
                    var match = System.Text.RegularExpressions.Regex.Match(line, @"\(Δ(-?\d+)\)");
                    if (match.Success && int.TryParse(match.Groups[1].Value, out int delta))
                    {
                        int deltaIndex = line.IndexOf("(Δ");
                        string prefix = line.Substring(0, deltaIndex);
                        string deltaText = match.Value;

                        Console.Write(prefix);

                        if (delta > 0)
                        {
                            ConsoleHelper.WriteColor(" ↑ ", ConsoleColor.Green);
                            ConsoleHelper.WriteLineColor(deltaText, ConsoleColor.Green);
                        }
                        else if (delta < 0)
                        {
                            ConsoleHelper.WriteColor(" ↓ ", ConsoleColor.Red);
                            ConsoleHelper.WriteLineColor(deltaText, ConsoleColor.Red);
                        }
                        else
                        {
                            Console.WriteLine(" → " + deltaText);
                        }
                    }
                    else
                    {
                        Console.WriteLine(line);
                    }
                }
            }

            ConsoleHelper.Pause();
        }

        public void ResetStartMMR()
        {
            Console.Write("Enter new Start MMR: ");
            if (int.TryParse(Console.ReadLine(), out int newVal))
            {
                _session.StartMMR = newVal;
                _fileManager.SaveStartMMR(newVal);
                Console.WriteLine("Start MMR updated.");
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}
