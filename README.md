# Internal Development in QA (VEEAM Software Test)

This program synchronizes two folders: source and replica. The
program maintains a full, identical copy of source folder at replica folder. 

## How to run this program 
    
To run this program on you PC, you must run the following comand in the Terminal:
    `SyncFolder [source folder] [replica folder] [log file path]`

##  Explanation of this program, line by line: 
- The program is written in C# language and it helps to keep two folders synchronized.
- The program uses four built-in libraries: System, System.IO, System.Security.Cryptography, and System.Threading.
- The program uses the namespace FolderSync to avoid naming conflicts with other programs.
- The main class is called Program.
- The Main method is where the program starts running.
- The Main method takes command-line arguments: sourceDir (the path to the source folder), replicaDir (the path to the replica folder), logFile (the path to the log file), and interval (the time interval between synchronization).
- The program checks if the source and replica folders exist, and if not, it creates them.
- The program initializes a file watcher for the source directory that monitors changes in the folder, such as file creation, deletion, or modification.
- The file watcher is configured to notify on changes to file names, directories, and when the last write occurs.
- The file watcher is configured to monitor all subdirectories of the source folder.
- The file watcher is enabled so that it starts monitoring for changes.
- The program initializes a timer that calls the Sync method periodically, based on the interval set in the command-line arguments.
- The Sync method is called every time the timer ticks, and it synchronizes the files in the source folder with the replica folder.
- The Sync method checks for differences between the files in the source and replica folders.
- If a file exists in the source folder but not in the replica folder, the file is copied to the replica folder.
- If a file exists in the replica folder but not in the source folder, the file is deleted from the replica folder.
- If a file exists in both folders, the Sync method compares the hashes of the two files to see if they are identical.
- If the hashes are different, the file in the replica folder is replaced with the file from the source folder.
- If the hashes are the same, no action is taken.
- The Sync method logs any changes made to the files to the log file.
- The program runs indefinitely, monitoring the source folder for changes and periodically synchronizing the files with the replica folder.