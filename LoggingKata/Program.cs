using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace LoggingKata
{
    class Program
    {
        static readonly ILog logger = new TacoLogger();
        const string csvPath = "TacoBell-US-AL.csv";

        static void Main(string[] args)
        {
            logger.LogInfo("Log initialized");


            //gets all the lines from the csv file in a comma deliniated string
            string[] lines = File.ReadAllLines(csvPath);
            if (lines.Length == 0)
            {
                logger.LogFatal("csv file empty");
                return;
            }
            if (lines.Length == 1)
                logger.LogWarning("csv file only has 1 line");


            //parses the comma delinated string into an array of ITrackable interface objects, namely taco bells
            logger.LogInfo("Begin parsing");
            TacoParser parser = new TacoParser();
            ITrackable[] locations = lines.Select(parser.Parse).ToArray();

            //Retrive connection string from json file and passes the connection into TacoRepository
            IConfigurationRoot config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
            string connString = config.GetConnectionString("DefaultConnection");
            IDbConnection conn = new MySqlConnection(connString);
            TacoRepository tacoRepository = new TacoRepository(conn);


            List<TacoBell> tacoList = (List<TacoBell>)tacoRepository.GetTacoBells();
            if (tacoList.Count == 0)
            {
                foreach (TacoBell tacoBell in locations)
                {
                    tacoRepository.InsertTacoBell(tacoBell.Location.Latitude, tacoBell.Location.Longitude, tacoBell.Name);
                }

            }

            tacoList = (List<TacoBell>)tacoRepository.GetTacoBells();

            //tries to iterate through the array of taco bells
            //and uses the latitude and longitude of the taco bells
            //to create geocoordinates then stores those in a list named geos
            //      throws an exception if a null reference
            logger.LogInfo("Creating List of GeoCoordinates");
            List<GeoCoordinate> geos = new List<GeoCoordinate>();
            try
            {
                foreach (TacoBell taco in tacoList)
                {
                    geos.Add(new GeoCoordinate(taco.Location.Latitude, taco.Location.Longitude));
                }
            }
            catch (Exception e)
            {
                logger.LogError("null", e);
            }


            // Creates a list of Tuples containing a pair of taco bells
            // along with the distance between them

            logger.LogInfo("Creating List of Distance Between Two Taco Bells");
            List<Tuple<ITrackable, ITrackable, double>> distances = new List<Tuple<ITrackable, ITrackable, double>>();
            for (int i = 0; i < geos.Count; i++)
            {
                for (int x = i + 1; x < geos.Count; x++)
                {
                    double dist = geos[i].GetDistanceTo(geos[x]);
                    Tuple<ITrackable, ITrackable, double> hold = new Tuple<ITrackable, ITrackable, double>
                                                            (tacoList[i], tacoList[x], dist);
                    distances.Add(hold);
                }
            }

            //iterates through the list of tuples
            //and compares the distance to the distance of the the pair at index farTaco
            //then sets farTaco to the the index of the greater of the two distances 
            logger.LogInfo("Finding Furthest Distance Between Two Taco Bells");
            int farTaco = 0;
            for (int i = 0; i < distances.Count; i++)
            {
                if (distances[i].Item3 > distances[farTaco].Item3)
                    farTaco = i;
            }

            //gets the pair and distance of the furthest two Taco Bells
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