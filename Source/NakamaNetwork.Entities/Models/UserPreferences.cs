using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class UserPreferences
    {
        public string UserId { get; set; }
        public int Key { get; set; }
        public string Value { get; set; }

        public virtual UserProfiles User { get; set; }
    }
}
