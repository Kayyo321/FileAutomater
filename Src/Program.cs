using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileAutomater
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Start();
            }
        }

        private static void Start()
        {
            Dialogue dialogue = new Dialogue();
            Program program = new Program();

            program.QuickChangeColor($"\nWelcome, {Environment.UserName}, to File Automater, made by Sully!", true, ConsoleColor.Yellow);

            program.QuickChangeColor("\nEnter the directory path : ", false, ConsoleColor.Green);

            string path = Console.ReadLine();

            dialogue.GetFiles(path);
        }

        public void QuickChangeColor(string content, bool isWriteline, ConsoleColor color)
        {
            Console.ForegroundColor = color;

            if (isWriteline)
            {
                Console.WriteLine(content);
            }
            else
            {
                Console.Write(content);
            }

            Console.ResetColor();
        }
    }

    class Dialogue
    {
        List<string> fileContentsTmp = new List<string>();
        List<string> directoryContentsTmp = new List<string>();

        string[] fileContents = { };
        string[] directoryContents = { };

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

        private string[] sendListToArray(List<string> list)
        {
            string[] tmp = { };

            tmp = list.ToArray();

            return tmp;
        }
    }
}
