using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using TreasureGuide.Entities;
using TreasureGuide.Web.Models.DonationModels;

namespace TreasureGuide.Web.Services.Donations
{
    public interface IDonationService
    {
        string WebRoot { get; }
        PaymentType PaymentType { get; }
        Task<DonationResultModel> Prepare(DonationSubmissionModel model, int id, string userId, string urlRoot);
        Task<DonationResultModel> Refresh(string paymentId);
    }

    public abstract class DonationService : IDonationService
    {
        protected IConfigurationRoot Config;
        protected bool IsTesting;
        protected TreasureEntities Entities;
        public string WebRoot { get; }
        public abstract PaymentType PaymentType { get; }

        protected DonationService(IHostingEnvironment env, IConfigurationRoot config, TreasureEntities entities)
        {
            IsTesting = env.IsDevelopment();
            Config = config;
            Entities = entities;
        }

        public async Task<DonationResultModel> Prepare(DonationSubmissionModel model, int id, string userId, string urlRoot)
        {
            var result = new DonationResultModel
            {
                Id = id,
                UserId = userId,
                Error = Validate(model, userId),
                PaymentType = PaymentType
            };
            if (result.HasError)
            {
                return result;
            }
            result = await DoPreparation(model, id, userId, urlRoot, result);
            return result;
        }

        public async Task<DonationResultModel> Refresh(string paymentId)
        {
            var result = new DonationResultModel
            {
                PaymentId = paymentId,
                PaymentType = PaymentType
            };
            result = await DoRefresh(paymentId, result);
            return result;
        }

        protected abstract Task<DonationResultModel> DoPreparation(DonationSubmissionModel model, int id, string userId, string urlRoot, DonationResultModel result);
        protected abstract Task<DonationResultModel> DoRefresh(string paymentId, DonationResultModel result);

        protected virtual string Validate(DonationSubmissionModel model, string userId)
        {
            if (model.Amount < 1.0m)
            {
                return "Donation amount must be at least $1.00.";
            }
            return null;
        }

        protected async Task<UserPaymentInfo> GetUserInfo(string userId)
        {
            var userInfo = await Entities.UserProfiles.Where(x => x.Id == userId).Select(x => new UserPaymentInfo
            {
                Id = userId,
                UserName = x.UserName,
                Email = x.AspNetUser.Email
            }).SingleOrDefaultAsync();
            return userInfo;
        }

        protected class UserPaymentInfo
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
        }
    }
}
