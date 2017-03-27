using System.Collections.Generic;
using TreasureGuide.Entities;

namespace TreasureGuide.Web.Models.Teams
{
    public class SaveTeamModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Credits { get; set; }
        public IEnumerable<TeamUnitModel> Units { get; set; }
        public IEnumerable<int> Stages { get; set; }
        public IEnumerable<TeamSocketModel> Sockets { get; set; }

        public class TeamUnitModel
        {
            public int Id { get; set; }
            public int Position { get; set; }
            public bool SpecialLevel { get; set; }
            public bool Sub { get; set; }
        }

        public class TeamSocketModel
        {
            public SocketType SocketType { get; set; }
            public int Level { get; set; }
        }
    }
}