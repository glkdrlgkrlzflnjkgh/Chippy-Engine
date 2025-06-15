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
        /// <summary>
        /// returns the amount of RAM 
        /// </summary>
    
        public static double GetRam() { 
           return Engine.MemoryUsage.GetCurrentMemoryUsageInMB();
        }
        /// <summary>
        /// The method that is called when there is not enough RAM to run the program.
        /// </summary>
        public void NotEnoughRam() {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You need at least 8GB of RAM to run this program.");
            Console.ResetColor();
            throw new OutOfMemoryException("Not enough memory to run this program.");
        }
        /// <summary>
        /// This method returns the CPU core count.
        /// </summary>
        public static int GetCPUCoreCount() {
            int coreCount = Environment.ProcessorCount;
            return coreCount;
            
        }
    }
}
