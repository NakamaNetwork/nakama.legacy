using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HeyRed.MarkdownSharp;
using Microsoft.EntityFrameworkCore;
using NakamaNetwork.Entities.Models;
using TreasureGuide.Common.Models;

namespace TreasureGuide.Web.Services
{
    public interface IMetadataService
    {
        Task<MetaResultModel> GetMetadata(string route);
    }

    public class MetadataService : IMetadataService
    {
        private static readonly Regex TeamRegex = new Regex("teams/(.+)/details");
        private static readonly Regex StageRegex = new Regex("stages/(.+)/details");
        private static readonly Regex BoxRegex = new Regex("boxes/(.+)/details");
        private static readonly Regex AccountRegex = new Regex("account/(.+)");

        private static readonly Regex TagRegex = new Regex("<[^>]*>");

        private readonly NakamaNetworkContext _dbContext;

        public MetadataService(NakamaNetworkContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MetaResultModel> GetMetadata(string route)
        {
            MetaResultModel model = null;
            if (!String.IsNullOrWhiteSpace(route))
            {
                Match match = null;
                if ((match = TeamRegex.Match(route)).Success)
                {
                    var team = GetId(match);
                    if (team.HasValue)
                    {
                        model = await _dbContext.Teams
                            .Where(x => x.Id == team)
                            .Select(x => new MetaResultModel
                            {
                                Title = x.Name,
                                Description = x.Guide
                            }).SingleOrDefaultAsync();
                    }
                }
                else if ((match = StageRegex.Match(route)).Success)
                {
                    var stage = GetId(match);
                    if (stage.HasValue)
                    {
                        model = await _dbContext.Stages
                            .Where(x => x.Id == stage)
                            .Select(x => new MetaResultModel
                            {
                                Title = x.Name
                            }).SingleOrDefaultAsync();
                    }
                }
                else if ((match = AccountRegex.Match(route)).Success)
                {
                    var acct = GetString(match);
                    if (!String.IsNullOrWhiteSpace(acct))
                    {
                        model = await _dbContext.UserProfiles
                            .Where(x => x.Id == acct || x.UserName == acct)
                            .Select(x => new MetaResultModel
                            {
                                Title = x.UserName
                            }).SingleOrDefaultAsync();
                    }
                }
                else if ((match = BoxRegex.Match(route)).Success)
                {
                    var box = GetId(match);
                    if (box.HasValue)
                    {
                        model = await _dbContext.Boxes
                            .Where(x => x.Id == box && x.Public == true)
                            .Select(x => new MetaResultModel
                            {
                                Title = x.Name
                            }).SingleOrDefaultAsync();
                    }
                }
            }
            model = model ?? new MetaResultModel();
            if (String.IsNullOrWhiteSpace(model.Title))
            {
                model.Title = MetaResultModel.DefaultTitle;
            }
            else
            {
                model.Title = Trim(model.Title, 80);
                model.Title += " | Nakama Network";
            }
            if (String.IsNullOrWhiteSpace(model.Description))
            {
                model.Description = MetaResultModel.DefaultDescription;
            }
            else
            {
                model.Description = CleanMarkdown(model.Description, 600);
            }
            return model;
        }

        private string Trim(string title, int length)
        {
            if (title.Length > length)
            {
                title = title.Substring(0, length - 3) + "...";
            }
            return title;
        }

        private string CleanMarkdown(string desc, int length)
        {
            var markdown = new Markdown();
            var output = markdown.Transform(desc);
            output = TagRegex.Replace(output, "");
            return Trim(output, 500);
        }

        private int? GetId(Match match)
        {
            var group = GetString(match);
            int id;
            if (Int32.TryParse(group, out id))
            {
                return id;
            }
            return null;
        }

        private string GetString(Match match)
        {
            if (match.Groups.Count > 1)
            {
                var group = match.Groups[1].Value;
                return group;
            }
            return null;
        }
    }
}
