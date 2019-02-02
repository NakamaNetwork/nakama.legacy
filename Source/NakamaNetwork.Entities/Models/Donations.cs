using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class Donations
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string Message { get; set; }
        public DateTimeOffset Date { get; set; }
        public byte State { get; set; }
        public byte PaymentType { get; set; }
        public string PaymentId { get; set; }
        public string PayerId { get; set; }
        public string TokenId { get; set; }
        public bool? Public { get; set; }

        public virtual UserProfiles User { get; set; }
    }
}
