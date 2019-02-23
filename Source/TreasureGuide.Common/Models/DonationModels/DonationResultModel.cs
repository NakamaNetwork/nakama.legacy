using System;
using NakamaNetwork.Entities;
using NakamaNetwork.Entities.EnumTypes;

namespace TreasureGuide.Common.Models.DonationModels
{
    public class DonationResultModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string PayerId { get; set; }
        public string PaymentId { get; set; }
        public string TokenId { get; set; }
        public PaymentState State { get; set; }
        public PaymentType PaymentType { get; set; }

        public string Error { get; set; }
        public string RedirectUrl { get; set; }

        public bool HasError => !String.IsNullOrWhiteSpace(Error);
    }
}
