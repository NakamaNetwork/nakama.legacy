using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TreasureGuide.Entities;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Models.UnitModels;

namespace TreasureGuide.Web.Controllers.API
{
    public class UnitController : SearchableApiController<int, Unit, UnitStubModel, UnitDetailModel, UnitEditorModel, UnitSearchModel>
    {
        public UnitController(TreasureEntities dbContext, IMapper autoMapper) : base(dbContext, autoMapper)
        {
        }

        protected override IQueryable<Unit> PerformSearch(IQueryable<Unit> results, UnitSearchModel model)
        {
            results = SearchTerm(results, model.Term);
            results = SearchTypes(results, model.Types);
            results = SearchClasses(results, model.Classes, model.ForceTypes);
            results = SearchGlobal(results, model.Global);
            results = SearchBox(results, model.MyBox);
            return results;
        }

        protected override IQueryable<Unit> OrderSearchResults(IQueryable<Unit> results)
        {
            return results.OrderBy(x => x.Id);
        }

        private IQueryable<Unit> SearchBox(IQueryable<Unit> results, bool myBox)
        {
            if (myBox)
            {
                throw new System.NotImplementedException();
            }
            return results;
        }

        private IQueryable<Unit> SearchGlobal(IQueryable<Unit> results, bool global)
        {
            if (global)
            {
                results = results.Where(x => x.UnitFlags.Any(y => y.FlagType == UnitFlagType.Global));
            }
            return results;
        }

        private IQueryable<Unit> SearchClasses(IQueryable<Unit> results, IEnumerable<UnitClass> classes, bool force)
        {
            if (classes?.Any() ?? false)
            {
                if (force)
                {
                    results = results.Where(x => x.UnitClasses.All(y => classes.Contains(y.Class)));
                }
                else
                {
                    results = results.Where(x => x.UnitClasses.Any(y => classes.Contains(y.Class)));
                }
            }
            return results;
        }

        private IQueryable<Unit> SearchTypes(IQueryable<Unit> results, IEnumerable<UnitType> types)
        {
            if (types?.Any() ?? false)
            {
                results = results.Where(x => types.Contains(x.Type));
            }
            return results;
        }

        private IQueryable<Unit> SearchTerm(IQueryable<Unit> results, string term)
        {
            if (!String.IsNullOrEmpty(term))
            {
                results = results.Where(x => x.Name.Contains(term));
            }
            return results;
        }
    }
}
