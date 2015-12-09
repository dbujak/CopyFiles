using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Permissions;

namespace CopyFiles
{
    class Program
    {
        private static string destinationPath;

        static void Main(string[] args)
        {
            Run();
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void Run()
        {
            string[] args = System.Environment.GetCommandLineArgs();

            // If source and destination directories are not specified, exit program.
            if (args.Length != 3)
            {
                // Display the proper way to call the program.
                Console.WriteLine("Usage: CopyFiles.exe (sourceDirectory) (destinationDirectory)");
                return;
            }

            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = args[1];
            destinationPath = args[2];
            /* Watch for new files */
            watcher.NotifyFilter = NotifyFilters.FileName;

            // Add event handlers.
            watcher.Created += new FileSystemEventHandler(OnChanged);

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            // Wait for the user to quit the program.
            Console.WriteLine("Press \'q\' to quit the sample.");
            while (Console.Read() != 'q') ;
        }

        // Define the event handler.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            System.Threading.Thread.Sleep(1000); // need this stupid pause, otherwise file is locked somehow

            try
            {
                FileInfo file = new FileInfo(e.FullPath);

                file.MoveTo(destinationPath + @"\" + e.Name);

                Console.WriteLine(e.FullPath + " Moved");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error with " + e.Name + " - " + ex.Message);
                Console.WriteLine();
            }
              

        }

    }
}
