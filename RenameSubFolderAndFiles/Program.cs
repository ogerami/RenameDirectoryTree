using System;
using System.IO;
using RenameSubFolderAndFiles.Models;
using RenameSubFolderAndFiles.Services;

namespace RenameSubFolderAndFiles
{
    class Program
    {
        public static void Main(string[] args)
        {

            var arguementsConverter = new ArguementsConverter(args);
            
            ProcessDirectory(arguementsConverter.ArguementsModel);
            
            Console.WriteLine("Done");
            Console.Read();
        }
        
        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public static void ProcessDirectory(ArguementsModel arguementsModel)
        {
            // Process the list of files found in the directory.
            var fileEntries = Directory.GetFiles(arguementsModel.Path);
            foreach (var fileName in fileEntries)
                ProcessFile(new ArguementsModel
                {
                    Path = fileName,
                    ChangeFrom = arguementsModel.ChangeFrom,
                    ChangeTo = arguementsModel.ChangeTo
                });

            // Recurse into subdirectories of this directory.
            var subdirectoryEntries = Directory.GetDirectories(arguementsModel.Path);
            foreach (var subdirectory in subdirectoryEntries)
            {
                var directory = subdirectory;
                if (subdirectory.Contains(arguementsModel.ChangeFrom))
                {
                    directory = subdirectory.Replace(Path.GetFileName(subdirectory),
                        Path.GetFileName(subdirectory).Replace(arguementsModel.ChangeFrom, arguementsModel.ChangeTo));

                    Directory.Move(subdirectory, directory);
                    Console.WriteLine("Processed directory '{0}' To: {1}.", subdirectory, subdirectory.Replace(arguementsModel.ChangeFrom, arguementsModel.ChangeTo));
                }
                    
                ProcessDirectory(new ArguementsModel
                {
                    Path = directory,
                    ChangeFrom = arguementsModel.ChangeFrom,
                    ChangeTo = arguementsModel.ChangeTo
                });
            }
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(ArguementsModel arguementsModel)
        {
            if (!arguementsModel.Path.Contains(arguementsModel.ChangeFrom)) return;
            
            Directory.Move(arguementsModel.Path, arguementsModel.Path.Replace(Path.GetFileName(arguementsModel.Path), Path.GetFileName(arguementsModel.Path).Replace(arguementsModel.ChangeFrom, arguementsModel.ChangeTo)));
            Console.WriteLine("Processed file '{0}' To: {1}.", arguementsModel.Path, arguementsModel.Path.Replace(arguementsModel.ChangeFrom, arguementsModel.ChangeTo));
        }
    }
}
