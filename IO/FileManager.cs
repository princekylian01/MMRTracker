using System;
using System.IO;

namespace mmrcalc.IO
{
    public class FileManager
    {
        public string UserDataPath { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "userdata.txt");
        public string LogPath { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "log.txt");

        public FileManager()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(LogPath)!);
        }

        public int LoadStartMMR()
        {
            if (File.Exists(UserDataPath))
            {
                string content = File.ReadAllText(UserDataPath);
                if (int.TryParse(content, out int mmr))
                    return mmr;
            }

            return 0;
        }

        public void SaveStartMMR(int mmr)
        {
            File.WriteAllText(UserDataPath, mmr.ToString());
        }

        public void AppendLog(string message)
        {
            File.AppendAllText(LogPath, $"{DateTime.Now:G} — {message}{Environment.NewLine}");
        }

        public string[] ReadLog() => File.Exists(LogPath) ? File.ReadAllLines(LogPath) : Array.Empty<string>();
    }
}
