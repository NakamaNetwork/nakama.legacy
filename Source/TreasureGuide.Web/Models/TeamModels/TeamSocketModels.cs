using System.Net.Sockets;

namespace TreasureGuide.Web.Models.TeamModels
{
    public abstract class TeamSocketModel
    {
        public SocketType SocketType { get; set; }
        public int Level { get; set; }
    }

    public class TeamSocketStubModel : TeamSocketModel { }

    public class TeamSocketDetailModel : TeamSocketModel { }

    public class TeamSocketEditorModel : TeamSocketModel { }
}
