using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
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
                results = results.Where(x => x.Name.Contains(term));
            }
            return results;
        }

        protected override IQueryable<Stage> OrderSearchResults(IQueryable<Stage> results, StageSearchModel model)
        {
            return results.OrderBy(x => x.Name);
        }
    }
}
