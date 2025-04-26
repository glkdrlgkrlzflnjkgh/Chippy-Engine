using System.IO;
using System;
using MainTracer;
namespace MainProg {
    public class BootStrap {
        static void Main(string[] args)
        {
            int imageWidth = 800;
            int imageHeight = 400;
            int samplesPerPixel = int.Parse(Console.ReadLine()); // Number of samples per pixel for better quality
            Vector3 lowerLeftCorner = new Vector3(-2.0f, -1.0f, -1.0f);
            Vector3 horizontal = new Vector3(4.0f, 0.0f, 0.0f);
            Vector3 vertical = new Vector3(0.0f, 2.0f, 0.0f);
            Vector3 origin = new Vector3(0.0f, 0.0f, 0.0f);

            Sphere sphere1 = new Sphere(new Vector3(0, 0, -1), 0.5f);
            Sphere sphere2 = new Sphere(new Vector3(-6f, 4f, 0f), 0.9f);

            using (StreamWriter writer = new StreamWriter("output.ppm"))
            {
                writer.WriteLine($"P3\n{imageWidth} {imageHeight}\n255");

                for (int j = imageHeight - 1; j >= 0; j--)
                {
                    for (int i = 0; i < imageWidth; i++)
                    {
                        Vector3 color = new Vector3(0, 0, 0);
                        for (int s = 0; s < samplesPerPixel; s++)
                        {
                            float u = (float)(i + Random.Shared.NextDouble()) / (imageWidth - 1);
                            float v = (float)(j + Random.Shared.NextDouble()) / (imageHeight - 1);
                            Ray ray = new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical - origin);
                            color += MainTracer.Engine.RayColor(ray, sphere1, sphere2);
                        }
                        color /= samplesPerPixel;
                        color = new Vector3((float)Math.Sqrt(color.X), (float)Math.Sqrt(color.Y), (float)Math.Sqrt(color.Z)); // Gamma correction
                        writer.WriteLine($"{(int)(255.99f * color.X)} {(int)(255.99f * color.Y)} {(int)(255.99f * color.Z)}");
                    }
                }
            }
            Console.WriteLine("Rendering complete. Output saved to 'output.ppm'.");
        }

    }


}