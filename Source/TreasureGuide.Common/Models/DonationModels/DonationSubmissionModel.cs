using System;
using System.ComponentModel.DataAnnotations;
using TreasureGuide.Entities;

namespace TreasureGuide.Common.Models.DonationModels
{
    public class DonationSubmissionModel
    {
        [Required]
        [Range(1.0, Double.MaxValue)]
        public decimal Amount { get; set; }
        public PaymentType PaymentType { get; set; } = PaymentType.Paypal;

        [StringLength(500)]
        public string Message { get; set; }
        public bool Public { get; set; }
    }
}
