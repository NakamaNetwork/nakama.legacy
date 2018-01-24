using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;
using TreasureGuide.Web.Constants;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Helpers;
using TreasureGuide.Web.Models;
using TreasureGuide.Web.Models.ProfileModels;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/profile")]
    public class ProfileContoller : SearchableApiController<string, UserProfile, string, ProfileStubModel, ProfileDetailModel, ProfileEditorModel, ProfileSearchModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileContoller(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(dbContext, autoMapper, throttlingService)
        {
            _userManager = userManager;
        }

        protected override async Task<object> PerformGet<TModel>(string id = null, bool required = false)
        {
            id = DefaultIfUnspecified(id, User.GetId());
            return await base.PerformGet<TModel>(id);
        }

        protected override bool CanPost(string id)
        {
            return base.CanPost(id) || id == User.GetId();
        }

        protected override async Task<ProfileEditorModel> PreProcess(ProfileEditorModel model)
        {
            if (User.IsInRole(RoleConstants.Administrator))
            {
                var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == model.Id);
                if (user == null)
                {
                    return model;
                }
                var roleList = model.UserRoles.ToList();
                if (User.GetId() == model.Id)
                {
                    roleList.Add(RoleConstants.Administrator);
                }
                var currentRoles = (await _userManager.GetRolesAsync(user)).ToList();
                var remove = currentRoles.Except(roleList).ToList();
                if (remove.Any())
                {
                    await _userManager.RemoveFromRolesAsync(user, remove);
                }
                var add = roleList.Except(currentRoles).ToList();
                if (add.Any())
                {
                    await _userManager.AddToRolesAsync(user, add);
                }
            }
            return await base.PreProcess(model);
        }

        protected override async Task<IQueryable<UserProfile>> PerformSearch(IQueryable<UserProfile> results, ProfileSearchModel model)
        {
            results = SearchTerm(results, model.Term);
            results = await SearchRoles(results, model.Roles);
            return results;
        }

        private async Task<IQueryable<UserProfile>> SearchRoles(IQueryable<UserProfile> results, IEnumerable<string> modelRoles)
        {
            if (modelRoles?.Any() ?? false)
            {
                var userIds = modelRoles
                    .SelectMany(x => x.Split(','))
                    .Select(x => _userManager.GetUsersInRoleAsync(x))
                    .SelectMany(x => x.Result)
                    .Select(y => y.Id).Distinct();
                results = results.Where(x => userIds.Contains(x.Id));
            }
            return results;
        }

        private IQueryable<UserProfile> SearchTerm(IQueryable<UserProfile> results, string modelTerm)
        {
            if (!String.IsNullOrWhiteSpace(modelTerm))
            {
                var terms = modelTerm.SplitSearchTerms();
                results = results.Where(x => terms.Any(t => x.UserName.Contains(t)));
            }
            return results;
        }
    }
}
