using AutoMapper;
using TreasureGuide.Entities;

namespace TreasureGuide.Web.Models.Stages
{
    public class SaveDifficultyModel
    {
        public int Id { get; set; }
        public int StageId { get; set; }
        public string Name { get; set; }
        public int Stamina { get; set; }
        public bool Global { get; set; }

        public static void Map(IMapperConfigurationExpression map)
        {
            map.CreateMap<SaveDifficultyModel, StageDifficulty>()
                .Ignore(x => x.Stage)
                .Ignore(x => x.Teams)
                .Ignore(x => x.StageLevels);
        }
    }
}