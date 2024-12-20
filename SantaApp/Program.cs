using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;


namespace SantaApp
{
    internal class Program
    {
        // Struktura do reprezentowania punktu
        public struct Punkt
        {
            // miałem najpierw int, ale zamieniłem na double po błądzie
            public double x { get; set; }
            public double y { get; set; }
        }

        static double GetDistance(Punkt p1, Punkt p2)
        {
            // literalnie √((x2-x1)^2+(y2-y1)^2)=dystans między kropkami
            //AKA twierdzenie Pidarasa
            return Math.Sqrt(Math.Pow(p2.x - p1.x, 2) + Math.Pow(p2.y - p1.y, 2));
        }
        static void Main(string[] args)
        {
            //timer na obliczenie czasu działania
            var stopwatch = Stopwatch.StartNew();
            //plik santa znajdujący się w SantaApp/bin/Debug
            string santa = "santa.json";
            string json = File.ReadAllText(santa);
            //deserializacja
            List<Punkt> points = JsonConvert.DeserializeObject<List<Punkt>>(json);
            //pierwszy punkt
            List<Punkt> route = new List<Punkt>();
            route.Add(points[0]);
            //lista punktów do odwiedzenia
            List<Punkt> remainingPoints = new List<Punkt>(points);
            //pierwszy punkt jest usuwany
            remainingPoints.RemoveAt(0);
            //ustawiam pierwszy punkt
            Punkt currentPoint = points[0];
            //algorytm najbliższego punktu z każdej iteracji
            while (remainingPoints.Count > 0)//dopóki są jeszcze punkty do odwiedzenia
            {
                //szukam najbliższy punkt spośród pozostałych
                //LINQ do posortowania punktów według odległości od obecnego punktu i wybiera pierwszy bliższy
                Punkt nearestPoint = remainingPoints.OrderBy(p => GetDistance(currentPoint, p)).First();
                //dodaje najbliższy punkt do trasy
                route.Add(nearestPoint);
                //usuwa najbliższy punkt z listy punktów do odwiedzenia
                remainingPoints.Remove(nearestPoint);
                //aktualny najbliższy punkt
                currentPoint = nearestPoint;
            }
            //całkowitą długość trasy
            double totalDistance = 0;
            //iteruje przez wszystkie pary punktów w trasie
            for (int i = 0; i < route.Count - 1; i++)
            {
                //odległość między kolejnymi punktami
                totalDistance += GetDistance(route[i], route[i + 1]);
            }
            // Wyświetla wyniki
            Console.WriteLine("Kolejność odwiedzonych punktów:");
            foreach (var point in route)
            {
                Console.WriteLine($"({point.x}, {point.y})");
            }
            Console.WriteLine($"\nCałkowita długość trasy: {totalDistance} jednostek.");
            stopwatch.Stop();
            Console.WriteLine($"Czas wykonania obliczeń: {stopwatch.ElapsedMilliseconds} ms");
            Console.ReadKey();
        }
    }
}
