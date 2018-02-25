using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;
using TreasureGuide.Web.Constants;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Models.TeamModels;
using TreasureGuide.Web.Models.UnitModels;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/unit")]
    public class UnitController : SearchableApiController<int, Unit, int?, UnitStubModel, UnitDetailModel, UnitEditorModel, UnitSearchModel>
    {
        public UnitController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(dbContext, autoMapper, throttlingService)
        {
        }

        protected override async Task<IQueryable<Unit>> PerformSearch(IQueryable<Unit> results, UnitSearchModel model)
        {
            results = SearchTerm(results, model.Term);
            results = SearchTypes(results, model.Types);
            results = SearchClasses(results, model.Classes, model.ForceClass);
            results = SearchGlobal(results, model.Global);
            results = SearchBox(results, model.BoxId, model.Blacklist);
            results = SearchFreeToPlay(results, model.FreeToPlay);
            return results;
        }


        protected override IQueryable<Unit> OrderSearchResults(IQueryable<Unit> results, UnitSearchModel model)
        {
            switch (model.SortBy ?? "")
            {
                case SearchConstants.SortId:
                    return results.OrderBy(x => x.Id, model.SortDesc);
                case SearchConstants.SortName:
                    return results.OrderBy(x => x.Name, model.SortDesc);
                case SearchConstants.SortType:
                    return results.OrderBy(x => x.Type, model.SortDesc).ThenBy(x => x.Stars ?? 0, true);
                case SearchConstants.SortClass:
                    return results.OrderBy(x => x.Class, model.SortDesc).ThenBy(x => x.Stars ?? 0, true);
                case SearchConstants.SortStars:
                    return results.OrderBy(x => x.Stars ?? 0, model.SortDesc).ThenBy(x => x.Type, false);
                default:
                    return results.OrderBy(x => x.Id, false);
            }
        }

        private IQueryable<Unit> SearchFreeToPlay(IQueryable<Unit> results, bool freeToPlay)
        {
            if (freeToPlay)
            {
                results = results.Where(x => !EnumerableHelper.PayToPlay.Any(y => x.Flags.HasFlag(y)));
            }
            return results;
        }

        private IQueryable<Unit> SearchBox(IQueryable<Unit> results, int? boxId, bool? blacklist)
        {
            if (boxId.HasValue)
            {
                if (blacklist ?? false)
                {
                    results = results.Where(x => !x.BoxUnits.Any(y => y.BoxId == boxId && y.Box.Blacklist));
                }
                else
                {
                    results = results.Where(x => x.BoxUnits.Any(y => y.BoxId == boxId && !y.Box.Blacklist));
                }
            }
            return results;
        }

        private IQueryable<Unit> SearchGlobal(IQueryable<Unit> results, bool global)
        {
            if (global)
            {
                results = results.Where(x => x.Flags.HasFlag(UnitFlag.Global));
            }
            return results;
        }

        private IQueryable<Unit> SearchClasses(IQueryable<Unit> results, UnitClass? classes, bool force)
        {
            if (classes.HasValue && classes != UnitClass.Unknown)
            {
                if (force)
                {
                    results = results.Where(x => x.Class == classes);
                }
                else
                {
                    results = results.Where(x => (x.Class & classes) != 0);
                }
            }
            return results;
        }

        private IQueryable<Unit> SearchTypes(IQueryable<Unit> results, UnitType? types)
        {
            if (types.HasValue && types != UnitType.Unknown)
            {
                results = results.Where(x => (x.Type & types) != 0);
            }
            return results;
        }

        private IQueryable<Unit> SearchTerm(IQueryable<Unit> results, string term)
        {
            if (!String.IsNullOrEmpty(term))
            {
                var terms = term.SplitSearchTerms();
                results = results.Where(x => terms.All(t => x.Name.Contains(t) || x.UnitAliases.Any(y => y.Name.Contains(t))));
            }
            return results;
        }
    }
}
