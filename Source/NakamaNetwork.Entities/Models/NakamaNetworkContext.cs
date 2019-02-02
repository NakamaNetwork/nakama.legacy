using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NakamaNetwork.Entities.Models
{
    public partial class NakamaNetworkContext : DbContext
    {
        public NakamaNetworkContext()
        {
        }

        public NakamaNetworkContext(DbContextOptions<NakamaNetworkContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<BoxUnits> BoxUnits { get; set; }
        public virtual DbSet<Boxes> Boxes { get; set; }
        public virtual DbSet<Donations> Donations { get; set; }
        public virtual DbSet<Notifications> Notifications { get; set; }
        public virtual DbSet<ScheduledEvents> ScheduledEvents { get; set; }
        public virtual DbSet<Ships> Ships { get; set; }
        public virtual DbSet<StageAliases> StageAliases { get; set; }
        public virtual DbSet<Stages> Stages { get; set; }
        public virtual DbSet<TeamBookmarks> TeamBookmarks { get; set; }
        public virtual DbSet<TeamCommentVotes> TeamCommentVotes { get; set; }
        public virtual DbSet<TeamComments> TeamComments { get; set; }
        public virtual DbSet<TeamCredits> TeamCredits { get; set; }
        public virtual DbSet<TeamGenericSlots> TeamGenericSlots { get; set; }
        public virtual DbSet<TeamReports> TeamReports { get; set; }
        public virtual DbSet<TeamSockets> TeamSockets { get; set; }
        public virtual DbSet<TeamUnits> TeamUnits { get; set; }
        public virtual DbSet<TeamVideos> TeamVideos { get; set; }
        public virtual DbSet<TeamVotes> TeamVotes { get; set; }
        public virtual DbSet<Teams> Teams { get; set; }
        public virtual DbSet<UnitAliases> UnitAliases { get; set; }
        public virtual DbSet<UnitEvolutions> UnitEvolutions { get; set; }
        public virtual DbSet<Units> Units { get; set; }
        public virtual DbSet<UserPreferences> UserPreferences { get; set; }
        public virtual DbSet<UserProfiles> UserProfiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<BoxUnits>(entity =>
            {
                entity.HasKey(e => new { e.BoxId, e.UnitId })
                    .HasName("PK.dbo_BoxUnits");

                entity.HasOne(d => d.Box)
                    .WithMany(p => p.BoxUnits)
                    .HasForeignKey(d => d.BoxId)
                    .HasConstraintName("FK.dbo_BoxUnits_dbo.Boxes");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.BoxUnits)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK.dbo_BoxUnits_dbo.Units");
            });

            modelBuilder.Entity<Boxes>(entity =>
            {
                entity.Property(e => e.FriendId).HasColumnType("numeric(9, 0)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Public).HasDefaultValueSql("((1))");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Boxes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK.dbo_Boxes_dbo.UserProfiles");
            });

            modelBuilder.Entity<Donations>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.Date).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.Message).HasMaxLength(500);

                entity.Property(e => e.PayerId).HasMaxLength(450);

                entity.Property(e => e.PaymentId).HasMaxLength(450);

                entity.Property(e => e.Public).HasDefaultValueSql("((0))");

                entity.Property(e => e.TokenId).HasMaxLength(450);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Donations)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK.dbo_Donations_dbo.UserProfiles");
            });

            modelBuilder.Entity<Notifications>(entity =>
            {
                entity.HasIndex(e => new { e.Id, e.UserId, e.EventType, e.EventId })
                    .HasName("IX_dbo.Notifications");

                entity.Property(e => e.ReceivedDate).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK.dbo_Notifications_dbo.UserProfiles");
            });

            modelBuilder.Entity<ScheduledEvents>(entity =>
            {
                entity.HasKey(e => new { e.StageId, e.Global, e.StartDate, e.EndDate })
                    .HasName("PK_dbo.ScheduledEvents");

                entity.HasOne(d => d.Stage)
                    .WithMany(p => p.ScheduledEvents)
                    .HasForeignKey(d => d.StageId)
                    .HasConstraintName("FK_dbo.ScheduledEvents_dbo.Stages");
            });

            modelBuilder.Entity<Ships>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Name).HasMaxLength(128);
            });

            modelBuilder.Entity<StageAliases>(entity =>
            {
                entity.HasKey(e => new { e.StageId, e.Name })
                    .HasName("PK_dbo.StageAliases");

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.Stage)
                    .WithMany(p => p.StageAliases)
                    .HasForeignKey(d => d.StageId)
                    .HasConstraintName("FK_dbo.StageAliasess_dbo.Stages");
            });

            modelBuilder.Entity<Stages>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Global)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Stages)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK.dbo_Stages_dbo.Units");
            });

            modelBuilder.Entity<TeamBookmarks>(entity =>
            {
                entity.HasKey(e => new { e.TeamId, e.UserId })
                    .HasName("PK_dbo.TeamBookmarks");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamBookmarks)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK_dbo.TeamBookmarks_dbo.Teams");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TeamBookmarks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.TeamBookmarks_dbo.UserProfiles");
            });

            modelBuilder.Entity<TeamCommentVotes>(entity =>
            {
                entity.HasKey(e => new { e.TeamCommentId, e.UserId })
                    .HasName("PK_dbo.TeamCommentVotes");

                entity.HasIndex(e => new { e.TeamCommentId, e.UserId })
                    .HasName("IX_dbo.TeamCommentVotes");

                entity.Property(e => e.SubmittedDate).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.HasOne(d => d.TeamComment)
                    .WithMany(p => p.TeamCommentVotes)
                    .HasForeignKey(d => d.TeamCommentId)
                    .HasConstraintName("FK_dbo.TeamCommentVotes_dbo.TeamComments");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TeamCommentVotes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.TeamCommentVotes_dbo.UserProfiles");
            });

            modelBuilder.Entity<TeamComments>(entity =>
            {
                entity.HasIndex(e => new { e.Id, e.TeamId, e.ParentId, e.Deleted, e.Reported, e.SubmittedById, e.EditedById })
                    .HasName("IX_dbo.TeamComments");

                entity.Property(e => e.EditedById).IsRequired();

                entity.Property(e => e.EditedDate).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.SubmittedById).IsRequired();

                entity.Property(e => e.SubmittedDate).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.HasOne(d => d.EditedBy)
                    .WithMany(p => p.TeamCommentsEditedBy)
                    .HasForeignKey(d => d.EditedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK.dbo_TeamComments_EditedById_dbo.UserProfiles");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_dbo.TeamComments_dbo.TeamComments");

                entity.HasOne(d => d.SubmittedBy)
                    .WithMany(p => p.TeamCommentsSubmittedBy)
                    .HasForeignKey(d => d.SubmittedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK.dbo_TeamComments_SubmittedById_dbo.UserProfiles");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamComments)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.TeamComments_dbo.Teams");
            });

            modelBuilder.Entity<TeamCredits>(entity =>
            {
                entity.HasKey(e => e.TeamId)
                    .HasName("PK_dbo.TeamCredits");

                entity.Property(e => e.TeamId).ValueGeneratedNever();

                entity.Property(e => e.Credit)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Team)
                    .WithOne(p => p.TeamCredits)
                    .HasForeignKey<TeamCredits>(d => d.TeamId)
                    .HasConstraintName("FK_dbo.TeamCredits_dbo.Teams");
            });

            modelBuilder.Entity<TeamGenericSlots>(entity =>
            {
                entity.HasKey(e => new { e.TeamId, e.Position, e.Type, e.Class, e.Role })
                    .HasName("PK_dbo.TeamGenericSlots");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamGenericSlots)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK_dbo.TeamGenericSlots_dbo.Teams");
            });

            modelBuilder.Entity<TeamReports>(entity =>
            {
                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SubmittedDate).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamReports)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK.dbo_TeamReports_dbo.Teams");
            });

            modelBuilder.Entity<TeamSockets>(entity =>
            {
                entity.HasKey(e => new { e.TeamId, e.SocketType })
                    .HasName("PK_dbo.TeamSockets");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamSockets)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK_dbo.TeamSockets_dbo.Teams");
            });

            modelBuilder.Entity<TeamUnits>(entity =>
            {
                entity.HasKey(e => new { e.TeamId, e.Position, e.UnitId })
                    .HasName("PK_dbo.TeamUnits");

                entity.HasIndex(e => new { e.TeamId, e.UnitId, e.Sub })
                    .HasName("IX_dbo.TeamUnits_Sub");

                entity.HasIndex(e => new { e.TeamId, e.UnitId, e.Position, e.Sub, e.Flags })
                    .HasName("IX_dbo.TeamUnits");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamUnits)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK_dbo.TeamUnits_dbo.Teams");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.TeamUnits)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_dbo.TeamUnits_dbo.Units");
            });

            modelBuilder.Entity<TeamVideos>(entity =>
            {
                entity.Property(e => e.SubmittedDate).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.VideoLink)
                    .IsRequired()
                    .HasMaxLength(12);

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamVideos)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK_dbo.TeamVideos_dbo.Teams");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TeamVideos)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.TeamVideos_dbo.UserProfiles");
            });

            modelBuilder.Entity<TeamVotes>(entity =>
            {
                entity.HasKey(e => new { e.TeamId, e.UserId })
                    .HasName("PK_dbo.TeamVotes");

                entity.Property(e => e.SubmittedDate).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamVotes)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK_dbo.TeamVotes_dbo.Teams");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TeamVotes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.TeamVotes_dbo.UserProfiles");
            });

            modelBuilder.Entity<Teams>(entity =>
            {
                entity.HasIndex(e => new { e.Id, e.Draft, e.Deleted, e.StageId, e.InvasionId })
                    .HasName("IX_dbo.Teams");

                entity.HasIndex(e => new { e.Name, e.StageId, e.InvasionId, e.ShipId, e.SubmittedById, e.SubmittedDate, e.Draft, e.Deleted })
                    .HasName("IX_dbo.Teams_Adv");

                entity.Property(e => e.Credits).HasMaxLength(2000);

                entity.Property(e => e.EditedById)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.EditedDate).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.SubmittedById).IsRequired();

                entity.Property(e => e.SubmittedDate).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.HasOne(d => d.EditedBy)
                    .WithMany(p => p.TeamsEditedBy)
                    .HasForeignKey(d => d.EditedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK.dbo_Teams_EditedById_dbo.UserProfiles");

                entity.HasOne(d => d.Invasion)
                    .WithMany(p => p.TeamsInvasion)
                    .HasForeignKey(d => d.InvasionId)
                    .HasConstraintName("FK.dbo_Teams_dbo.Stages_Invasions");

                entity.HasOne(d => d.Ship)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.ShipId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK.dbo_Teams_dbo.Ships");

                entity.HasOne(d => d.Stage)
                    .WithMany(p => p.TeamsStage)
                    .HasForeignKey(d => d.StageId)
                    .HasConstraintName("FK.dbo_Teams_dbo.Stages");

                entity.HasOne(d => d.SubmittedBy)
                    .WithMany(p => p.TeamsSubmittedBy)
                    .HasForeignKey(d => d.SubmittedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK.dbo_Teams_SubmittedById_dbo.UserProfiles");
            });

            modelBuilder.Entity<UnitAliases>(entity =>
            {
                entity.HasKey(e => new { e.UnitId, e.Name })
                    .HasName("PK_dbo.UnitAliases");

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.UnitAliases)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_dbo.UnitAliasess_dbo.Units");
            });

            modelBuilder.Entity<UnitEvolutions>(entity =>
            {
                entity.HasKey(e => new { e.FromUnitId, e.ToUnitId })
                    .HasName("PK_dbo.UnitEvolutions");

                entity.HasOne(d => d.FromUnit)
                    .WithMany(p => p.UnitEvolutionsFromUnit)
                    .HasForeignKey(d => d.FromUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.UnitEvolutions_dbo.FromUnitId");

                entity.HasOne(d => d.ToUnit)
                    .WithMany(p => p.UnitEvolutionsToUnit)
                    .HasForeignKey(d => d.ToUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.UnitEvolutions_dbo.ToUnitId");
            });

            modelBuilder.Entity<Units>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ExptoMax).HasColumnName("EXPtoMax");

                entity.Property(e => e.GrowthRate).HasColumnType("decimal(2, 1)");

                entity.Property(e => e.MaxAtk).HasColumnName("MaxATK");

                entity.Property(e => e.MaxHp).HasColumnName("MaxHP");

                entity.Property(e => e.MaxRcv).HasColumnName("MaxRCV");

                entity.Property(e => e.MinAtk).HasColumnName("MinATK");

                entity.Property(e => e.MinHp).HasColumnName("MinHP");

                entity.Property(e => e.MinRcv).HasColumnName("MinRCV");

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.Property(e => e.Stars).HasColumnType("decimal(2, 1)");
            });

            modelBuilder.Entity<UserPreferences>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.Key })
                    .HasName("PK_dbo.UserPreferences");

                entity.Property(e => e.Value).HasMaxLength(250);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPreferences)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK.dbo_UserPreferences_dbo.UserProfiles");
            });

            modelBuilder.Entity<UserProfiles>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FriendId).HasColumnType("numeric(9, 0)");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Website).HasMaxLength(200);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.UserProfiles)
                    .HasForeignKey<UserProfiles>(d => d.Id)
                    .HasConstraintName("FK.dbo_UserProfile_dbo.AspNetUsers");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.UserProfiles)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK.dbo_UserProfile_dbo.Units");
            });
        }
    }
}
