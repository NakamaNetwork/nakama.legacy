namespace TreasureGuide.Web.Models.BoxModels
{
    public class BoxSearchModel : SearchModel
    {
        public string UserId { get; set; }
        public bool? Blacklist { get; set; }
    }
}
