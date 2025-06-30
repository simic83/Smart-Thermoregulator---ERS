using System;
using System.IO;
using Infrastructure;

namespace Presentation.Screens
{
    public static class LogScreen
    {
        public static void Prikazi(FileLogger logger, HeaterCycleLogger cycleLogger)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(">> Pregled logovanih događaja\n");
            Console.ResetColor();

            // Prikaz dnevnika (log.txt)
            string logPath = "log.txt";
            if (File.Exists(logPath))
            {
                string[] poslednjih20 = File.ReadAllLines(logPath);
                int start = poslednjih20.Length > 20 ? poslednjih20.Length - 20 : 0;
                for (int i = start; i < poslednjih20.Length; i++)
                {
                    if (poslednjih20[i].Contains("ERROR"))
                        Console.ForegroundColor = ConsoleColor.Red;
                    else if (poslednjih20[i].Contains("INFO"))
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else
                        Console.ForegroundColor = ConsoleColor.Gray;

                    Console.WriteLine(poslednjih20[i]);
                }
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Log fajl nije pronađen.");
                Console.ResetColor();
            }

            // Prikaz ciklusa grejača (grejac-log.txt)
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n>> Ciklusi rada grejača\n");
            Console.ResetColor();

            string cyclePath = "grejac-log.txt";
            if (File.Exists(cyclePath))
            {
                string[] poslednjih10 = File.ReadAllLines(cyclePath);
                int start = poslednjih10.Length > 10 ? poslednjih10.Length - 10 : 0;
                for (int i = start; i < poslednjih10.Length; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(poslednjih10[i]);
                }
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Nema evidentiranih ciklusa grejača.");
                Console.ResetColor();
            }

            Console.WriteLine("\nPritisnite ENTER za povratak u meni...");
            Console.ReadLine();
        }
    }
}
