﻿using System.Collections.Generic;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Models.ProfileModels
{
    public class ProfileAuthExport
    {
        public string UserName { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }

    public class UserProfileStubModel : IIdItem<string>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public int? UnitId { get; set; }
    }

    public class UserProfileDetailModel : UserProfileStubModel
    {
        public string AccountNumber { get; set; }
        public string Website { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
