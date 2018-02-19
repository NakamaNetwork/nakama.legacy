namespace TreasureGuide.Web.Models
{
    public class MetaResultModel
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public const string DefaultTitle = "Nakama Network";
        public const string DefaultDescription =
            "One Piece: Treasure Cruise teams, guides, videos, box management, and more! " +
            "Find the latest guides and teams for Bandai Namco's hit mobile RPG. " +
            "Manage your units and find teams to clear the hardest content or share " +
            "your own creations with the community! Register for free today!";
        public const string StateKey = "meta_path_uri";
    }
}