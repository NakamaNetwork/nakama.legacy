using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TreasureGuide.Entities;
using TreasureGuide.Sniffer.Parsers;

namespace TreasureGuide.Sniffer
{
    public static class Program
    {
        private static int ParsersRunning;

        public static void Main(string[] args)
        {
            var context = new TreasureEntities();
            AssureContextOpen(context);
            RunParsers(context);
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

        private static void RunParsers(TreasureEntities context)
        {
            var parsers = new ITreasureParser[]
            {
                new UnitParser(context),
                new UnitFlagParser(context),
                new UnitAliasParser(context),
                new StageParser(context),
                new ShipParser(context),
            };
            ParsersRunning = parsers.Count();

            Task.Run(async () =>
            {
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
            });
        }
    }
}