using System.IO;
using System;
using MainTracer;
using System.Drawing.Imaging;
using System.Threading;

namespace MainProg
{
    public class BootStrap
    {
        static void Main(string[] args)
        {
            string OutPutName = String.Empty;
            String path = Directory.GetCurrentDirectory();
            string OutDir = Directory.CreateDirectory(path + "/OutPut").FullName;
            Console.Write("How wide will the image be? ");
            int imageWidth = int.Parse(Console.ReadLine());
            Console.Write("How tall will the image be? ");
            int imageHeight = int.Parse(Console.ReadLine());
            Console.Write("How many samples? ");
            int samplesPerPixel = int.Parse(Console.ReadLine());
            Console.Write("What should the output file be called? ");
            OutPutName = Console.ReadLine();

            using (StreamWriter writer = new StreamWriter(OutDir + "/" + OutPutName + ".ppm"))
            {
                writer.WriteLine($"P3\n{imageWidth} {imageHeight}\n255");
                Console.WriteLine($"There are: {imageWidth * imageHeight} pixels in this image");

                int totalPixels = imageWidth * imageHeight;
                int currentPixel = 0;

                for (int j = imageHeight - 1; j >= 0; j--)
                {
                    for (int i = 0; i < imageWidth; i++)
                    {
                        Vector3 color = new Vector3(0, 0, 0);
                        for (int s = 0; s < samplesPerPixel; s++)
                        {
                            float u = (float)(i + Random.Shared.NextDouble()) / (imageWidth - 1);
                            float v = (float)(j + Random.Shared.NextDouble()) / (imageHeight - 1);
                            Ray ray = new Ray(new Vector3(0, 0, 0), new Vector3(-2.0f, -1.0f, -1.0f) + u * new Vector3(4.0f, 0.0f, 0.0f) + v * new Vector3(0.0f, 2.0f, 0.0f));
                            color += Engine.RayColor(ray, new Sphere(new Vector3(0, 0, -1), 0.5f), new Sphere(new Vector3(-6f, 4f, 0f), 0.9f));
                        }
                        color /= samplesPerPixel;
                        color = new Vector3((float)Math.Sqrt(color.X), (float)Math.Sqrt(color.Y), (float)Math.Sqrt(color.Z));
                        writer.WriteLine($"{(int)(255.99f * color.X)} {(int)(255.99f * color.Y)} {(int)(255.99f * color.Z)}");

                        // 🔥 Progress Bar Logic
                        currentPixel++;
                        if (currentPixel % (totalPixels / 50) == 0) // Update every ~2% progress
                        {
                            Console.Write("\r[");
                            int progress = (int)((double)currentPixel / totalPixels * 50);
                            Console.Write(new string('#', progress));
                            Console.Write(new string('-', 50 - progress));
                            Console.Write($"] {progress * 2}%");
                        }
                    }
                }
            }
            Console.WriteLine("\nRendering complete. Output saved to 'output.ppm'.");
        }
    }
}