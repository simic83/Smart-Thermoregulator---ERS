using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Options
{
    public class ExitOption
    {
        public bool Execute()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nDa li ste sigurni da želite da izađete? (D/N)");
            Console.ResetColor();

            var key = Console.ReadKey(true);
            return key.Key == ConsoleKey.D;
        }
    }
}