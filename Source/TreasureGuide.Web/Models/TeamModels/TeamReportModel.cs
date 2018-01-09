using System.ComponentModel.DataAnnotations;

namespace TreasureGuide.Web.Models.TeamModels
{
    public class TeamReportModel
    {
        public int TeamId { get; set; }

        [StringLength(100)]
        public string Reason { get; set; }
    }

    public class TeamReportStubModel
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string Reason { get; set; }
        public bool Acknowledged { get; set; }
    }
}
