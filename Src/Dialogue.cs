using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileAutomater
{
    /// <summary>
    /// This class holds all of the meat and potatoes to make the application do commands that it's ment to do.
    /// </summary>
    class Dialogue
    {
        List<string> fileContentsTmp = new List<string>();
        List<string> directoryContentsTmp = new List<string>();

        string[] fileContents = { };
        string[] directoryContents = { };

        /// <summary>
        /// This function makes sure that the path given is correct, and loads every file/directory from it.
        /// </summary>
        /// <param name="path"></param>
        public void GetFiles(string path)
        {
            Program program = new Program();

            string[] tmp = { };
            string[] temp = { };

            try
            {
                tmp = Directory.GetFiles(@path);
                temp = Directory.GetDirectories(@path);
            }
            catch (Exception exc)
            {
                Console.Error.WriteLine(exc);
            }

            fileContentsTmp = tmp.ToList();
            directoryContentsTmp = temp.ToList();

            fileContents = sendListToArray(fileContentsTmp);
            directoryContents = sendListToArray(directoryContentsTmp);

            foreach (String line in fileContents)
            {
                Console.WriteLine();
                AskAboutFile(program, line);
            }

            foreach (String line in directoryContents)
            {
                Console.WriteLine();
                AskAboutDirectory(program, line);
            }
        }

        /// <summary>
        /// This function, which is called for each directory in the givin path, asks the user what they want to do with each directory.
        /// </summary>
        /// <param name="program"></param>
        /// <param name="line"></param>
        private void AskAboutDirectory(Program program, string line)
        {
            program.QuickChangeColor($"File Directory at -> {line}", true, ConsoleColor.Cyan);

            bool toRename = Interupt("Would you like to rename this file?");

            if (toRename)
            {
                program.QuickChangeColor("\n  -> Name cannot contain : [ <, >, :, \", /, \\, |, ?, * ]", true, ConsoleColor.DarkMagenta);

                RenameDirectory(program, line);
            }

            bool toDelete = Interupt("Would you like to delete this file?");

            if (toDelete)
            {
                try
                {
                    File.Delete(line);
                    Console.WriteLine($"Deleting '{line}'...");
                }
                catch (Exception exception)
                {
                    program.QuickChangeColor($"{exception}", true, ConsoleColor.Red);
                }
            }
        }

        /// <summary>
        /// This function holds the code that renames directories, when the command is given
        /// </summary>
        /// <param name="program"></param>
        /// <param name="line"></param>
        private void RenameDirectory(Program program, string line)
        {
            Console.Write($"\n     -> Put the output path here ({line}) -> : ");
            string reply = Console.ReadLine();

            bool exists = Directory.Exists(line);

            if (exists)
            {
                Directory.Move(line, reply);
            }
            else
            {
                Console.Error.WriteLine("This directory does not exist");
            }
        }

        /// <summary>
        /// This function, which is called for each file in the givin path, asks the user what they want to do with each file.
        /// </summary>
        /// <param name="program"></param>
        /// <param name="line"></param>
        private void AskAboutFile(Program program, string line)
        {
            long fileSize = new FileInfo(line).Length;
            program.QuickChangeColor($"File Found at -> {line} ({fileSize} bytes)", true, ConsoleColor.Cyan);

            bool askToDelete = true;
            bool toOpen   = Interupt("Would you like to see the contents of this file, before you make any decisions about it? [This will not work if file is not a text document]");

            if (toOpen)
            {
                try
                {
                    if (fileSize < 500000)
                        Console.WriteLine($"    -> File Contents : \" {File.ReadAllText(line)} \"");
                    else
                        program.QuickChangeColor($"\nFile was too large to open; most likely not text-based. ({fileSize} bytes)", true, ConsoleColor.DarkRed);
                }
                catch (Exception exception)
                {
                    program.QuickChangeColor($"{exception}", true, ConsoleColor.Red);
                }
            }

            bool toRename = Interupt("Would you like to rename this file?");

            if (toRename)
            {
                program.QuickChangeColor("\n  -> Name cannot contain : [ <, >, :, \", /, \\, |, ?, * ]", true, ConsoleColor.DarkMagenta);
                
                RenameFile(program, line);

                askToDelete = false;
            }

            bool toDelete = false;
            
            if (askToDelete)
                Interupt("Would you like to delete this file?");

            bool isSure     = false;
            
            if (toDelete) 
                isSure = Interupt("Are you sure you want to delete this file?");

            if (toDelete && isSure)
            {
                try
                {
                    File.Delete(line);
                }
                catch (Exception exception)
                {
                    program.QuickChangeColor($"{exception}", true, ConsoleColor.Red);
                }
            }
        }

        /// <summary>
        /// This function holds the code that renames files.
        /// </summary>
        /// <param name="program"></param>
        /// <param name="line"></param>
        private void RenameFile(Program program, string line)
        {
            Console.Write($"\n     -> Put the output path here ({line}) -> : ");
            string outPath = Console.ReadLine();

            try
            {
                File.Move(line, outPath);
                File.Delete(line);
            }
            catch (Exception exception)
            {
                program.QuickChangeColor($"{exception}", true, ConsoleColor.Red);
            }
        }

        /// <summary>
        /// This function interupts the user with a question about something. (i.e. do you want to delete this file?)
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private bool Interupt(string content)
        {
            Console.Write($"\n{content} (y/n/c&n) : ");

            string reply = Console.ReadLine();

            bool returnType = false;

            if (reply.ToLower().Contains("y"))
            {
                returnType = true;
            }
            else if (reply.ToLower().Contains("n"))
            {
                returnType = false;
            }
            else if (reply.ToLower().Contains("c&n"))
            {
                Console.Clear();
            }

            return returnType;
        }

        /// <summary>
        /// This function makes sure that the path is turned into a string array for the application to process.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string[] sendListToArray(List<string> list)
        {
            string[] tmp = { };

            tmp = list.ToArray();

            return tmp;
        }
    }
}
