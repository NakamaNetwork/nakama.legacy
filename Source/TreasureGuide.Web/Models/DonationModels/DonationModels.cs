using System;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Models.DonationModels
{
    public class DonationStubModel : IIdItem<int>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int? UnitId { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset Date { get; set; }
        public bool? Public { get; set; }
    }

    public class DonationDetailModel : DonationStubModel
    {
        public string Message { get; set; }
        public TransactionState State { get; set; }
    }

    public class DonationEditorModel : DonationDetailModel, IIdItem<int?>
    {
        public new int? Id { get; set; }
    }
}
