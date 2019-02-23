using System;
using System.Collections.Generic;
using NakamaNetwork.Entities.Interfaces;

namespace TreasureGuide.Common.Models.TeamModels
{
    public class WikiSearchResultModel : IIdItem<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubmittedById { get; set; }
        public string SubmittedByName { get; set; }
        public DateTimeOffset? EditedDate { get; set; }
        public int Score { get; set; }

        public int ShipId { get; set; }
        public string ShipName { get; set; }
        public int? StageId { get; set; }
        public string StageName { get; set; }
        public int? InvasionId { get; set; }
        public string InvasionName { get; set; }

        public IEnumerable<WikiSearchUnitStubModel> TeamUnits { get; set; }
    }

    public class WikiSearchUnitStubModel : TeamUnitStubModel
    {
        public string UnitName { get; set; }
    }
}
