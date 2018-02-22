using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Helpers;
using TreasureGuide.Web.Models.DonationModels;
using TreasureGuide.Web.Services;
using TreasureGuide.Web.Services.Donations;

namespace TreasureGuide.Web.Controllers.API
{
    [Authorize]
    [Route("api/donation")]
    public class DonationController : SearchableApiController<int, Donation, int?, DonationStubModel, DonationDetailModel, DonationEditorModel, DonationSearchModel>
    {
        private readonly IDonationService _donationService;

        public DonationController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService, IDonationService donationService) : base(dbContext, autoMapper, throttlingService)
        {
            _donationService = donationService;
        }

        [HttpPost]
        [ActionName("Donate")]
        [Route("[action]")]
        public async Task<IActionResult> Donate([FromBody] DonationSubmissionModel model)
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

            // Send it to PayPal
            var url = Request.GetUri().GetLeftPart(UriPartial.Authority);
            try
            {
                var result = await _donationService.Process(model, donation.Id, userId, url);
            }
            catch
            {
                donation.State = TransactionState.Failed;
                await DbContext.SaveChangesAsync();
                return BadRequest("An error has occurred processing your payment. Please try again later.");
            }
            if (result.HasError)
            {
                donation.State = TransactionState.Failed;
                await DbContext.SaveChangesAsync();
                return BadRequest(result.Error);
            }
            donation.State = TransactionState.Processing;
            donation.TransactionId = result.TransactionId;
            await DbContext.SaveChangesAsync();
            return Ok(donation.Id);
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
