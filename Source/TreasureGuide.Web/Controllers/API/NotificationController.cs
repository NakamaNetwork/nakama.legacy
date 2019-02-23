using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Common.Helpers;
using TreasureGuide.Common.Models.NotificationModels;
using NakamaNetwork.Entities;
using TreasureGuide.Web.Helpers;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    [Authorize]
    [Route("api/notifications")]
    public class NotificationController : Controller
    {
        protected readonly TreasureEntities DbContext;
        protected readonly IMapper AutoMapper;
        protected readonly IThrottleService ThrottlingService;

        public bool Throttled { get; set; } = true;

        public NotificationController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService)
        {
            DbContext = dbContext;
            AutoMapper = autoMapper;
            ThrottlingService = throttlingService;
        }

        [HttpGet]
        [Authorize]
        [ActionName("")]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            if (Throttled && !ThrottlingService.CanAccess(User, Request, Request.Path))
            {
                return StatusCode(429, ThrottleService.Message);
            }
            var id = User.GetId();
            if (String.IsNullOrWhiteSpace(User.GetId()))
            {
                return Unauthorized();
            }
            var notifications =  await DbContext.Notifications
                .Where(x => x.UserId == id)
                .OrderBy(x => x.Id)
                .ProjectTo<NotificationModel>(AutoMapper.ConfigurationProvider)
                .ToListAsync();
            return Ok(notifications);
        }

        [HttpGet]
        [Authorize]
        [ActionName("Count")]
        [Route("[action]")]
        public async Task<IActionResult> Count()
        {
            if (Throttled && !ThrottlingService.CanAccess(User, Request, Request.Path))
            {
                return StatusCode(429, ThrottleService.Message);
            }
            var id = User.GetId();
            if (String.IsNullOrWhiteSpace(User.GetId()))
            {
                return Unauthorized();
            }
            var notifications = await DbContext.Notifications
                .Where(x => x.UserId == id)
                .CountAsync();
            return Ok(notifications);
        }

        [HttpDelete]
        [Authorize]
        [ActionName("")]
        [Route("{id?}")]
        public async Task<IActionResult> Delete(int? id = null)
        {
            var userId = User.GetId();
            if (String.IsNullOrWhiteSpace(User.GetId()))
            {
                return Unauthorized();
            }
            var notifications = DbContext.Notifications
                .Where(x => x.UserId == userId);
            if (id != null)
            {
                notifications = notifications.Where(x => x.Id == id);
            }
            DbContext.Notifications.RemoveRange(notifications);
            await DbContext.SaveChangesSafe();
            return Ok(1);
        }
    }
}

