using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NakamaNetwork.Entities.Helpers;
using NakamaNetwork.Entities.Models;
using NakamaNetwork.Sniffer.DataParser;
using TreasureGuide.Sniffer;

namespace NakamaNetwork.Sniffer
{
    public static class Program
    {
        private static bool Running;
        private static int ParsersRunning;

        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var dbOptions =
                new DbContextOptionsBuilder<NakamaNetworkContext>()
                    .UseSqlServer(configuration.GetConnectionString("NakamaNetworkContext"))
                    .EnableSensitiveDataLogging();

            var context = new NakamaNetworkContext(dbOptions.Options);
            AssureContextOpen(context);
            RunParsers(context, configuration);
            while (Running || ParsersRunning > 0)
            {
                // ...
            }
            Debug.WriteLine("Seeya");
        }

        private static void AssureContextOpen(NakamaNetworkContext context)
        {
            Debug.WriteLine("Checking if database is accessible.");
            Debug.WriteLine("There are " + context.Units.Count() + " unit(s) in the database right now.");
            Debug.WriteLine("Success!");
        }

        private static void RunParsers(NakamaNetworkContext context, IConfigurationRoot configuration)
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
                    await PostRun(context);
                    Running = false;
                });
        }

        private static async Task PreRun(NakamaNetworkContext context)
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
            await context.SaveChangesAsync();
        }

        private static async Task PostRun(NakamaNetworkContext context)
        {
            await context.SaveChangesAsync();
        }
    }
}