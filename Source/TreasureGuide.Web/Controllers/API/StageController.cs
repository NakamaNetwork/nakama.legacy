using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;
using TreasureGuide.Web.Constants;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Models.StageModels;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/stage")]
    public class StageController : SearchableApiController<int, Stage, int?, StageStubModel, StageDetailModel, StageEditorModel, StageSearchModel>
    {
        public StageController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(dbContext, autoMapper, throttlingService)
        {
        }

        protected override async Task<IActionResult> Stub(int? id = default(int?))
        {
            return await Get<StageStubModel>(id, false);
        }

        protected override IQueryable<Stage> FetchEntities(int? id = null)
        {
            var queryable = DbContext.Stages.AsQueryable();
            if (id.HasValue)
            {
                queryable = queryable.Where(x => x.Id == id || x.OldId == id);
            }
            queryable = Filter(queryable);
            return queryable;
        }

        protected override async Task<IQueryable<Stage>> PerformSearch(IQueryable<Stage> results, StageSearchModel model)
        {
            results = SearchGlobal(results, model.Global);
            results = SearchType(results, model.Type);
            results = SearchTerm(results, model.Term);
            return results;
        }

        private IQueryable<Stage> SearchGlobal(IQueryable<Stage> results, bool global)
        {
            if (global)
            {
                results = results.Where(x => x.Global);
            }
            return results;
        }

        private IQueryable<Stage> SearchType(IQueryable<Stage> results, StageType? type)
        {
            if (type.HasValue)
            {
                results = results.Where(x => x.Type == type.Value);
            }
            return results;
        }

        private IQueryable<Stage> SearchTerm(IQueryable<Stage> results, string term)
        {
            if (!String.IsNullOrEmpty(term))
            {
                var terms = term.SplitSearchTerms();
                results = results.Where(x => terms.All(t => x.Name.Contains(t) || x.StageAliases.Any(y => y.Name.Contains(t))));
            }
            return results;
        }

        protected override IQueryable<Stage> OrderSearchResults(IQueryable<Stage> results, StageSearchModel model)
        {
            switch (model.SortBy ?? "")
            {
                case SearchConstants.SortId:
                    return results.OrderBy(x => x.Id, model.SortDesc);
                case SearchConstants.SortName:
                    return results.OrderBy(x => x.Name, model.SortDesc);
                case SearchConstants.SortType:
                    return results.OrderBy(x => x.Type, model.SortDesc);
                case SearchConstants.SortCount:
                    return results.OrderBy(x => x.Teams.Count(y => !y.Deleted && !y.Draft), model.SortDesc);
                default:
                    return results.OrderBy(x => x.Name, false);
            }
        }
    }
}
