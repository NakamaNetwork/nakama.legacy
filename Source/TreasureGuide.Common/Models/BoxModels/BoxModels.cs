using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NakamaNetwork.Entities.Interfaces;

namespace TreasureGuide.Common.Models.BoxModels
{
    public class BoxStubModel : IIdItem<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? FriendId { get; set; }
        public bool? Global { get; set; }
        public bool Public { get; set; }
    }

    public class BoxDetailModel : BoxStubModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool UserIsDonor { get; set; }
        public int? UserUnitId { get; set; }
        public IEnumerable<BoxUnitDetailModel> BoxUnits { get; set; }
    }

    public class BoxEditorModel : IIdItem<int?>
    {
        public int? Id { get; set; }

        [StringLength(250, MinimumLength = 5)]
        public string Name { get; set; }

        public decimal? FriendId { get; set; }
        public bool? Global { get; set; }
        public bool Public { get; set; }
    }
}
