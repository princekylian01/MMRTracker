using System;
using mmrcalc.Helpers;
using mmrcalc.IO;
using mmrcalc.Models;
using System.Collections.Generic;
using System.Linq;

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

            ShowRankInfo(currentMMR);

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

        private void ShowRankInfo(int mmr)
        {
            var ranks = new List<(string Name, int Min, int Max)>
            {
                ("Herald 1", 0, 149), ("Herald 2", 150, 299), ("Herald 3", 300, 449), ("Herald 4", 450, 609), ("Herald 5", 610, 769),
                ("Guardian 1", 770, 919), ("Guardian 2", 920, 1079), ("Guardian 3", 1080, 1229), ("Guardian 4", 1230, 1399), ("Guardian 5", 1400, 1539),
                ("Crusader 1", 1540, 1699), ("Crusader 2", 1700, 1849), ("Crusader 3", 1850, 1999), ("Crusader 4", 2000, 2149), ("Crusader 5", 2150, 2309),
                ("Archon 1", 2310, 2449), ("Archon 2", 2450, 2609), ("Archon 3", 2610, 2769), ("Archon 4", 2770, 2929), ("Archon 5", 2930, 3079),
                ("Legend 1", 3080, 3229), ("Legend 2", 3230, 3389), ("Legend 3", 3390, 3539), ("Legend 4", 3540, 3699), ("Legend 5", 3700, 3849),
                ("Ancient 1", 3850, 3999), ("Ancient 2", 4000, 4149), ("Ancient 3", 4150, 4299), ("Ancient 4", 4300, 4459), ("Ancient 5", 4460, 4619),
                ("Divine 1", 4620, 4819), ("Divine 2", 4820, 5019), ("Divine 3", 5020, 5219), ("Divine 4", 5220, 5419), ("Divine 5", 5420, 5619),
                ("Titan", 5620, int.MaxValue)
            };

            var rank = ranks.FirstOrDefault(r => mmr >= r.Min && mmr <= r.Max);
            int progress = (int)(((mmr - rank.Min) / (float)(rank.Max - rank.Min)) * 100);

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"\nRank: {rank.Name} ({progress}%)");
            Console.ResetColor();
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