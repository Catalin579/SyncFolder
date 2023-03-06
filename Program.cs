using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Usage: folder-sync [source folder] [replica folder] [log file path]");
            return;
        }

        string sourcePath = args[0];
        string replicaPath = args[1];
        string logFilePath = args[2];

        Console.WriteLine($"Syncing {sourcePath} to {replicaPath} every 5 seconds...");

        while (true)
        {
            try
            {
                SyncFolders(sourcePath, replicaPath, logFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Thread.Sleep(5000);
        }
    }

    static void SyncFolders(string sourcePath, string replicaPath, string logFilePath)
    {
        Console.WriteLine($"Syncing {sourcePath} to {replicaPath}...");

        using (StreamWriter logFile = new StreamWriter(logFilePath, true))
        {
            logFile.WriteLine($"Syncing {sourcePath} to {replicaPath} at {DateTime.Now}");

            if (!Directory.Exists(sourcePath))
            {
                throw new Exception($"Source folder {sourcePath} does not exist");
            }

            if (!Directory.Exists(replicaPath))
            {
                Directory.CreateDirectory(replicaPath);
                Console.WriteLine($"Created replica folder at {replicaPath}");
                logFile.WriteLine($"Created replica folder at {replicaPath}");
            }

            foreach (string sourceFilePath in Directory.GetFiles(sourcePath))
            {
                string sourceFileName = Path.GetFileName(sourceFilePath);
                string replicaFilePath = Path.Combine(replicaPath, sourceFileName);

                if (File.Exists(replicaFilePath))
                {
                    byte[] sourceFileHash = GetFileHash(sourceFilePath);
                    byte[] replicaFileHash = GetFileHash(replicaFilePath);

                    if (ByteArraysAreEqual(sourceFileHash, replicaFileHash))
                    {
                        continue;
                    }
                }

                File.Copy(sourceFilePath, replicaFilePath, true);
                Console.WriteLine($"Copied {sourceFilePath} to {replicaFilePath}");
                logFile.WriteLine($"Copied {sourceFilePath} to {replicaFilePath}");
            }

            foreach (string replicaFilePath in Directory.GetFiles(replicaPath))
            {
                string replicaFileName = Path.GetFileName(replicaFilePath);
                string sourceFilePath = Path.Combine(sourcePath, replicaFileName);

                if (!File.Exists(sourceFilePath))
                {
                    File.Delete(replicaFilePath);
                    Console.WriteLine($"Deleted {replicaFilePath}");
                    logFile.WriteLine($"Deleted {replicaFilePath}");
                }
            }
        }
    }

    static byte[] GetFileHash(string filePath)
    {
        using (var md5 = MD5.Create())
        {
            using (var stream = File.OpenRead(filePath))
            {
                return md5.ComputeHash(stream);
            }
        }
    }

    static bool ByteArraysAreEqual(byte[] first, byte[] second)
    {
        if (first == null || second == null)
        {
            return false;
        }

        if (first.Length != second.Length)
        {
            return false;
        }

        for (int i = 0; i < first.Length; i++)
        {
            if (first[i] != second[i])
            {
                return false;
            }
        }

        return true;
    }
}