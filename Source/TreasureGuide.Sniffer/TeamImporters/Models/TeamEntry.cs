using System;
using TreasureGuide.Entities;

namespace TreasureGuide.Sniffer.TeamImporters.Models
{
    public class TeamEntry
    {
        public string Name { get; set; }
        public string CalcLink { get; set; }
        public string Desc { get; set; }
        public string Credit { get; set; }
        public string CreditReference { get; set; }
        public int? StageId { get; set; }
        public TeamCreditType CreditType { get; set; }
        public string Video { get; set; }

        public string ParseOut()
        {
            return String.Join("Ξ",
                Name, // name
                CalcLink, // calc
                StageId, // stage
                Desc, // desc
                Credit, // credit
                Video, // videos
                CreditReference, // ref
                (int)CreditType // type
            );
        }
    }
}
