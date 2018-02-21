using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Models.DonationModels;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    [Authorize]
    [Route("api/payment")]
    public class DonationController : SearchableApiController<int, Donation, int?, DonationStubModel, DonationDetailModel, DonationEditorModel, DonationSearchModel>
    {
        public DonationController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(dbContext, autoMapper, throttlingService)
        {
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
