using System.Collections.Generic;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Models.ProfileModels
{
    public class ProfileAuthExportModel
    {
        public string UserName { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }

    public class ProfileStubModel : IIdItem<string>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public int? UnitId { get; set; }

        public int TeamCount { get; set; }
    }

    public class ProfileDetailModel : ProfileStubModel, ICanEdit
    {
        public decimal? FriendId { get; set; }
        public string Website { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public bool? Global { get; set; }
        public bool CanEdit { get; set; }
    }

    public class ProfileEditorModel : IIdItem<string>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public int? UnitId { get; set; }
        public decimal? FriendId { get; set; }
        public string Website { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public bool? Global { get; set; }
    }

    public class ProfileSearchModel : SearchModel
    {
        public string Term { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
