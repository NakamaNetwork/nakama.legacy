using System;

namespace TreasureGuide.Common.Models.DonationModels
{
    public class DonationSearchModel : SearchModel
    {
        public string User { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public bool Complex { get; set; }
    }
}
