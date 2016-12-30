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
            context.Units.Count();
        }

        private static void RunParsers(TreasureEntities context)
        {
            var parsers = new[]
            {
             new UnitParser(context),
            };
            ParsersRunning = parsers.Count();

            foreach (var parser in parsers)
            {
                var name = parser.GetType().Name;
                Task.Run(async () =>
                {
                    try
                    {
                        Debug.WriteLine($"Running {name}.");
                        await parser.Execute();
                        Debug.WriteLine($"{name} Succeeded!");
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine($"{name} Failed!");
                    }
                    finally
                    {
                        ParsersRunning--;
                        Debug.WriteLine($"{ParsersRunning} Parser(s) Remain");
                    }
                });
            }
        }
    }
}
