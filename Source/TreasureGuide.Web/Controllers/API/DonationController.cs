using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;
using TreasureGuide.Web.Constants;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Helpers;
using TreasureGuide.Web.Models;
using TreasureGuide.Web.Models.DonationModels;
using TreasureGuide.Web.Services;
using TreasureGuide.Web.Services.Donations;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/donation")]
    public class DonationController : SearchableApiController<int, Donation, int?, DonationStubModel, DonationDetailModel, DonationEditorModel, DonationSearchModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDonationService _donationService;

        public DonationController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService, UserManager<ApplicationUser> userManager, IDonationService donationService) : base(dbContext, autoMapper, throttlingService)
        {
            _userManager = userManager;
            _donationService = donationService;
        }

        protected override IQueryable<Donation> Filter(IQueryable<Donation> entities)
        {
            entities = entities.Where(x => x.State == PaymentState.Complete);
            if (!User.IsInRole(RoleConstants.Administrator))
            {
                entities = entities.Where(x => x.UserId == User.GetId() || x.Public == true);
            }
            return entities;
        }

        [HttpPost]
        [ActionName("Prepare")]
        [Route("[action]")]
        public async Task<IActionResult> Prepare([FromBody] DonationSubmissionModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ConcatErrors();
                return BadRequest(errors);
            }
            var userId = User.GetId();
            model.Amount = Math.Round(model.Amount, 2);
            // Create the donation record.
            var donation = new Donation
            {
                Amount = model.Amount,
                Message = model.Message,
                Public = model.Public,
                PaymentType = model.PaymentType,
                PaymentId = "",
                PayerId = "",
                UserId = userId,
                State = PaymentState.Initialized,
                Date = DateTimeOffset.Now
            };
            DbContext.Donations.Add(donation);
            await DbContext.SaveChangesAsync();

            var payment = await _donationService.Prepare(model, donation.Id, userId, Request.GetUri().GetLeftPart(UriPartial.Authority));
            if (payment.HasError)
            {
                donation.State = PaymentState.Failed;
                await DbContext.SaveChangesAsync();
                return BadRequest(payment.Error);
            }
            donation.PaymentId = payment.PaymentId;
            donation.TokenId = payment.TokenId;
            donation.State = PaymentState.Processing;
            await DbContext.SaveChangesAsync();

            return Ok(payment);
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.Administrator)]
        [ActionName("RefreshAll")]
        [Route("[action]")]
        public async Task<IActionResult> RefreshAll()
        {
            var donations = DbContext.Donations.Where(x => x.State == PaymentState.Initialized || x.State == PaymentState.Processing || x.State == PaymentState.Processing);
            var results = await RefreshDonations(donations, false);
            return Ok(results);
        }

        [HttpPost]
        [ActionName("Refresh")]
        [Route("[action]")]
        public async Task<IActionResult> Refresh([FromBody] DonationVerificationModel model)
        {
            var donation = await GetDonation(model);
            var results = await RefreshDonations(new[] { donation }, true);
            return Ok(results.FirstOrDefault());
        }

        private async Task<IEnumerable<DonationResultModel>> RefreshDonations(IEnumerable<Donation> donations, bool push)
        {
            var results = new List<DonationResultModel>();
            var changed = false;
            foreach (var donation in donations)
            {
                var result = await _donationService.Refresh(donation.PaymentId, push);
                result.Id = donation.Id;
                result.UserId = donation.UserId;
                if (result.HasError)
                {
                    changed = true;
                    donation.State = PaymentState.Failed;
                }
                else
                {
                    if (result.State == PaymentState.Complete)
                    {
                        await GrantPerks(result.UserId);
                    }
                    if (result.State != donation.State)
                    {
                        changed = true;
                        donation.PaymentId = result.PaymentId;
                        donation.PayerId = result.PayerId;
                        donation.TokenId = result.TokenId;
                        donation.State = result.State;
                    }
                }
                results.Add(result);
            }
            if (changed)
            {
                await DbContext.SaveChangesAsync();
            }
            return results;
        }

        [HttpPost]
        [ActionName("Cancel")]
        [Route("[action]")]
        public async Task<IActionResult> Cancel([FromBody] DonationVerificationModel model)
        {
            var result = await GetDonation(model);
            if (result == null)
            {
                return BadRequest("Could not find donation record.");
            }
            result.State = PaymentState.Cancelled;
            await DbContext.SaveChangesAsync();

            return Ok(result);
        }

        private async Task<Donation> GetDonation(DonationVerificationModel model)
        {
            if (model.Id.HasValue)
            {
                return await DbContext.Donations.SingleOrDefaultAsync(x => x.Id == model.Id);
            }
            if (!String.IsNullOrWhiteSpace(model.TokenId))
            {
                return await DbContext.Donations.SingleOrDefaultAsync(x => x.TokenId == model.TokenId);
            }
            if (!String.IsNullOrWhiteSpace(model.PaymentId))
            {
                return await DbContext.Donations.SingleOrDefaultAsync(x => x.PaymentId == model.PaymentId);
            }
            return null;
        }

        private async Task<bool> GrantPerks(string id)
        {
            var user = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.SingleOrDefaultAsync(_userManager.Users, x => x.Id == id);
            if (user == null || await _userManager.IsInRoleAsync(user, RoleConstants.Donor))
            {
                return false;
            }
            await _userManager.AddToRoleAsync(user, RoleConstants.Donor);
            return true;
        }

        protected override bool CanPost(int? id)
        {
            return false;
        }

        protected override bool CanGet(int? id)
        {
            return false;
        }

        protected override async Task<IQueryable<Donation>> PerformSearch(IQueryable<Donation> results, DonationSearchModel model)
        {
            results = SearchUser(results, model.User);
            results = SearchAmount(results, model.MinAmount, model.MaxAmount);
            results = SearchDate(results, model.StartDate, model.EndDate);
            results = SearchComplex(results, model.Complex);
            return results;
        }

        private IQueryable<Donation> SearchComplex(IQueryable<Donation> results, bool complex)
        {
            if (!complex)
            {
                results = results.Where(x => x.Public == true && x.State == PaymentState.Complete);
            }
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

        protected override IQueryable<Donation> OrderSearchResults(IQueryable<Donation> results, DonationSearchModel model)
        {
            switch (model.SortBy)
            {
                case SearchConstants.SortUser:
                    return results.OrderBy(x => x.UserProfile.UserName, model.SortDesc);
                case SearchConstants.SortDate:
                    return results.OrderBy(x => x.Date, !model.SortDesc);
                case SearchConstants.SortAmount:
                    return results.OrderBy(x => x.Amount, !model.SortDesc);
                default:
                    return results.OrderBy(x => x.Date, true);
            }
        }
    }
}
