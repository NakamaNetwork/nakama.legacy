using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RedditSharp;
using RedditSharp.Things;
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

        protected override string Output { get; } = "Reddit";

        public RedditImporter(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        protected override async Task<IEnumerable<TeamEntry>> GetTeams()
        {
            var teams = new List<TeamEntry>();
            var section = _configuration.GetSection("RedditThreads").Get<RedditThread[]>();
            var reddit = new Reddit(WebAgent, false);
            foreach (var stage in section)
            {
                if (stage.Threads != null)
                {
                    foreach (var thread in stage.Threads)
                    {
                        teams.AddRange(FindTeamComments(thread, stage, reddit));
                    }
                }
            }
            return teams;
        }

        private IEnumerable<TeamEntry> FindTeamComments(string thread, RedditThread stage, Reddit reddit)
        {
            var teams = new List<TeamEntry>();
            var post = reddit.GetPost(thread.GetThreadUri());
            var comments = post.Comments;
            foreach (var comment in comments)
            {
                teams.AddRange(GetTeamsFromComment(comment, stage));
            }
            return teams;
        }

        private IEnumerable<TeamEntry> GetTeamsFromComment(Comment comment, RedditThread stage)
        {
            var author = comment.AuthorName;
            var body = comment.Body;
            if (author == "optclinkbot")
            {
                // Don't pull bot links.
                return Enumerable.Empty<TeamEntry>();
            }
            var links = body.GetCalcLinks();
            var teams = links.Select(x => new TeamEntry
            {
                CalcLink = x,
                Content = stage.Name,
                Leader = author,
                Credit = CreateCredit(author, comment.Shortlink),
                CreditReference = author,
                CreditType = TeamCreditType.Reddit,
                Desc = body,
                StageId = stage.StageId,
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
    }
}
