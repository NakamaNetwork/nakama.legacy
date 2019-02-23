namespace NakamaNetwork.Entities.EnumTypes
{
    public enum PaymentState : byte
    {
        Unknown = 0,
        Initialized = 1,
        Processing = 2,
        Complete = 3,
        Failed = 4,
        Cancelled = 5,
        Chargeback = 6
    }
}
