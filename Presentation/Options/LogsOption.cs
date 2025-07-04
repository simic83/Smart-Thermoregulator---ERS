using System;
using System.IO;
using System.Linq;

namespace Presentation.Options
{
    public class LogsOption
    {
        public void Execute()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    PREGLED LOGOVA                             ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            try
            {
                if (File.Exists("thermoregulator_log.txt"))
                {
                    var lines = File.ReadAllLines("thermoregulator_log.txt");
                    var lastLines = lines.TakeLast(30).ToArray();

                    Console.WriteLine($"\nPoslednja {lastLines.Length} događaja:\n");

                    foreach (var line in lastLines)
                    {
                        ColorizeLogLine(line);
                        Console.WriteLine(line);
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.WriteLine("\nLog fajl još nije kreiran.");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nGreška pri čitanju log fajla: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPritisnite bilo koji taster za povratak...");
            Console.ReadKey();
        }

        private void ColorizeLogLine(string line)
        {
            if (line.Contains("ERROR"))
                Console.ForegroundColor = ConsoleColor.Red;
            else if (line.Contains("HEAT-CYCLE"))
                Console.ForegroundColor = ConsoleColor.Magenta;
            else if (line.Contains("HEATER"))
                Console.ForegroundColor = ConsoleColor.Yellow;
            else if (line.Contains("TEMP"))
                Console.ForegroundColor = ConsoleColor.Cyan;
            else
                Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
