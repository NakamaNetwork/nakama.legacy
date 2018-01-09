using System.ComponentModel.DataAnnotations;

namespace TreasureGuide.Web.Models.TeamModels
{
    public class TeamReportModel
    {
        public int TeamId { get; set; }

        [StringLength(100)]
        public string Reason { get; set; }
    }
}
