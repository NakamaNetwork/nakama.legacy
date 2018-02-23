using System;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Models.DonationModels
{
    public class DonationStubModel : IIdItem<int>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int? UserUnitId { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset Date { get; set; }
        public bool? Public { get; set; }
        public string Message { get; set; }
    }

    public class DonationDetailModel : DonationStubModel
    {
    }

    public class DonationEditorModel : IIdItem<int?>
    {
        public int? Id { get; set; }
    }
}
