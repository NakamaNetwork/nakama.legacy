namespace NakamaNetwork.Entities.Models
{
    public partial class UserRoleSummary
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}
