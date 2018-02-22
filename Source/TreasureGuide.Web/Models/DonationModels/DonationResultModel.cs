using System;

namespace TreasureGuide.Web.Models.DonationModels
{
    public class DonationResultModel
    {
        public string TransactionId { get; set; }
        public string Error { get; set; }

        public DonationSubmissionModel Info { get; set; }

        public bool HasError => !String.IsNullOrWhiteSpace(Error);
    }
}
