namespace TreasureGuide.Web.Models
{
    public class MetaResultModel
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public const string DefaultTitle = "Nakama Network";
        public const string DefaultDescription = "One Piece: Treasure Cruise teams, guides, box management, and more!";
        public const string StateKey = "meta_path_uri";
    }
}