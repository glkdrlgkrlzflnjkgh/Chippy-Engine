﻿using System.IO;
using System;
using Engine;
using System.Drawing.Imaging;
using System.Threading;
using RTX;
using Aardvark.OpenImageDenoise;
using System.Runtime.InteropServices;
namespace MainProg
{
	
	public class BootStrap
    {	 
		public static int samplesPerPixel = 0;
        static void Main(string[] args)
        {
			Helper helper = new Helper(); // This is used to access the helper methods.
			Console.WriteLine(helper.GetRam());
			if (helper.GetRam() < 8) {
				helper.NotEnoughRam(); // If there is not enough RAM, this method will be called and the program will exit.
			}
			string OutPutName = String.Empty;
			Aardvark.Base.Aardvark.Init(); // Initialize Aardvark for OpenImageDenoise
			Aardvark.OpenImageDenoise.Device device = new Device(DeviceFlags.Default,12); // Create the default device for OpenImageDenoise
			 


			String path = Directory.GetCurrentDirectory();
            string OutDir = Directory.CreateDirectory(path + "/OutPut").FullName;
            Console.Write("How wide will the image be? ");
            int imageWidth = int.Parse(Console.ReadLine());
            Console.Write("How tall will the image be? ");
            int imageHeight = int.Parse(Console.ReadLine());
           
            SetSamples();
            void SetSamples() {

                try
                {
                    Console.Write("How many samples? ");
                    int In = int.Parse(Console.ReadLine());
                    if (In < 1) { 
                        SampleSetError();
                    }
                    samplesPerPixel = In;
                }
                catch (FormatException e)
                {   Console.WriteLine("oh crap! "+e.Message);
                    SampleSetError();
                }
            }   
            void SampleSetError()
            {
                Console.WriteLine("Invalid input!");
                SetSamples(); // sets the sample count for rendering
            }
            Console.Write("What should the output file be called? ");
            OutPutName = Console.ReadLine();
            if (imageWidth is 1 || imageHeight is 1 || imageWidth is < 1 || imageHeight is < 1) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Width and height must be greater than 1.");
                Console.ResetColor();
                return;
            }
			try {
				using (StreamWriter writer = new StreamWriter(OutDir + "/" + OutPutName + ".ppm")) {
					int PixCount = imageWidth * imageHeight;
					writer.WriteLine($"P3\n{imageWidth} {imageHeight}\n255");
					Console.WriteLine($"There are: {PixCount} pixels in this image");
					Console.WriteLine($"There will be: {PixCount * samplesPerPixel} samples in total");
					int totalPixels = imageWidth * imageHeight;
					int currentPixel = 0;

					for (int j = imageHeight - 1; j >= 0; j--) {
						for (int i = 0; i < imageWidth; i++) {
							Vector3 color = new Vector3(0, 0, 0);
							for (int s = 0; s < samplesPerPixel; s++) {
								float u = (float)(i + Random.Shared.NextDouble()) / (imageWidth - 1);
								float v = (float)(j + Random.Shared.NextDouble()) / (imageHeight - 1);
								Ray ray = new Ray(new Vector3(0, 0, 0), new Vector3(-2.0f, -1.0f, -1.0f) + u * new Vector3(4.0f, 0.0f, 0.0f) + v * new Vector3(0.0f, 2.0f, 0.0f));
								color += Engine.Engine.RayColor(ray, new Sphere(new Vector3(0, 0, -1), 0.5f), new Sphere(new Vector3(0f, 2f, 0f), 0.9f));
							}
							color /= samplesPerPixel;
							color = new Vector3((float)Math.Sqrt(color.X), (float)Math.Sqrt(color.Y), (float)Math.Sqrt(color.Z));
							writer.WriteLine($"{(int)(255.99f * color.X)} {(int)(255.99f * color.Y)} {(int)(255.99f * color.Z)}");

							// 🔥 Progress Bar Logic
							currentPixel++;
							if (currentPixel % (totalPixels / 100) == 0) // Update every ~1% progress
							{
								Console.Write("\r Rendering...");
								int progress = (int)((double)currentPixel / totalPixels * 100);

								Console.Write($" {progress}%");
							}
						}
					}
				}
			}
			catch (Exception e) { 
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"Error Rendering: {e.Message}");
				Console.ResetColor();
			}
			Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nDONE! Output saved to '{OutPutName}.ppm'.");
			
			Console.ResetColor();
        }
    }
}