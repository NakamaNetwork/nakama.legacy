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
        byte TransactionType { get; }
        Task<DonationResultModel> Process(DonationSubmissionModel model, int id, string userId, string urlRoot);
        Task<DonationResultModel> Execute(string transactionId);
    }

    public abstract class DonationService : IDonationService
    {
        protected IConfigurationRoot Config;
        protected bool IsTesting;
        protected TreasureEntities Entities;
        public string WebRoot { get; }
        public byte TransactionType { get; } = 1;

        protected DonationService(IHostingEnvironment env, IConfigurationRoot config, TreasureEntities entities)
        {
            IsTesting = env.IsDevelopment();
            Config = config;
            Entities = entities;
        }

        public async Task<DonationResultModel> Process(DonationSubmissionModel model, int id, string userId, string urlRoot)
        {
            var result = new DonationResultModel
            {
                Error = Validate(model, userId)
            };
            if (result.HasError)
            {
                return result;
            }
            try
            {
                result = await DoPreparation(model, id, userId, urlRoot, result);
            }
            catch (Exception e)
            {
                result.Error = e.Message;
            }
            return result;
        }

        protected abstract Task<DonationResultModel> DoPreparation(DonationSubmissionModel model, int id, string userId, string urlRoot, DonationResultModel result);
        public abstract Task<DonationResultModel> Execute(string transactionId);

        protected virtual string Validate(DonationSubmissionModel model, string userId)
        {
            if (String.IsNullOrWhiteSpace(userId))
            {
                return "Could not find current user.";
            }
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
