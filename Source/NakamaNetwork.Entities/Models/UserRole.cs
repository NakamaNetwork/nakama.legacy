namespace NakamaNetwork.Entities.Models
{
    public partial class UserRole
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}
