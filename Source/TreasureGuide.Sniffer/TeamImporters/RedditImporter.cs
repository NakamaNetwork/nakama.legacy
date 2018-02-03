using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RedditSharp;
using TreasureGuide.Sniffer.TeamImporters.Models;

namespace TreasureGuide.Sniffer.TeamImporters
{
    public class RedditImporter : TeamImporter
    {
        private static IConfigurationRoot _configuration;
        private static BotWebAgent _webAgent;

        private static BotWebAgent WebAgent
        {
            get
            {
                if (_webAgent == null)
                {
                    _webAgent = new BotWebAgent(
                        _configuration["reddit.username"],
                        _configuration["reddit.password"],
                        _configuration["reddit.clientId"],
                        _configuration["reddit.clientSecret"],
                        _configuration["reddit.redirectUri"]
                   );
                }
                return _webAgent;
            }
        }

        protected override string Output { get; } = "Reddit";

        public RedditImporter(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        protected override Task<IEnumerable<TeamEntry>> GetTeams()
        {
            throw new NotImplementedException();
        }
    }
}
