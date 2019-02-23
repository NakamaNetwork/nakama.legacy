using System.Collections.Generic;

namespace TreasureGuide.Common.Models.TeamModels
{
    public class TeamImportModel
    {
        public TeamEditorModel Team { get; set; }
        public TeamCreditModel Credit { get; set; }
        public IEnumerable<TeamVideoModel> Videos { get; set; }
    }
}
