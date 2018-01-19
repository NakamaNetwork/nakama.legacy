using System;

namespace TreasureGuide.Web.Models.ProfileModels
{
    public class AccessTokenModel
    {
        public string Token { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
