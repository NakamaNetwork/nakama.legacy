using NakamaNetwork.Entities;
using NakamaNetwork.Entities.EnumTypes;

namespace TreasureGuide.Common.Models.TeamModels
{
    public abstract class TeamSocketModel
    {
        public SocketType SocketType { get; set; }
        public byte Level { get; set; }
    }

    public class TeamSocketStubModel : TeamSocketModel { }

    public class TeamSocketDetailModel : TeamSocketModel { }

    public class TeamSocketEditorModel : TeamSocketModel { }
}
