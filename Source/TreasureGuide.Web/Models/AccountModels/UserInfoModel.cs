using System.Collections.Generic;

namespace TreasureGuide.Web.Models.AccountModels
{
    public class UserInfoModel
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
