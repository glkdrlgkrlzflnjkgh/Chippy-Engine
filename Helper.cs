using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardware.Info;
using MainProg;
namespace RTX 
{
    internal class Helper
    {
        public static double GetRam() { 
           return MainTracer.MemoryUsage.GetCurrentMemoryUsageInMB();
        }
        public void NotEnoughRam() {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You need at least 8GB of RAM to run this program.");
            Console.ResetColor();
            throw new OutOfMemoryException("Not enough memory to run this program.");
        }
    }
}
