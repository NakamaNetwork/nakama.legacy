using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using PayPal.Api;
using TreasureGuide.Entities;
using TreasureGuide.Web.Models.DonationModels;

namespace TreasureGuide.Web.Services.Donations
{
    public class PaypalDonationService : DonationService
    {
        public PaypalDonationService(IHostingEnvironment env, IConfigurationRoot config, TreasureEntities entities) : base(env, config, entities)
        {
        }

        protected override async Task<DonationResultModel> DoPreparation(DonationSubmissionModel model, int id, string userId, string urlRoot, DonationResultModel result)
        {
            var userInfo = await GetUserInfo(userId);
            if (userInfo == null)
            {
                result.Error = "No user found.";
                return result;
            }
            var amt = Math.Round(model.Amount, 2).ToString("0.00");
            var context = GetContext();
            var payment = new Payment
            {
                intent = "sale",
                payer = new Payer
                {
                    payment_method = "paypal",
                    payer_info = new PayerInfo
                    {
                        first_name = userInfo.UserName,
                        last_name = userInfo.Id,
                        email = userInfo.Email
                    }
                },
                transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        description = "Nakama Network Donation",
                        invoice_number = id.ToString(),
                        amount = new Amount
                        {
                            currency = "USD",
                            total = amt
                        }
                    }
                },
                redirect_urls = new RedirectUrls
                {
                    return_url = urlRoot + "/donate/complete",
                    cancel_url = urlRoot + "/donate/cancel"
                }
            };

            try
            {
                var serverPayment = Payment.Create(context, payment);
                result.RedirectUrl = serverPayment.links.First().href;
                result.TransactionId = serverPayment.id;
                result.Info = model;
            }
            catch
            {
                result.Error = "An error has occurred trying to submit your payment. Please try again later.";
            }
            return result;
        }

        private APIContext GetContext()
        {
            var config = GetConfig();
            var token = new OAuthTokenCredential(config).GetAccessToken();
            var context = new APIContext(token);
            return context;
        }

        private Dictionary<string, string> GetConfig()
        {
            return new Dictionary<string, string>
            {
                {"mode", IsTesting ? "sandbox" : "live" },
                {"clientId",Config["Authentication:PayPal:ClientID"] },
                {"clientSecret",Config["Authentication:PayPal:ClientSecret"] }
            };
        }
    }
}
