using System;
using System.Collections.Generic;
using NakamaNetwork.Entities.EnumTypes;

namespace NakamaNetwork.Entities.Models
{
    public partial class UserPreference
    {
        public string UserId { get; set; }
        public UserPreferenceType Key { get; set; }
        public string Value { get; set; }

        public virtual UserProfile User { get; set; }
    }
}
