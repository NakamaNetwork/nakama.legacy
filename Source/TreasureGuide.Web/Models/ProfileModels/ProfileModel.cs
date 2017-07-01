using System.Collections.Generic;

namespace TreasureGuide.Web.Models.ProfileModels
{
    public class ProfileModel
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
