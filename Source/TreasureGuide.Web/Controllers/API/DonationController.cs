using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.ApplicationServices;
using AutoMapper;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TreasureGuide.Entities;
using TreasureGuide.Web.Constants;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Helpers;
using TreasureGuide.Web.Models;
using TreasureGuide.Web.Models.DonationModels;
using TreasureGuide.Web.Services;
using TreasureGuide.Web.Services.Donations;

namespace TreasureGuide.Web.Controllers.API
{
    [Authorize]
    [Route("api/donation")]
    public class DonationController : SearchableApiController<int, Donation, int?, DonationStubModel, DonationDetailModel, DonationEditorModel, DonationSearchModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DonationController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService, UserManager<ApplicationUser> userManager) : base(dbContext, autoMapper, throttlingService)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [ActionName("Prepare")]
        [Route("[action]")]
        public async Task<IActionResult> Prepare([FromBody] DonationSubmissionModel model)
        {
            var userId = User.GetId();
            if (String.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            // Create the donation record.
            var donation = new Donation
            {
                Amount = model.Amount,
                Message = model.Message,
                Public = model.Public,
                TransactionType = model.ProviderType,
                TransactionId = "",
                UserId = userId,
                State = TransactionState.Initialized
            };
            DbContext.Donations.Add(donation);
            await DbContext.SaveChangesAsync();
            return Ok(donation.Id);
        }

        [HttpPost]
        [ActionName("Finalize")]
        [Route("[action]")]
        public async Task<IActionResult> Finalize([FromBody] DonationFinalizationModel model)
        {
            var result = await DbContext.Donations.SingleOrDefaultAsync(x => x.Id == model.Id);
            if (result == null)
            {
                return BadRequest("Could not find donation record.");
            }
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == User.GetId());
            if (user == null)
            {
                return Unauthorized();
            }
            result.TransactionId = model.PaymentId;
            result.State = TransactionState.Complete;

            if (User.IsInRole(RoleConstants.Donor))
            {
                return Ok("Thank you for your donation!");
            }
            await _userManager.AddToRoleAsync(user, RoleConstants.Donor);
            return Ok("Thank you for your donation! Please log out and back in to obtain your new donor perks!");
        }

        [HttpPost]
        [ActionName("Abort")]
        [Route("[action]")]
        public async Task<IActionResult> Abort([FromBody] DonationFinalizationModel model)
        {
            var result = await DbContext.Donations.SingleOrDefaultAsync(x => x.Id == model.Id);
            if (result == null)
            {
                return BadRequest("Could not find donation record.");
            }
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == User.GetId());
            if (user == null)
            {
                return Unauthorized();
            }
            result.TransactionId = model.PaymentId;
            result.State = TransactionState.Failed;

            return Ok("The transaction has been aborted.");
        }

        protected override bool CanPost(int? id)
        {
            return false;
        }

        protected override async Task<IQueryable<Donation>> PerformSearch(IQueryable<Donation> results, DonationSearchModel model)
        {
            results = SearchUser(results, model.User);
            results = SearchAmount(results, model.MinAmount, model.MaxAmount);
            results = SearchDate(results, model.StartDate, model.EndDate);
            return results;
        }

        private IQueryable<Donation> SearchAmount(IQueryable<Donation> results, decimal? modelMinAmount, decimal? modelMaxAmount)
        {
            if (modelMinAmount.HasValue)
            {
                results = results.Where(x => x.Amount >= modelMinAmount);
            }
            if (modelMaxAmount.HasValue)
            {
                results = results.Where(x => x.Amount <= modelMaxAmount);
            }
            return results;
        }

        private IQueryable<Donation> SearchDate(IQueryable<Donation> results, DateTimeOffset? modelStartDate, DateTimeOffset? modelEndDate)
        {
            if (modelStartDate.HasValue)
            {
                results = results.Where(x => x.Date >= modelStartDate);
            }
            if (modelEndDate.HasValue)
            {
                results = results.Where(x => x.Date <= modelEndDate);
            }
            return results;
        }

        private IQueryable<Donation> SearchUser(IQueryable<Donation> results, string modelUser)
        {
            if (!String.IsNullOrWhiteSpace(modelUser))
            {
                return results.Where(x => x.UserId == modelUser || x.UserProfile.UserName.Contains(modelUser));
            }
            return results;
        }
    }
}
