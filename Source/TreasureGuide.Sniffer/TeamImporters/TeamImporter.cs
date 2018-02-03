using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TreasureGuide.Sniffer.TeamImporters.Models;

namespace TreasureGuide.Sniffer.TeamImporters
{
    public abstract class TeamImporter : IParser
    {
        protected abstract Task<IEnumerable<TeamEntry>> GetTeams();

        protected abstract string Output { get; }

        public async Task Execute()
        {
            var teams = await GetTeams();
            var teamStrings = teams.Select(x => x.ParseOut());
            var output = String.Join("|", teamStrings);
            var filename = $"{DateTime.Now.ToFileTime()}_{Output}.txt";
            File.WriteAllText(filename, output);
        }
    }
}
