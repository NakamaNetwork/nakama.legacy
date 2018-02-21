using System;

namespace TreasureGuide.Web.Models.DonationModels
{
    public class DonationResultModel
    {
        public byte TransactionType { get; set; }
        public string TransactionId { get; set; }
        public string Error { get; set; }

        public bool HasError => !String.IsNullOrWhiteSpace(Error);
    }
}
