using System.ComponentModel.DataAnnotations;
using TreasureGuide.Entities;

namespace TreasureGuide.Web.Models.TeamModels
{
    public class TeamCreditModel
    {
        [StringLength(450)]
        public string Credit { get; set; }
        public TeamCreditType Type { get; set; }
    }
}
