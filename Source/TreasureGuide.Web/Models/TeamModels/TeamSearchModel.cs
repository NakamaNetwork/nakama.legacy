namespace TreasureGuide.Web.Models.TeamModels
{
    public class TeamSearchModel : SearchModel
    {
        public string Term { get; set; }
        public string SubmittedBy { get; set; }
        public int? LeaderId { get; set; }
        public int? StageId { get; set; }

        public bool MyBox { get; set; }
        public bool Global { get; set; }
        public bool FreeToPlay { get; set; }

        public bool Deleted { get; set; }
        public bool Reported { get; set; }
    }
}
