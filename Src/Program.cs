using System;

namespace FileAutomater
{
    /// <summary>
    /// This class holds the Main function for the application, and is what holds all the code to start running the program.
    /// </summary>
    class Program
    {
        /// <summary>
        /// This function starts the application, and makes sure to ask, and always ask for a new directory, when completed.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Program program = new Program();

            program.QuickChangeColor($"\nWelcome, {Environment.UserName}, to File Automater, made by Sully!", true, ConsoleColor.Yellow);

            while (true)
            {
                Start();
            }
        }

        /// <summary>
        /// This function holds the code that gets the path required, and runs the rest of the application.
        /// </summary>
        private static void Start()
        {
            Dialogue dialogue = new Dialogue();
            Program program = new Program();

            program.QuickChangeColor("\nEnter a new directory/path : ", false, ConsoleColor.Green);

            string path = Console.ReadLine();

            dialogue.GetFiles(path);
        }

        /// <summary>
        /// This function is a sort of 'Second Console.Writeline', which allows me to do one line color changes much more easily.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="isWriteline"></param>
        /// <param name="color"></param>
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
