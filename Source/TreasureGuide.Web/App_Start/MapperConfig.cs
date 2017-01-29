using System;
using System.Linq.Expressions;
using AutoMapper;
using TreasureGuide.Entities;
using TreasureGuide.Web.Models.Stages;

namespace TreasureGuide.Web
{
    public static class MapperConfig
    {
        public static IMapper Create()
        {
            var config = new MapperConfiguration(map =>
            {
                map.CreateMap<SaveStageModel, Stage>().Ignore(x => x.StageDifficulties);
            });

            config.AssertConfigurationIsValid();
            return config.CreateMapper();
        }

        private static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map, Expression<Func<TDestination, object>> selector)
        {
            map.ForMember(selector, config => config.Ignore());
            return map;
        }
    }
}