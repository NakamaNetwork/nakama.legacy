using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TreasureGuide.Entities;
using TreasureGuide.Sniffer.DataParser;
using TreasureGuide.Sniffer.TeamImporters;

namespace TreasureGuide.Sniffer
{
    public static class Program
    {
        private static int ParsersRunning;

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("redditthreads.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            var context = new TreasureEntities(configuration.GetConnectionString("TreasureEntities"));
            AssureContextOpen(context);
            RunParsers(context, configuration);
            while (ParsersRunning > 0)
            {
                // ...
            }
        }

        private static void AssureContextOpen(TreasureEntities context)
        {
            Console.WriteLine("Checking if database is accessible.");
            Console.WriteLine("There are " + context.Units.Count() + " unit(s) in the database right now.");
            Console.WriteLine("Success!");
        }

        private static void RunParsers(TreasureEntities context, IConfigurationRoot configuration)
        {
            IEnumerable<IParser> parsers = new IParser[]
            {
                //new UnitParser(context),
                //new UnitFlagParser(context),
                //new UnitAliasParser(context),
                new StageParser(context),
                //new ShipParser(context),
            };
          //  parsers = parsers.Concat(RedditImporter.GetThreads(configuration));
            ParsersRunning = parsers.Count();

            Task.Run(async () =>
            {
                foreach (var parser in parsers)
                {
                    var name = parser.GetType().Name;
                    try
                    {
                        Console.WriteLine($"Running {name}.");
                        await parser.Execute();
                        Console.WriteLine($"{name} Succeeded!");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"{name} Failed!");
                        Console.WriteLine(e);
                    }
                    finally
                    {
                        ParsersRunning--;
                        Console.WriteLine($"{ParsersRunning} Parser(s) Remain");
                    }
                    GC.Collect();
                }
            });
        }
    }
}