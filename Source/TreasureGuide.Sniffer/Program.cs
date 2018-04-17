using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;
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
            Debug.WriteLine("Checking if database is accessible.");
            Debug.WriteLine("There are " + context.Units.Count() + " unit(s) in the database right now.");
            Debug.WriteLine("Success!");
        }

        private static void RunParsers(TreasureEntities context, IConfigurationRoot configuration)
        {
            IEnumerable<IParser> parsers = new IParser[]
            {
                new UnitParser(context),
                new UnitFlagParser(context),
                new UnitAliasParser(context),
                new UnitEvolutionParser(context),
                new ShipParser(context),
                new StageParser(context),
                new ScheduleParserAgenda(context),
                new ScheduleParserCal(context)
            };
            //  parsers = parsers.Concat(RedditImporter.GetThreads(configuration));
            ParsersRunning = parsers.Count();

            Task.Run(async () =>
            {
                await PreRun(context);
                foreach (var parser in parsers)
                {
                    var name = parser.GetType().Name;
                    try
                    {
                        Debug.WriteLine($"Running {name}.");
                        await parser.Execute();
                        Debug.WriteLine($"{name} Succeeded!");
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine($"{name} Failed!");
                        Debug.WriteLine(e);
                    }
                    finally
                    {
                        ParsersRunning--;
                        Debug.WriteLine($"{ParsersRunning} Parser(s) Remain");
                    }
                    GC.Collect();
                }
                await PostRun(context);
            });
        }

        private static async Task PreRun(TreasureEntities context)
        {
            context.Teams.Clear();
            context.TeamUnits.Clear();
            context.ScheduledEvents.Clear();
            context.StageAliases.Clear();
            context.Stages.Clear();
            context.Ships.Clear();
            context.UnitAliases.Clear();
            context.UnitEvolutions.Clear();
            context.Units.Clear();
            context.DeletedItems.Clear();
            await context.SaveChangesAsync();
        }

        private static async Task PostRun(TreasureEntities context)
        {
            context.DeletedItems.Clear();
            await context.SaveChangesAsync();
        }
    }
}