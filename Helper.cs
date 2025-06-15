using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using Hardware.Info;
using MainProg;

namespace RTX {
	internal class Helper {
		/// <summary>
		/// Returns the amount of RAM (in GB).
		/// </summary>
		public double GetRam() {
			using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem")) {
				foreach (ManagementObject obj in searcher.Get()) {
					return Convert.ToDouble(obj["TotalPhysicalMemory"]) / (1024 * 1024 * 1024); // Convert bytes to GB
				}
			}
			return 0;
		}

		/// <summary>
		/// The method that is called when there is not enough RAM to run the program.
		/// </summary>
		public void NotEnoughRam() {
			Console.WriteLine("Error: Not enough RAM to run the program!");
			Console.WriteLine("Consider closing unnecessary applications or upgrading your system memory.");
			Environment.Exit(1); // Exit the program with an error code
		}

		/// <summary>
		/// This method returns the CPU core count.
		/// </summary>
		public int GetCPUCoreCount() {
			return Environment.ProcessorCount;
		}
	}
}
