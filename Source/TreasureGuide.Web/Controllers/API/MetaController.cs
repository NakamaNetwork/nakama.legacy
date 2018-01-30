using System;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Web.Models;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/meta")]
    public class SeoController : Controller
    {
        private static readonly Regex TeamRegex = new Regex("teams/(.+)/details");
        private static readonly Regex StageRegex = new Regex("stages/(.+)/details");
        private static readonly Regex AccountRegex = new Regex("account/(.+)");

        private readonly TreasureEntities _dbContext;

        public SeoController(TreasureEntities dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [ActionName("")]
        [Route("{id?}")]
        public async Task<IActionResult> Get(string id)
        {
            MetaResultModel model = null;
            Match match = null;
            if ((match = TeamRegex.Match(id)).Success)
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
            else if ((match = StageRegex.Match(id)).Success)
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
            else if ((match = AccountRegex.Match(id)).Success)
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
            return Ok(model);
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
