using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using TreasureGuide.Common;
using TreasureGuide.Common.Models.ShipModels;
using TreasureGuide.Common.Models.StageModels;
using TreasureGuide.Common.Models.UnitModels;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;
using TreasureGuide.Sniffer.DataParser;
using TreasureGuide.Sniffer.Helpers;

namespace TreasureGuide.Sniffer
{
    public static class Program
    {
        private static bool Running;
        private static int ParsersRunning;

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("redditthreads.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            var context = new TreasureEntities(configuration.GetConnectionString("TreasureEntities"));
            var mapper = MapperConfig.Create();
            AssureContextOpen(context);
            RunParsers(context, mapper, configuration);
            while (Running || ParsersRunning > 0)
            {
                // ...
            }
            Debug.WriteLine("Seeya");
        }

        private static void AssureContextOpen(TreasureEntities context)
        {
            Debug.WriteLine("Checking if database is accessible.");
            Debug.WriteLine("There are " + context.Units.Count() + " unit(s) in the database right now.");
            Debug.WriteLine("Success!");
        }

        private static void RunParsers(TreasureEntities context, IMapper mapper, IConfigurationRoot configuration)
        {
            IEnumerable<IParser> parsers = new IParser[]
            {
                new UnitParser(context),
                new UnitFlagParser(context),
                new UnitAliasParser(context),
                new UnitEvolutionParser(context),
                new ShipParser(context),
                new StageParser(context),
                new ScheduleParserCal(context)
            };
            //  parsers = parsers.Concat(RedditImporter.GetThreads(configuration));
            Running = true;
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
                    await PostRun(context, mapper);
                    Running = false;
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
            context.CacheSets.Clear();
            await context.SaveChangesAsync();
        }

        private static async Task PostRun(TreasureEntities context, IMapper mapper)
        {
            await context.SaveChangesAsync();
            var timestamp = DateTimeOffset.UtcNow;
            await CacheBuilder.BuildCache<Unit, UnitStubModel>(context, mapper, CacheItemType.Unit, timestamp);
            await CacheBuilder.BuildCache<Stage, StageStubModel>(context, mapper, CacheItemType.Stage, timestamp);
            await CacheBuilder.BuildCache<Ship, ShipStubModel>(context, mapper, CacheItemType.Ship, timestamp);
        }
    }
}