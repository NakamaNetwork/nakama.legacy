using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TreasureGuide.Common.Constants;
using TreasureGuide.Common.Models.BoxModels;
using TreasureGuide.Common.Models.DonationModels;
using TreasureGuide.Common.Models.GCRModels;
using TreasureGuide.Common.Models.NotificationModels;
using TreasureGuide.Common.Models.ProfileModels;
using TreasureGuide.Common.Models.ShipModels;
using TreasureGuide.Common.Models.StageModels;
using TreasureGuide.Common.Models.TeamModels;
using TreasureGuide.Common.Models.UnitModels;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;

namespace TreasureGuide.Common
{
    public static class MapperConfig
    {
        private const string DefaultSubmitterName = "Monkey D. Luffy";

        public static IMapper Create()
        {
            var config = new MapperConfiguration(mapper =>
            {
                string userId = null;
                bool? canEdit = null;

                var gUnit = mapper.CreateMap<GCRUnit, GCRUnitEditModel>().ReverseMap();
                var gStage = mapper.CreateMap<GCRStage, GCRStageEditModel>().ReverseMap();

                var unit = mapper.CreateMap<Unit, UnitStubModel>();
                unit.ForMember(x => x.Aliases, o => o.MapFrom(y => y.UnitAliases.Select(z => z.Name)));
                unit.ForMember(x => x.EvolvesTo, o => o.MapFrom(y => y.EvolvesTo.Select(z => z.ToUnitId)));
                unit.ForMember(x => x.EvolvesFrom, o => o.MapFrom(y => y.EvolvesFrom.Select(z => z.FromUnitId)));

                var stage = mapper.CreateMap<Stage, StageStubModel>();
                stage.ForMember(x => x.Aliases, o => o.MapFrom(y => y.StageAliases.Select(z => z.Name)));

                var ship = mapper.CreateMap<Ship, ShipStubModel>();

                var teamUnit = mapper.CreateControllerMapping<TeamUnit, TeamUnitDetailModel, TeamUnitStubModel, TeamUnitEditorModel>();

                var teamGenericUnit = mapper.CreateControllerMapping<TeamGenericSlot, TeamGenericSlotDetailModel, TeamGenericSlotStubModel, TeamGenericSlotEditorModel>();

                var teamSocket = mapper.CreateControllerMapping<TeamSocket, TeamSocketDetailModel, TeamSocketStubModel, TeamSocketEditorModel>();

                var teamVideo = mapper.CreateMap<TeamVideo, TeamVideoModel>();
                teamVideo.ForMember(x => x.UserName, o => o.MapFrom(y => y.UserProfile.UserName));
                teamVideo.ForMember(x => x.UserUnitId, o => o.MapFrom(y => y.UserProfile.UnitId));
                teamVideo.ForMember(x => x.UserIsDonor, o => o.MapFrom(y => y.UserProfile.UserRoles.Any(z => z.Name == RoleConstants.Donor)));

                var team = mapper.CreateControllerMapping<Team, TeamDetailModel, TeamStubModel, TeamEditorModel>();
                team.StubMapping.ForMember(x => x.Global, o => o.MapFrom(y => y.TeamUnits.All(z => z.Unit.Flags.HasFlag(UnitFlag.Global))));

                team.StubMapping.ForMember(x => x.Global, o => o.MapFrom(y => y.TeamUnits.All(z => z.Unit.Flags.HasFlag(UnitFlag.Global))));
                team.StubMapping.ForMember(x => x.SubmittedByName, o => o.MapFrom(y => y.SubmittingUser != null ? y.SubmittingUser.UserName : DefaultSubmitterName));
                team.StubMapping.ForMember(x => x.SubmittedByUnitId, o => o.MapFrom(y => y.SubmittingUser != null ? y.SubmittingUser.UnitId : null));
                team.StubMapping.ForMember(x => x.SubmittedByIsDonor, o => o.MapFrom(y => y.SubmittingUser.UserRoles.Any(z => z.Name == RoleConstants.Donor)));
                team.StubMapping.ForMember(x => x.Score, o => o.MapFrom(y => y.TeamScore != null ? y.TeamScore.Value : 0));
                team.StubMapping.ForMember(x => x.Reported, o => o.MapFrom(y => y.TeamReports.Any(z => !z.AcknowledgedDate.HasValue)));
                team.StubMapping.ForMember(x => x.HasVideos, o => o.MapFrom(y => y.TeamVideos.Any(z => !z.Deleted)));
                team.StubMapping.ForMember(x => x.F2P, o => o.MapFrom(y => y.TeamUnits.All(z =>
                    z.Sub ||
                    z.Position == 0 ||
                    !EnumerableHelper.PayToPlay.Any(u => z.Unit.Flags.HasFlag(u))
                )));
                team.StubMapping.ForMember(x => x.F2P, o => o.MapFrom(y => y.TeamUnits.All(z =>
                    z.Sub ||
                    z.Position == 0 ||
                    !EnumerableHelper.PayToPlay.Any(u => z.Unit.Flags.HasFlag(u))
                )));
                team.StubMapping.ForMember(x => x.F2PC, o => o.MapFrom(y => y.TeamUnits.All(z =>
                    z.Sub ||
                    z.Position < 2 || !EnumerableHelper.PayToPlay.Any(u => z.Unit.Flags.HasFlag(u))
                )));
                team.StubMapping.ForMember(x => x.TeamUnits, o => o.MapFrom(y => y.TeamUnits.Where(z => !z.Sub)));
                team.StubMapping.ForMember(x => x.HasComments, o => o.MapFrom(y => y.TeamComments.Any(z => !z.Deleted)));

                team.DetailMapping.ForMember(x => x.Global, o => o.MapFrom(y => y.TeamUnits.All(z => z.Unit.Flags.HasFlag(UnitFlag.Global))));
                team.DetailMapping.ForMember(x => x.SubmittedByName, o => o.MapFrom(y => y.SubmittingUser != null ? y.SubmittingUser.UserName : DefaultSubmitterName));
                team.DetailMapping.ForMember(x => x.SubmittedByUnitId, o => o.MapFrom(y => y.SubmittingUser != null ? y.SubmittingUser.UnitId : null));
                team.DetailMapping.ForMember(x => x.SubmittedByIsDonor, o => o.MapFrom(y => y.SubmittingUser.UserRoles.Any(z => z.Name == RoleConstants.Donor)));

                team.DetailMapping.ForMember(x => x.Score, o => o.MapFrom(y => y.TeamScore != null ? y.TeamScore.Value : 0));
                team.DetailMapping.ForMember(x => x.Reported, o => o.MapFrom(y => y.TeamReports.Any(z => !z.AcknowledgedDate.HasValue)));
                team.DetailMapping.ForMember(x => x.CanEdit, o => o.Ignore()); // Handle this manually
                team.DetailMapping.ForMember(x => x.MyVote, o => o.Ignore()); // Handle this manually
                team.DetailMapping.ForMember(x => x.MyBookmark, o => o.Ignore()); // Handle this manually
                team.DetailMapping.ForMember(x => x.TeamSockets, o => o.MapFrom(y => y.TeamSockets.Where(z => z.Level > 0)));
                team.DetailMapping.ForMember(x => x.F2P, o => o.MapFrom(y => y.TeamUnits.All(z =>
                    z.Sub ||
                    z.Position == 0 ||
                    !EnumerableHelper.PayToPlay.Any(u => z.Unit.Flags.HasFlag(u))
                )));
                team.DetailMapping.ForMember(x => x.F2PC, o => o.MapFrom(y => y.TeamUnits.All(z =>
                    z.Sub ||
                    z.Position < 2 || !EnumerableHelper.PayToPlay.Any(u => z.Unit.Flags.HasFlag(u))
                )));
                team.DetailMapping.ForMember(x => x.Comments, o => o.MapFrom(y => y.TeamComments.Count(z => !z.Deleted)));

                var comments = mapper.CreateControllerMapping<TeamComment, TeamCommentDetailModel, TeamCommentStubModel, TeamCommentEditorModel>();
                comments.StubMapping.ForMember(x => x.Score, o => o.MapFrom(y => y.TeamCommentScore != null ? y.TeamCommentScore.Value : 0));
                comments.StubMapping.ForMember(x => x.SubmittedByName, o => o.MapFrom(y => y.SubmittedBy != null ? y.SubmittedBy.UserName : DefaultSubmitterName));
                comments.StubMapping.ForMember(x => x.SubmittedByUnitId, o => o.MapFrom(y => y.SubmittedBy != null ? y.SubmittedBy.UnitId : null));
                comments.StubMapping.ForMember(x => x.SubmittedByIsDonor, o => o.MapFrom(y => y.SubmittedBy.UserRoles.Any(z => z.Name == RoleConstants.Donor)));
                comments.StubMapping.ForMember(x => x.MyVote, o => o.MapFrom(y => y.TeamCommentVotes.Where(z => z.UserId == userId).Select(z => z.Value).DefaultIfEmpty((short)0).Sum(x => x)));
                comments.StubMapping.ForMember(x => x.CanEdit, o => o.MapFrom(y => (canEdit ?? false) || y.SubmittedById == userId));
                comments.StubMapping.ForMember(x => x.ChildCount, o => o.MapFrom(y => y.Children.Count(z => (canEdit ?? false) || !z.Deleted)));
                comments.StubMapping.ForMember(x => x.Children, o => o.MapFrom(y => y.Children.Where(z => (canEdit ?? false) || !z.Deleted).OrderBy(z => z.Id).Take(10)));

                comments.DetailMapping.ForMember(x => x.Score, o => o.MapFrom(y => y.TeamCommentScore != null ? y.TeamCommentScore.Value : 0));
                comments.DetailMapping.ForMember(x => x.SubmittedByName, o => o.MapFrom(y => y.SubmittedBy != null ? y.SubmittedBy.UserName : DefaultSubmitterName));
                comments.DetailMapping.ForMember(x => x.SubmittedByUnitId, o => o.MapFrom(y => y.SubmittedBy != null ? y.SubmittedBy.UnitId : null));
                comments.DetailMapping.ForMember(x => x.SubmittedByIsDonor, o => o.MapFrom(y => y.SubmittedBy.UserRoles.Any(z => z.Name == RoleConstants.Donor)));
                comments.DetailMapping.ForMember(x => x.MyVote, o => o.MapFrom(y => y.TeamCommentVotes.Where(z => z.UserId == userId).Select(z => z.Value).DefaultIfEmpty((short)0).Sum(x => x)));
                comments.DetailMapping.ForMember(x => x.CanEdit, o => o.MapFrom(y => (canEdit ?? false) || y.SubmittedById == userId));

                var wiki = mapper.CreateMap<Team, WikiSearchResultModel>();
                wiki.ForMember(x => x.SubmittedByName, o => o.MapFrom(y => y.SubmittingUser != null ? y.SubmittingUser.UserName : DefaultSubmitterName));
                wiki.ForMember(x => x.StageName, o => o.MapFrom(y => y.Stage != null ? y.Stage.Name : null));
                wiki.ForMember(x => x.InvasionName, o => o.MapFrom(y => y.Invasion != null ? y.Invasion.Name : null));
                wiki.ForMember(x => x.ShipName, o => o.MapFrom(y => y.Ship != null ? y.Ship.Name : null));
                wiki.ForMember(x => x.Score, o => o.MapFrom(y => y.TeamVotes.Select(z => z.Value).DefaultIfEmpty((short)0).Sum(x => x)));
                wiki.ForMember(x => x.TeamUnits, o => o.MapFrom(y => y.TeamUnits.Where(z => !z.Sub)));

                var wikiUnit = mapper.CreateMap<TeamUnit, WikiSearchUnitStubModel>();
                wikiUnit.ForMember(x => x.UnitName, o => o.MapFrom(y => y.Unit != null ? y.Unit.Name : null));

                var user = mapper.CreateControllerMapping<UserProfile, ProfileDetailModel, ProfileStubModel, ProfileEditorModel>();
                user.EntityMapping.ForMember(x => x.UserRoles, o => o.Ignore()); // Handle this manually.
                user.EntityMapping.ForMember(x => x.UserName, o => o.Ignore()); // Don't allow thise to be changed.

                user.StubMapping.ForMember(x => x.TeamCount, o => o.MapFrom(y => y.SubmittedTeams.Count(x => !x.Deleted && !x.Draft)));

                user.DetailMapping.ForMember(x => x.TeamCount, o => o.MapFrom(y => y.SubmittedTeams.Count(x => !x.Deleted && !x.Draft)));
                user.DetailMapping.ForMember(x => x.UserRoles, o => o.MapFrom(y => y.UserRoles.Select(z => z.Name)));
                user.DetailMapping.ForMember(x => x.CanEdit, o => o.Ignore());

                user.EditorMapping.ForMember(x => x.UserRoles, o => o.MapFrom(y => y.UserRoles.Select(z => z.Name)));

                var myUser = mapper.CreateMap<UserProfile, MyProfileModel>();
                myUser.ForMember(x => x.TeamCount, o => o.MapFrom(y => y.SubmittedTeams.Count(x => !x.Deleted && !x.Draft)));
                myUser.ForMember(x => x.UserRoles, o => o.MapFrom(y => y.UserRoles.Select(z => z.Name)));
                myUser.ForMember(x => x.CanEdit, o => o.MapFrom(y => true));
                myUser.ForMember(x => x.UserPreferences, o => o.MapFrom(y => y.UserPreferences.ToDictionary(z => z.Key, z => z.Value)));
                myUser.ForMember(x => x.BoxCount, o => o.MapFrom(y => y.Boxes.Count()));

                var report = mapper.CreateMap<TeamReport, TeamReportStubModel>();
                report.ForMember(x => x.Acknowledged, o => o.MapFrom(y => y.AcknowledgedDate.HasValue));

                var boxUnit = mapper.CreateControllerMapping<BoxUnit, BoxUnitDetailModel, BoxUnitStubModel, BoxUnitEditorModel>();

                var box = mapper.CreateControllerMapping<Box, BoxDetailModel, BoxStubModel, BoxEditorModel>();
                box.DetailMapping.ForMember(x => x.UserName, o => o.MapFrom(y => y.UserProfile.UserName));
                box.DetailMapping.ForMember(x => x.UserUnitId, o => o.MapFrom(y => y.UserProfile.UnitId));
                box.DetailMapping.ForMember(x => x.UserIsDonor, o => o.MapFrom(y => y.UserProfile.UserRoles.Any(z => z.Name == RoleConstants.Donor)));

                box.EntityMapping.ForMember(x => x.BoxUnits, o => o.Ignore());

                var donation = mapper.CreateControllerMapping<Donation, DonationDetailModel, DonationStubModel, DonationEditorModel>();
                donation.StubMapping.ForMember(x => x.UserName, o => o.MapFrom(y => y.UserProfile.UserName));
                donation.StubMapping.ForMember(x => x.UserUnitId, o => o.MapFrom(y => y.UserProfile.UnitId));
                donation.StubMapping.ForMember(x => x.UserIsDonor, o => o.MapFrom(y => y.UserProfile.UserRoles.Any(z => z.Name == RoleConstants.Donor)));

                donation.DetailMapping.ForMember(x => x.UserName, o => o.MapFrom(y => y.UserProfile.UserName));
                donation.DetailMapping.ForMember(x => x.UserIsDonor, o => o.MapFrom(y => y.UserProfile.UserRoles.Any(z => z.Name == RoleConstants.Donor)));
                donation.DetailMapping.ForMember(x => x.UserUnitId, o => o.MapFrom(y => y.UserProfile.UnitId));

                var notification = mapper.CreateMap<Notification, NotificationModel>();
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
