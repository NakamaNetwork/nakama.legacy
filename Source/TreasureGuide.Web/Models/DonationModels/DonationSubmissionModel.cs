using System;
using System.ComponentModel.DataAnnotations;
using TreasureGuide.Entities;

namespace TreasureGuide.Web.Models.DonationModels
{
    public class DonationSubmissionModel
    {
        [Required]
        [Range(1.0, Double.MaxValue)]
        public decimal Amount { get; set; }
        public TransactionType ProviderType { get; set; } = TransactionType.Paypal;

        [StringLength(500)]
        public string Message { get; set; }
        public bool Public { get; set; }
    }
}
