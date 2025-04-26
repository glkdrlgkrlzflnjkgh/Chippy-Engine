using System;
using System.IO;
using System.Diagnostics;
using System.Drawing.Imaging;
namespace MainTracer
{
    public static class MemoryUsage
    {
        public static double GetCurrentMemoryUsageInMB()
        {
            Process currentProcess = Process.GetCurrentProcess();
            long memorySize = currentProcess.WorkingSet64;
            Console.WriteLine(memorySize / (1024.0 * 1024.0));
            return memorySize / (1024.0 * 1024.0);
        }
    }

    public class Vector3
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vector3 operator *(Vector3 a, float t) => new Vector3(a.X * t, a.Y * t, a.Z * t);
        public static Vector3 operator *(float t, Vector3 a) => new Vector3(a.X * t, a.Y * t, a.Z * t);
        public static Vector3 operator /(Vector3 a, float t) => new Vector3(a.X / t, a.Y / t, a.Z / t);

        public float Dot(Vector3 v) => X * v.X + Y * v.Y + Z * v.Z;

        public Vector3 Normalize()
        {
            float length = (float)Math.Sqrt(X * X + Y * Y + Z * Z);
            return new Vector3(X / length, Y / length, Z / length);
        }

        public Vector3 Reflect(Vector3 normal)
        {
            return this - 2 * this.Dot(normal) * normal;
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }

    public class Ray
    {
        public Vector3 Origin { get; }
        public Vector3 Direction { get; }

        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public Vector3 At(float t)
        {
            return Origin + t * Direction;
        }

        public override string ToString()
        {
            return $"Ray(Origin={Origin}, Direction={Direction})";
        }
    }

    public class Sphere
    {
        public Vector3 Center { get; }
        public float Radius { get; }

        public Sphere(Vector3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public bool Hit(Ray ray, float tMin, float tMax, out HitRecord rec)
        {
            rec = new HitRecord();
            Vector3 oc = ray.Origin - Center;
            float a = ray.Direction.Dot(ray.Direction);
            float b = oc.Dot(ray.Direction);
            float c = oc.Dot(oc) - Radius * Radius;
            float discriminant = b * b - a * c;

            if (discriminant > 0)
            {
                float temp = (-b - (float)Math.Sqrt(discriminant)) / a;
                if (temp < tMax && temp > tMin)
                {
                    rec.T = temp;
                    rec.P = ray.At(rec.T);
                    rec.Normal = (rec.P - Center) / Radius;
                    return true;
                }
                temp = (-b + (float)Math.Sqrt(discriminant)) / a;
                if (temp < tMax && temp > tMin)
                {
                    rec.T = temp;
                    rec.P = ray.At(rec.T);
                    rec.Normal = (rec.P - Center) / Radius;
                    return true;
                }
            }
            return false;
        }
    }

    public struct HitRecord
    {
        public float T;
        public Vector3 P;
        public Vector3 Normal;
    }

    class Engine
    {
       

       public static Vector3 RayColor(Ray ray, Sphere sphere1, Sphere sphere2)
        {
            if (sphere1.Hit(ray, 0.0f, float.MaxValue, out HitRecord rec1))
            {
                Vector3 target = rec1.P + rec1.Normal + RandomInUnitSphere();
                return 0.5f * RayColor(new Ray(rec1.P, target - rec1.P), sphere1, sphere2);
            }
            else if (sphere2.Hit(ray, 0.0f, float.MaxValue, out HitRecord rec2))
            {
                Vector3 target = rec2.P + rec2.Normal + RandomInUnitSphere();
                return 0.5f * RayColor(new Ray(rec2.P, target - rec2.P), sphere1, sphere2);
            }
            Vector3 unitDirection = ray.Direction.Normalize();
            float t = 0.5f * (unitDirection.Y + 1.0f);
            return (1.0f - t) * new Vector3(1.0f, 1.0f, 1.0f) + t * new Vector3(0.5f, 0.7f, 1.0f);
        }

        static Vector3 RandomInUnitSphere()
        {
            Random random = new Random();
            while (true) // could this be optimized?
            {
                Vector3 p = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble()) * 2.0f - new Vector3(1, 1, 1);
                if (p.Dot(p) >= 1.0f) continue;
                return p;
            }
        }
    }
}