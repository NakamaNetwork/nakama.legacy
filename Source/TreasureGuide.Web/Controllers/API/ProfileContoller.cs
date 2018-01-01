using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Models.ProfileModels;

namespace TreasureGuide.Web.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProfileContoller : EntityApiController<string, UserProfile, string, UserProfileStubModel, UserProfileDetailModel, UserProfileEditorModel>
    {
        public ProfileContoller(TreasureEntities dbContext, IMapper autoMapper) : base(dbContext, autoMapper)
        {
        }
    }
}
