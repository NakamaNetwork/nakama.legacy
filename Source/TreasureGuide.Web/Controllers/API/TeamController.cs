using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;
using TreasureGuide.Web.Constants;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Helpers;
using TreasureGuide.Web.Models.TeamModels;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/team")]
    public class TeamController : SearchableApiController<int, Team, int?, TeamStubModel, TeamDetailModel, TeamEditorModel, TeamSearchModel>
    {
        public TeamController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(dbContext, autoMapper, throttlingService)
        {
        }

        protected override async Task<Team> PostProcess(Team entity)
        {
            var userId = User.GetId();
            var now = DateTimeOffset.Now;
            if (entity.Version == 0)
            {
                entity.SubmittedById = userId;
                entity.SubmittedDate = now;
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
                var userId = User.GetId();
                if (userId != null)
                {
                    var vote = await DbContext.TeamVotes.SingleOrDefaultAsync(x => x.TeamId == id && x.UserId == userId);
                    detail.MyVote = vote?.Value ?? 0;
                }
            }
            return await base.SingleGetTransform(single, id);
        }

        protected override bool CanPost(int? id)
        {
            return User.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator) || OwnsTeam(id);
        }

        protected override bool CanDelete(int? id)
        {
            return CanPost(id);
        }

        protected override IQueryable<Team> OrderSearchResults(IQueryable<Team> results)
        {
            return results.OrderByDescending(x => x.EditedDate);
        }

        protected bool OwnsTeam(int? id)
        {
            var userId = User.GetId();
            return DbContext.Teams.Any(x => x.Id == id && x.SubmittedById == userId);
        }

        protected override async Task<IQueryable<Team>> PerformSearch(IQueryable<Team> results, TeamSearchModel model)
        {
            results = SearchStage(results, model.StageId);
            results = SearchTerm(results, model.Term);
            results = SearchSubmitter(results, model.SubmittedBy);
            results = SearchLead(results, model.LeaderId);
            results = SearchGlobal(results, model.Global);
            results = SearchFreeToPlay(results, model.FreeToPlay, model.LeaderId);
            results = SearchBox(results, model.MyBox);
            return results;
        }

        private IQueryable<Team> SearchTerm(IQueryable<Team> teams, string term)
        {
            if (!String.IsNullOrEmpty(term))
            {
                teams = teams.Where(x => x.Name.Contains(term));
            }
            return teams;
        }

        private IQueryable<Team> SearchSubmitter(IQueryable<Team> teams, string term)
        {
            if (!String.IsNullOrEmpty(term))
            {
                teams = teams.Where(x => x.SubmittingUser.UserName.Contains(term));
            }
            return teams;
        }

        private IQueryable<Team> SearchStage(IQueryable<Team> teams, int? stageId)
        {
            if (stageId.HasValue)
            {
                teams = teams.Where(x => x.StageId == stageId);
            }
            return teams;
        }

        private IQueryable<Team> SearchLead(IQueryable<Team> teams, int? leaderId)
        {
            if (leaderId.HasValue)
            {
                teams = teams.Where(x => x.TeamUnits.Any(y => y.UnitId == leaderId && y.Position < 2));
            }
            return teams;
        }

        private IQueryable<Team> SearchGlobal(IQueryable<Team> teams, bool global)
        {
            if (global)
            {
                teams = teams.Where(x => x.TeamUnits.All(y => y.Sub || y.Unit.Flags.HasFlag(UnitFlag.Global)));
            }
            return teams;
        }

        private IQueryable<Team> SearchBox(IQueryable<Team> teams, bool myBox)
        {
            if (myBox)
            {
                throw new System.NotImplementedException();
            }
            return teams;
        }

        private IQueryable<Team> SearchFreeToPlay(IQueryable<Team> results, bool freeToPlay, int? leaderId)
        {
            if (freeToPlay)
            {
                results = results.Where(x => x.TeamUnits.All(y => y.Sub || y.Position == 0 || y.UnitId == leaderId || !EnumerableHelper.PayToPlay.Any(z => y.Unit.Flags.HasFlag(z))));
            }
            return results;
        }

        [HttpPost]
        [Authorize]
        [ActionName("Vote")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> Vote([FromBody] TeamVoteModel model, int? id = null)
        {
            if (Throttled && !ThrottlingService.CanAccess(User, Request))
            {
                return StatusCode((int)HttpStatusCode.Conflict, ThrottleService.Message);
            }
            var teamId = id ?? model.TeamId;
            var userId = User.GetId();
            var vote = await DbContext.TeamVotes.SingleOrDefaultAsync(x => x.TeamId == model.TeamId && x.UserId == userId);
            var exists = vote != null;
            vote = vote ?? new TeamVote
            {
                TeamId = teamId,
                UserId = userId
            };
            var value = model.Up.HasValue ? (model.Up ?? true) ? 1 : -1 : 0;
            if (model.Up.HasValue)
            {
                if (!exists)
                {
                    DbContext.TeamVotes.Add(vote);
                }
                vote.Value = (short)value;
            }
            else if (exists)
            {
                DbContext.TeamVotes.Remove(vote);
            }
            await DbContext.SaveChangesAsync();
            var returnValue = await DbContext.TeamVotes.Where(x => x.TeamId == teamId).Select(x => x.Value).DefaultIfEmpty((short)0).SumAsync(x => x);
            return Ok(returnValue);
        }
    }
}
