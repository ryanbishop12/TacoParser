using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;
using System.Collections.Generic;

namespace LoggingKata
{
    class Program
    {
        static readonly ILog logger = new TacoLogger();
        const string csvPath = "TacoBell-US-AL.csv";

        static void Main(string[] args)
        {
            logger.LogInfo("Log initialized");

            string[] lines = File.ReadAllLines(csvPath);

            logger.LogInfo("Begin parsing");

            TacoParser parser = new TacoParser();

            ITrackable[] locations = lines.Select(parser.Parse).ToArray();

            // TODO:  Find the two Taco Bells in Alabama that are the furthest from one another.
            // HINT:  You'll need two nested forloops

            logger.LogInfo("Creating List of GeoCoordinates");
            List<GeoCoordinate> geos = new List<GeoCoordinate>();
            foreach (TacoBell taco in locations)
            {
                geos.Add(new GeoCoordinate(taco.Location.Latitude, taco.Location.Longitude));
            }

            logger.LogInfo("Creating List of Distance Between Two Taco Bells");
            List<Tuple<ITrackable, ITrackable, double>> distances = new List<Tuple<ITrackable, ITrackable, double>>();

            for (int i = 0; i < geos.Count; i++)
            {
                for (int x = i + 1; x < geos.Count; x++)
                {
                    double dist = geos[i].GetDistanceTo(geos[x]);
                    Tuple<ITrackable, ITrackable, double> hold = new Tuple<ITrackable, ITrackable, double>
                                                            (locations[i], locations[x], geos[i].GetDistanceTo(geos[x]));
                    distances.Add(hold);
                }
            }

            logger.LogInfo("Finding Furthest Distance Between Two Taco Bells");
            int farTaco = 0;
            for (int i = 0; i < distances.Count; i++)
            {
                if (distances[i].Item3 > distances[farTaco].Item3)
                    farTaco = i;
            }

            Tuple<ITrackable, ITrackable, double> furthestTaco = distances[farTaco];

            Console.WriteLine();
            Console.WriteLine($"Taco Bell 1 : {furthestTaco.Item1.Name}");
            Console.WriteLine($"{furthestTaco.Item1.Location.Latitude}, {furthestTaco.Item1.Location.Longitude}");
            Console.WriteLine();
            Console.WriteLine($"Taco Bell 2 :{furthestTaco.Item2.Name}");
            Console.WriteLine($"{furthestTaco.Item2.Location.Latitude}, {furthestTaco.Item2.Location.Longitude}");
            Console.WriteLine();
            Console.WriteLine($"Distance : {furthestTaco.Item3}");
        }
    }
}