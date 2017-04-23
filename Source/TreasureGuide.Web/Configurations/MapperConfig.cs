using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TreasureGuide.Entities;
using TreasureGuide.Web.Models.UnitModels;

namespace TreasureGuide.Web.Configurations
{
    public static class MapperConfig
    {
        public static IMapper Create()
        {
            var config = new MapperConfiguration(mapper =>
            {
                var mapping = mapper.CreateControllerMapping<Unit, UnitDetailModel, UnitStubModel, UnitEditorModel>();
                mapping.StubMapping.ForMember(x => x.Global, o => o.MapFrom(y => y.UnitFlags.Any(z => z.FlagType == UnitFlagType.Global)));
                mapping.StubMapping.ForMember(x => x.UnitClasses, o => o.MapFrom(y => y.UnitClasses.Select(z => z.Class)));

                mapping.DetailMapping.ForMember(x => x.UnitFlags, o => o.MapFrom(y => y.UnitFlags.Select(z => z.FlagType)));
                mapping.DetailMapping.ForMember(x => x.UnitClasses, o => o.MapFrom(y => y.UnitClasses.Select(z => z.Class)));
            });
            config.AssertConfigurationIsValid();
            return config.CreateMapper();
        }

        private static MapperSet<TEntity, TDetailModel, TStubModel, TEditorModel> CreateControllerMapping<TEntity, TDetailModel, TStubModel, TEditorModel>(this IMapperConfigurationExpression config)
        {
            return new MapperSet<TEntity, TDetailModel, TStubModel, TEditorModel>
            {
                StubMapping = config.CreateMap<TEntity, TStubModel>(MemberList.Destination),
                DetailMapping = config.CreateMap<TEntity, TDetailModel>(MemberList.Destination),
                EditorMapping = config.CreateMap<TEntity, TEditorModel>(MemberList.Destination),
                EntityMapping = config.CreateMap<TEditorModel, TEntity>(MemberList.Source)
            };
        }

        private struct MapperSet<TEntity, TDetailModel, TStubModel, TEditorModel>
        {
            public IMappingExpression<TEntity, TStubModel> StubMapping { get; set; }
            public IMappingExpression<TEntity, TDetailModel> DetailMapping { get; set; }
            public IMappingExpression<TEntity, TEditorModel> EditorMapping { get; set; }
            public IMappingExpression<TEditorModel, TEntity> EntityMapping { get; set; }
        }
    }
}
