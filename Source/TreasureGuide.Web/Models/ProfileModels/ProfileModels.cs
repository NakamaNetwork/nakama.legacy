using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Models.ProfileModels
{
    public class ProfileStubModel : IIdItem<string>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public int? UnitId { get; set; }

        public int TeamCount { get; set; }
    }

    public class ProfileDetailModel : ProfileStubModel, ICanEdit
    {
        public string Website { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public bool CanEdit { get; set; }
    }

    public class MyProfileModel : ProfileDetailModel
    {
        public IDictionary<int, string> UserPreferences { get; set; }
        public int BoxCount { get; set; }
    }

    public class ProfileEditorModel : IIdItem<string>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public int? UnitId { get; set; }
        public string Website { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
    }

    public class ProfileSearchModel : SearchModel
    {
        public string Term { get; set; }
        public string[] Roles { get; set; }
    }
}
