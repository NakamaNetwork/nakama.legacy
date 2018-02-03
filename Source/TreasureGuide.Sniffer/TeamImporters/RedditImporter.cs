using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RedditSharp;
using TreasureGuide.Entities;
using TreasureGuide.Sniffer.Helpers;
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

        private string _title;
        private RedditThread _thread;

        protected override string Output => _title;

        public RedditImporter(RedditThread thread, IConfigurationRoot configuration)
        {
            _configuration = configuration;
            _title = thread.Name.Replace(" ", "_");
            _thread = thread;
        }

        protected override async Task<IEnumerable<TeamEntry>> GetTeams()
        {
            var teams = new List<TeamEntry>();
            var reddit = new Reddit(WebAgent, false);
            foreach (var thread in _thread.Threads)
            {
                teams.AddRange(FindTeamComments(thread, reddit));
            }
            return teams;
        }

        private IEnumerable<TeamEntry> FindTeamComments(string thread, Reddit reddit)
        {
            var teams = new List<TeamEntry>();
            var post = reddit.GetPost(thread.GetThreadUri());
            if (post != null)
            {
                teams.AddRange(GetTeamsFromComment(_thread.Name, _thread.StageId, post.AuthorName, post.SelfText, post.Shortlink));
                var comments = post.Comments;
                foreach (var comment in comments)
                {
                    teams.AddRange(GetTeamsFromComment(_thread.Name, _thread.StageId, comment.AuthorName, comment.Body, comment.Shortlink));
                }
            }
            return teams;
        }

        private IEnumerable<TeamEntry> GetTeamsFromComment(string name, int? stageId, string author, string body, string link)
        {
            if (author == "optclinkbot")
            {
                // Don't pull bot links.
                return Enumerable.Empty<TeamEntry>();
            }
            var links = body.GetCalcLinks();
            var teams = links.Select(x => new TeamEntry
            {
                Name = $"/u/{author}'s {name} Team",
                CalcLink = x,
                Credit = CreateCredit(author, link),
                CreditReference = author,
                CreditType = TeamCreditType.Reddit,
                Desc = body,
                StageId = stageId,
                Video = String.Join(",", body.GetYouTubeLinks())
            });
            return teams;
        }

        private string CreateCredit(string author, string commentShortlink)
        {
            return
                $"Team submitted to [/r/OnePieceTC](http://www.reddit.com/r/onepiecetc) by user [/u/{author}](http://www.reddit.com/u/{author})\n\n" +
                $"[Click here for the original comment.]({commentShortlink.Replace("oauth", "www")})";
        }

        public static IEnumerable<RedditImporter> GetThreads(IConfigurationRoot config)
        {
            var section = config.GetSection("RedditThreads").Get<RedditThread[]>();
            return section.Select(x => new RedditImporter(x, config));
        }
    }
}
