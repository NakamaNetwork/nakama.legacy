using System;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Models.TeamModels
{
    public class TeamVideoModel : IIdItem<int?>
    {
        public int? Id { get; set; }
        public int TeamId { get; set; }
        public string VideoLink { get; set; }
        public DateTimeOffset SubmittedDate { get; set; }
        public bool Deleted { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public int? UserUnitId { get; set; }
    }
}
