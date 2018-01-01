﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;
using TreasureGuide.Web.Constants;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Models.TeamModels;
using TreasureGuide.Web.Helpers;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    public class TeamController : SearchableApiController<int, Team, int?, TeamStubModel, TeamDetailModel, TeamEditorModel, TeamSearchModel>
    {
        public TeamController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(dbContext, autoMapper, throttlingService)
        {
        }

        protected override Team PostProcess(Team entity)
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
            return base.PostProcess(entity);
        }

        protected override bool CanPost(int? id)
        {
            return User.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator) || OwnsTeam(id);
        }

        protected override bool CanDelete(int? id)
        {
            return CanPost(id);
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
    }
}
