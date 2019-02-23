using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Common.Constants;
using NakamaNetwork.Entities;
using NakamaNetwork.Entities.Helpers;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Common.Helpers;
using TreasureGuide.Common.Models;
using TreasureGuide.Common.Models.TeamModels;
using TreasureGuide.Web.Helpers;
using TreasureGuide.Web.Services;
using TreasureGuide.Web.Services.SearchService.Teams;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/team")]
    public class TeamController : SearchableApiController<int, Team, int?, TeamStubModel, TeamDetailModel, TeamEditorModel, TeamSearchModel>
    {
        private readonly TeamSearchService _searchService;

        public TeamController(TeamSearchService searchService, TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(dbContext, autoMapper, throttlingService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        [ActionName("Wiki")]
        [Route("[action]")]
        [EnableCors("NakamaCORS")]
        public async Task<SearchResult<WikiSearchResultModel>> Wiki(TeamSearchModel model)
        {
            return await Search<WikiSearchResultModel>(model);
        }

        protected override async Task<object> CreateOrUpdate(TeamEditorModel model, Team entity = null)
        {
            var result = await base.CreateOrUpdate(model, entity);
            var id = (result as IdResponse<int>)?.Id;
            if (id.HasValue)
            {
                await UpdateTeamMinis(id.Value);
            }
            return result;
        }

        private async Task UpdateTeamMinis(int idValue)
        {
            var teamData = await DbContext.Teams.Select(x => new
            {
                TeamId = x.Id,
                Name = x.Name,
                StageId = x.StageId,
                StageName = x.Stage != null ? x.Stage.Name : "",
                InvasionId = x.InvasionId,
                InvasionName = x.Invasion != null ? x.Invasion.Name : "",
                EventShip = x.Ship != null && x.Ship.EventShip,
                SubmittedById = x.SubmittedById,
                SubmittingUserName = x.SubmittingUser != null ? x.SubmittingUser.UserName : "",
                Draft = x.Draft,
                Deleted = x.Deleted,
                HasReport = x.TeamReports.Any(y => y.AcknowledgedDate == null),
                Units = x.TeamUnits.Where(y => !y.Sub).Select(y => new { Id = y.UnitId, Position = y.Position, Flags = y.Unit.Flags, Class = y.Unit.Class, Type = y.Unit.Type })
            }).SingleOrDefaultAsync(x => x.TeamId == idValue);
            if (teamData != null)
            {
                var mini = await DbContext.TeamMinis.SingleOrDefaultAsync(x => x.TeamId == idValue);
                if (mini != null)
                {
                    mini.Name = teamData.Name;
                    mini.StageId = teamData.StageId;
                    mini.StageName = teamData.StageName;
                    mini.InvasionId = teamData.InvasionId;
                    mini.InvasionName = teamData.InvasionName;
                    mini.EventShip = teamData.EventShip;
                    mini.SubmittedById = teamData.SubmittedById;
                    mini.SubmittingUserName = teamData.SubmittingUserName;
                    mini.Draft = teamData.Draft;
                    mini.Deleted = teamData.Deleted;
                    mini.HasReport = teamData.HasReport;
                    mini.HelperId = teamData.Units.SingleOrDefault(x => x.Position == 0)?.Id;
                    mini.LeaderId = teamData.Units.SingleOrDefault(x => x.Position == 1)?.Id;
                    mini.F2PC = !teamData.Units.Where(x => x.Position > 1)
                        .Any(x => x.Flags.HasFlag(UnitFlag.RareRecruitExclusive) ||
                                  x.Flags.HasFlag(UnitFlag.RareRecruitLimited));
                    mini.F2P = mini.F2PC && !teamData.Units.Where(x => x.Position == 1)
                                   .Any(x => x.Flags.HasFlag(UnitFlag.RareRecruitExclusive) ||
                                             x.Flags.HasFlag(UnitFlag.RareRecruitLimited));
                    mini.Type = teamData.Units.Select(x => x.Type).Aggregate((sum, current) => sum & current);
                    mini.Class = teamData.Units.Select(x => x.Class).Aggregate((sum, current) => sum & current);
                    await DbContext.SaveChangesSafe();
                }
            }
        }

        protected override async Task<Team> PostProcess(Team entity)
        {
            var userId = User.GetId();
            var now = DateTimeOffset.Now;
            if (entity.Version == 0 || entity.SubmittedById == null)
            {
                entity.SubmittedById = userId;
                entity.SubmittedDate = now;
                entity.TeamVotes = new List<TeamVote>
                {
                    new TeamVote
                    {
                        SubmittedDate = now,
                        UserId = userId,
                        Value = 1
                    }
                };
                entity.TeamScore = new TeamScore
                {
                    Value = 1
                };
                entity.TeamMini = new TeamMini
                {
                    Name = entity.Name,
                    SubmittingUserName = entity.SubmittedById,
                    SubmittedById = entity.SubmittedById
                };
            }
            entity.EditedById = userId;
            entity.EditedDate = now;
            entity.Version = entity.Version + 1;
            return await base.PostProcess(entity);
        }

        protected override async Task<TModel> SingleGetTransform<TModel>(TModel single, int? id = null)
        {
            var detail = single as TeamDetailModel;
            if (detail != null)
            {
                if (!User.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator))
                {
                    detail.TeamVideos = detail.TeamVideos.Where(x => !x.Deleted);
                }
                var userId = User.GetId();
                if (userId != null)
                {
                    var user = DbContext.UserProfiles.Where(x => x.Id == userId);
                    detail.MyVote = await user.SelectMany(x => x.TeamVotes.Where(y => y.TeamId == id).Select(y => y.Value)).SingleOrDefaultAsync();
                    detail.MyBookmark = await user.SelectMany(x => x.BookmarkedTeams.Where(y => y.Id == id).Select(y => true)).SingleOrDefaultAsync();
                }
            }
            return await base.SingleGetTransform(single, id);
        }

        protected override IQueryable<Team> Filter(IQueryable<Team> entities)
        {
            var userId = User.GetId();
            if (!User.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator))
            {
                entities = entities.Where(x => !x.Deleted && (!x.Draft || x.SubmittedById == userId));
            }
            return base.Filter(entities);
        }

        protected override bool CanPost(int? id)
        {
            var userId = User.GetId();
            return !String.IsNullOrWhiteSpace(userId) && (User.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator) || OwnsTeam(id, userId));
        }

        protected override bool CanDelete(int? id)
        {
            return CanPost(id);
        }

        protected override IQueryable<Team> OrderSearchResults(IQueryable<Team> results, TeamSearchModel model)
        {
            switch (model.SortBy ?? "")
            {
                case SearchConstants.SortId:
                    return results.OrderBy(x => x.Id, model.SortDesc);
                case SearchConstants.SortName:
                    return results.OrderBy(x => x.Name, model.SortDesc);
                case SearchConstants.SortStage:
                    return results.OrderBy(x => x.Stage != null ? x.Stage.Name : "", model.SortDesc);
                case SearchConstants.SortLeader:
                    return results.OrderBy(x => x.TeamUnits.Where(y => y.Position == 1 && !y.Sub).Select(y => y.Unit.Name).DefaultIfEmpty("").FirstOrDefault(), model.SortDesc);
                case SearchConstants.SortScore:
                    return results.OrderBy(x => x.TeamScore != null ? x.TeamScore.Value : 0, !model.SortDesc);
                case SearchConstants.SortDate:
                    return results.OrderBy(x => x.SubmittedDate, !model.SortDesc);
                case SearchConstants.SortUser:
                    return results.OrderBy(x => x.SubmittingUser.UserName, model.SortDesc);
                default:
                    return results.OrderBy(x => x.Id, true);
            }
        }

        protected bool OwnsTeam(int? id, string userId)
        {
            if (!id.HasValue || id == 0)
            {
                return true;
            }
            return DbContext.Teams.Any(x => x.Id == id && x.SubmittedById == userId);
        }

        protected override async Task<IQueryable<Team>> PerformSearch(IQueryable<Team> results, TeamSearchModel model)
        {
            return await _searchService.Search(results, model, User);
        }

        [HttpGet]
        [ActionName("Calc")]
        [Route("{id}/[action]")]
        public async Task<IActionResult> Calc(int? id)
        {
            var link = (await GetCalcLinks(id)).FirstOrDefault().Value;
            if (link == null)
            {
                return BadRequest("Could not find the input team.");
            }
            return Redirect(link);
        }

        private static Regex IdRegex = new Regex("(\\d+)");
        private const string CalcPrefix = "http://optc-db.github.io/damage/#/transfer/D";
        private const string CalcPostfix = "B0D0E0Q0L0G0R0S100H";

        [HttpGet]
        [ActionName("CalcLink")]
        [Route("{ids}/[action]")]
        public async Task<IActionResult> CalcLink(string ids)
        {
            var matches = IdRegex.Matches(ids);
            if (matches.Count > 0)
            {
                var separated = Enumerable.Range(0, matches.Count).Select(x =>
                {
                    int id;
                    if (Int32.TryParse(matches[x].Value, out id))
                    {
                        return id;
                    }
                    return (int?)null;
                }).Where(x => x != null).ToArray();
                var link = await GetCalcLinks(separated);
                return Ok(link);
            }
            return BadRequest();
        }

        private async Task<IDictionary<int, string>> GetCalcLinks(params int?[] ids)
        {
            var teams = await DbContext.Teams.Where(x => ids.Contains(x.Id)).Select(x => new
            {
                x.Id,
                x.ShipId,
                Units = x.TeamUnits.Where(y => !y.Sub).Select(z => new { Id = z.UnitId, z.Position, z.Unit.MaxLevel })
            }).ToListAsync();
            var output = teams.ToDictionary(x => x.Id, x =>
            {
                var characters = "";
                for (var i = 0; i < 6; i++)
                {
                    var unit = x.Units.SingleOrDefault(y => y.Position == i);
                    if (unit != null)
                    {
                        characters += unit.Id + ":" + Math.Max((int)unit.MaxLevel, 1);
                    }
                    else
                    {
                        characters += "!";
                    }
                    if (i < 5)
                    {
                        characters += ",";
                    }
                }
                var boatString = "C" + x.ShipId + ",10";
                return CalcPrefix + characters + boatString + CalcPostfix;
            });
            return output;
        }

        [HttpGet]
        [ActionName("Trending")]
        [Route("[action]")]
        public async Task<IActionResult> Trending()
        {
            var threshold = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(1.5));
            var result = await DbContext.TeamVotes
                .Where(x => x.SubmittedDate > threshold)
                .GroupBy(x => x.TeamId)
                .Join(DbContext.Teams, x => x.Key, y => y.Id, (x, y) => new { score = x.Select(z => z.Value).DefaultIfEmpty().Sum(z => z), team = y })
                .OrderByDescending(x => x.score)
                .Select(x => x.team)
                .Where(x => !x.Deleted && !x.Draft)
                .Take(5)
                .ProjectTo<TeamStubModel>(AutoMapper.ConfigurationProvider)
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet]
        [ActionName("Latest")]
        [Route("[action]")]
        public async Task<IActionResult> Latest()
        {
            var top = await DbContext.Teams
                .Where(x => !x.Deleted && !x.Draft)
                .OrderByDescending(x => x.Id)
                .Take(5)
                .ProjectTo<TeamStubModel>(AutoMapper.ConfigurationProvider)
                .ToListAsync();
            return Ok(top);
        }

        [HttpPost]
        [Authorize]
        [ActionName("Vote")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> Vote([FromBody] TeamVoteModel model, int? id = null)
        {
            if (Throttled && !ThrottlingService.CanAccess(User, Request))
            {
                return StatusCode(429, ThrottleService.Message);
            }
            var teamId = id ?? model.TeamId;
            var userId = User.GetId();
            var vote = await DbContext.TeamVotes.SingleOrDefaultAsync(x => x.TeamId == model.TeamId && x.UserId == userId);
            var exists = vote != null;
            vote = vote ?? new TeamVote
            {
                TeamId = teamId,
                UserId = userId,
                SubmittedDate = DateTimeOffset.Now
            };
            var value = model.Up.HasValue ? (model.Up ?? true) ? 1 : -1 : 0;
            if (!exists)
            {
                DbContext.TeamVotes.Add(vote);
            }
            vote.Value = (short)value;
            await DbContext.SaveChangesSafe();
            await UpdateTeamScores(teamId);
            var returnValue = (await DbContext.TeamScores.SingleOrDefaultAsync(x => x.TeamId == teamId))?.Value ?? 0;
            return Ok(returnValue);
        }

        private async Task UpdateTeamScores(int teamId)
        {
            var set = await DbContext.TeamScores.SingleOrDefaultAsync(x => x.TeamId == teamId);
            if (set != null)
            {
                var score = await DbContext.TeamVotes.Where(x => x.TeamId == teamId).Select(x => x.Value).DefaultIfEmpty().SumAsync(x => x);
                set.Value = score;
                await DbContext.SaveChangesSafe();
            }
        }

        [HttpPost]
        [Authorize]
        [ActionName("Bookmark")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> Bookmark(int? id = null)
        {
            if (Throttled && !ThrottlingService.CanAccess(User, Request))
            {
                return StatusCode(429, ThrottleService.Message);
            }
            var team = await DbContext.Teams.SingleOrDefaultAsync(x => x.Id == id);
            if (team == null)
            {
                return BadRequest("Could not find team.");
            }
            var userId = User.GetId();
            var profile = await DbContext.UserProfiles.SingleOrDefaultAsync(x => x.Id == userId);
            if (profile == null)
            {
                return Unauthorized();
            }
            var existed = team.BookmarkedUsers.Any(x => x.Id == userId);
            if (existed)
            {
                team.BookmarkedUsers.Remove(profile);
            }
            else
            {
                team.BookmarkedUsers.Add(profile);
            }
            await DbContext.SaveChangesSafe();
            return Ok(!existed);
        }

        [HttpPost]
        [Authorize]
        [ActionName("Report")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> Report([FromBody] TeamReportModel model, int? id = null)
        {
            if (Throttled && !ThrottlingService.CanAccess(User, Request))
            {
                return StatusCode(429, ThrottleService.Message);
            }
            var teamId = id ?? model.TeamId;

            DbContext.TeamReports.Add(new TeamReport
            {
                TeamId = teamId,
                Reason = model.Reason
            });
            await DbContext.SaveChangesSafe();
            return Ok(teamId);
        }


        [HttpGet]
        [Authorize(Roles = RoleConstants.Administrator + "," + RoleConstants.Moderator)]
        [ActionName("Reports")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> Reports(int? id = null)
        {
            if (Throttled && !ThrottlingService.CanAccess(User, Request))
            {
                return StatusCode(429, ThrottleService.Message);
            }
            var reports = await DbContext
                .TeamReports
                .Where(x => x.TeamId == id)
                .OrderBy(x => x.AcknowledgedDate)
                .ThenBy(x => x.TeamId)
                .ProjectTo<TeamReportStubModel>(AutoMapper.ConfigurationProvider)
                .ToArrayAsync();
            return Ok(reports);
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.Administrator + "," + RoleConstants.Moderator)]
        [ActionName("AcknowledgeReport")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> AcknowledgeReport(int? id = null)
        {
            var team = await DbContext.TeamReports.SingleOrDefaultAsync(x => x.Id == id);
            if (team == null)
            {
                return BadRequest("Could not find report.");
            }
            team.AcknowledgedDate = DateTimeOffset.Now;
            await DbContext.SaveChangesSafe();
            return Ok(id);
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.Contributor)]
        [ActionName("Video")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> Video([FromBody] TeamVideoModel model, int? id = null)
        {
            var videoId = id ?? model.Id;
            TeamVideo video = null;
            if (videoId.HasValue)
            {
                video = await DbContext.TeamVideos.SingleOrDefaultAsync(x => x.Id == videoId);
            }
            var exists = video != null;
            if (exists && !(await CanEditVideo(video.Id)))
            {
                return Unauthorized();
            }
            video = video ?? new TeamVideo();
            video.UserId = User.GetId();
            video.Deleted = model.Deleted;
            video.SubmittedDate = DateTimeOffset.Now;
            video.VideoLink = model.VideoLink;
            video.TeamId = model.TeamId;
            if (!exists)
            {
                DbContext.TeamVideos.Add(video);
            }
            await DbContext.SaveChangesSafe();
            return Ok(video.Id);
        }

        private async Task<bool> CanEditVideo(int? videoId)
        {
            var userId = User.GetId();
            return userId != null && User.IsInAnyRole(RoleConstants.Moderator, RoleConstants.Administrator) ||
                   await DbContext.TeamVideos.AnyAsync(x => x.Id == videoId && x.UserId == userId);
        }

        public const string ImportId = "112cf4e4-cb26-4293-afa2-e663785fd276";

        [HttpPost]
        [Authorize(Roles = RoleConstants.GCRViewer)]
        [ActionName("Import")]
        [Route("[action]")]
        public async Task<IActionResult> Import([FromBody] TeamImportModel model)
        {
            var userId = User.GetId();
            var now = DateTimeOffset.Now;
            var team = AutoMapper.Map<Team>(model.Team);
            if (team.TeamUnits.Count < 2)
            {
                return BadRequest("Not enough units on team.");
            }
            if (String.IsNullOrWhiteSpace(team.Name))
            {
                team = await AutoGenName(team);
            }
            team.SubmittedDate = now;
            team.SubmittedById = userId;
            team.EditedDate = now;
            team.EditedById = userId;
            DbContext.Teams.Add(team);

            if (!String.IsNullOrWhiteSpace(model.Credit.Credit))
            {
                var credit = new TeamCredit
                {
                    Team = team,
                    Credit = model.Credit.Credit,
                    Type = model.Credit.Type
                };
                DbContext.TeamCredits.Add(credit);
            }

            var videos = model.Videos.Where(x => !String.IsNullOrWhiteSpace(x.VideoLink)).Select(x => new TeamVideo
            {
                Team = team,
                SubmittedDate = now,
                UserId = userId,
                VideoLink = x.VideoLink
            }).ToList();
            DbContext.TeamVideos.AddRange(videos);

            await DbContext.SaveChangesSafe();

            return Ok(team.Id);
        }

        private async Task<Team> AutoGenName(Team team)
        {
            var leader = team.TeamUnits.SingleOrDefault(x => x.Position == 1 && x.Sub == false)?.UnitId;
            var unitName = "";
            if (leader.HasValue)
            {
                unitName = (await DbContext.GCRUnitInfoes.SingleOrDefaultAsync(x => x.UnitId == leader))?.Name;
                if (String.IsNullOrWhiteSpace(unitName))
                {
                    unitName = (await DbContext.Units.SingleOrDefaultAsync(x => x.Id == leader))?.Name;
                }
            }

            var stage = team.StageId;
            var stageName = "";
            if (stage.HasValue)
            {
                stageName = (await DbContext.GCRStageInfoes.SingleOrDefaultAsync(x => x.StageId == stage))?.Name;
                if (String.IsNullOrWhiteSpace(stageName))
                {
                    stageName = (await DbContext.Stages.SingleOrDefaultAsync(x => x.Id == stage))?.Name;
                }
            }

            var name = unitName;
            if (String.IsNullOrWhiteSpace(stageName))
            {
                name += " Team";
            }
            else
            {
                name += " @ " + stageName;
            }
            team.Name = name;
            return team;
        }
    }
}