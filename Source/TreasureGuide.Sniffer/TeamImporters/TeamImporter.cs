using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreasureGuide.Sniffer.TeamImporters.Models;

namespace TreasureGuide.Sniffer.TeamImporters
{
    public abstract class TeamImporter : IParser
    {
        protected abstract Task<IEnumerable<TeamEntry>> GetTeams();

        public async Task Execute()
        {
            var teams = await GetTeams();
            var teamStrings = teams.Select(x => x.ParseOut());
            var output = String.Join("|", teamStrings);
            Console.WriteLine(output);
        }
    }
}
