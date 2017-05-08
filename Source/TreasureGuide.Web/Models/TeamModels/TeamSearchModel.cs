namespace TreasureGuide.Web.Models.TeamModels
{
    public class TeamSearchModel
    {
        public string Team { get; set; }
        public int? LeaderId { get; set; }
        public int? StageId { get; set; }

        public bool MyBox { get; set; }
        public bool Global { get; set; }

        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 25;
    }
}
