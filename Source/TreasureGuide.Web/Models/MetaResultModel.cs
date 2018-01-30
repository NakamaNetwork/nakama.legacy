namespace TreasureGuide.Web.Models
{
    public class MetaResultModel
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public const string DefaultTitle = "Nakama Network";
        public const string DefaultDescription = "The premiere team-building and sharing website for One Piece: Treasure Cruise.";
        public const string StateKey = "meta_path_uri";
    }
}