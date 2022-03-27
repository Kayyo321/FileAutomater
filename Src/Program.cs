using System;

namespace FileAutomater
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();

            program.QuickChangeColor($"\nWelcome, {Environment.UserName}, to File Automater, made by Sully!", true, ConsoleColor.Yellow);

            while (true)
            {
                Start();
            }
        }

        private static void Start()
        {
            Dialogue dialogue = new Dialogue();
            Program program = new Program();

            program.QuickChangeColor("\nEnter a new directory/path : ", false, ConsoleColor.Green);

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
}
