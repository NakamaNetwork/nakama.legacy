using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TreasureGuide.Entities;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Helpers;
using TreasureGuide.Web.Models;
using TreasureGuide.Web.Models.ProfileModels;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    public class ProfileContoller : SearchableApiController<string, UserProfile, string, UserProfileStubModel, UserProfileDetailModel, UserProfileEditorModel, UserProfileSearchModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileContoller(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService, UserManager<ApplicationUser> userManager) : base(dbContext, autoMapper, throttlingService)
        {
            _userManager = userManager;
        }

        protected override async Task<object> PerformGet<TModel>(string id = null)
        {
            id = DefaultIfUnspecified(id, User.GetId());
            return await base.PerformGet<TModel>(id);
        }

        protected override bool CanPost(string id)
        {
            return base.CanPost(id) || id == User.GetId();
        }

        protected override async Task<IQueryable<UserProfile>> PerformSearch(IQueryable<UserProfile> results, UserProfileSearchModel model)
        {
            results = SearchTerm(results, model.Term);
            results = await SearchRoles(results, model.Roles);
            return results;
        }

        private async Task<IQueryable<UserProfile>> SearchRoles(IQueryable<UserProfile> results, IEnumerable<string> modelRoles)
        {
            if (modelRoles.Any())
            {
                var userIds = modelRoles.Select(async x => await _userManager.GetUsersInRoleAsync(x))
                    .SelectMany(x => x.Result).Select(y => y.Id).Distinct();
                results = results.Where(x => userIds.Contains(x.Id));
            }
            return results;
        }

        private IQueryable<UserProfile> SearchTerm(IQueryable<UserProfile> results, string modelTerm)
        {
            if (!String.IsNullOrWhiteSpace(modelTerm))
            {
                results = results.Where(x => x.UserName.Contains(modelTerm));
            }
            return results;
        }
    }
}
