using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using PayPal.Api;
using NakamaNetwork.Entities.Models;
using TreasureGuide.Common.Models.DonationModels;

namespace TreasureGuide.Web.Services.Donations
{
    public class PaypalDonationService : DonationService
    {
        public override PaymentType PaymentType { get; } = PaymentType.Paypal;

        public PaypalDonationService(IHostingEnvironment env, IConfigurationRoot config, NakamaNetworkContext entities) : base(env, config, entities)
        {
        }

        protected override async Task<DonationResultModel> DoPreparation(DonationSubmissionModel model, int id, string userId, string urlRoot, DonationResultModel result)
        {
            var userInfo = await GetUserInfo(userId) ?? new UserPaymentInfo
            {
                UserName = "Anonymous",
                Email = "",
                Id = ""
            };
            var amt = Math.Round(model.Amount, 2).ToString("0.00");
            var context = GetContext();
            var payment = new Payment
            {
                experience_profile_id = Config["Authentication:PayPal:ProfileID"],
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
                            total = amt,
                            details = new Details
                            {
                                subtotal = amt
                            }
                        },
                        item_list = new ItemList
                        {
                            items = new List<Item>
                            {
                                new Item
                                {
                                    name = "Nakama Network Donation",
                                    price = amt,
                                    quantity = "1",
                                    currency = "USD"
                                }
                            }
                        }
                    }
                },
                redirect_urls = new RedirectUrls
                {
                    return_url = urlRoot + "/donate/update",
                    cancel_url = urlRoot + "/donate/cancel"
                }
            };

            try
            {
                var serverPayment = Payment.Create(context, payment);
                result.RedirectUrl = serverPayment.links.First(x => x.rel == "approval_url").href;
                result.PaymentId = serverPayment.id;
                result.TokenId = serverPayment.token;
            }
            catch
            {
                result.Error = "An error has occurred trying to submit your payment. Please try again later. " +
                               "You have not been charged.";
            }
            result.State = GetState(payment.state);
            return result;
        }

        protected override async Task<DonationResultModel> DoRefresh(string paymentId, DonationResultModel result, bool push)
        {
            var context = GetContext();
            var payment = Payment.Get(context, paymentId);
            if (payment != null)
            {
                if (push && payment.state == "created")
                {
                    try
                    {
                        var payExecute = new PaymentExecution
                        {
                            payer_id = payment.payer?.payer_info?.payer_id,
                            transactions = payment.transactions
                        };
                        payment = Payment.Execute(context, paymentId, payExecute);
                    }
                    catch
                    {
                        result.Error = "An error has occurred trying to finalize your payment. Please try again later. " +
                                       "You have not been charged";
                    }
                }
                result.PaymentId = payment.id;
                result.PayerId = payment.payer?.payer_info?.payer_id;
                result.TokenId = payment.token;
            }
            result.State = GetState(payment?.state);
            return result;
        }

        private PaymentState GetState(string paymentState)
        {
            switch (paymentState)
            {
                case "created":
                    return PaymentState.Initialized;
                case "approved":
                    return PaymentState.Complete;
                case "failed":
                    return PaymentState.Failed;
                case "canceled":
                    return PaymentState.Failed;
                case "expired":
                    return PaymentState.Cancelled;
                default:
                    return PaymentState.Unknown;
            }
        }

        private APIContext GetContext()
        {
            var config = GetConfig();
            var token = new OAuthTokenCredential(config).GetAccessToken();
            var context = new APIContext(token) { Config = config };
            return context;
        }

        private Dictionary<string, string> GetConfig()
        {
            return new Dictionary<string, string>
            {
                {"mode", IsTesting ? "sandbox" : "live"},
                {"clientId", Config["Authentication:PayPal:ClientID"]},
                {"clientSecret", Config["Authentication:PayPal:ClientSecret"]}
            };
        }
    }
}