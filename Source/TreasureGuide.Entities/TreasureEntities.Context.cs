﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TreasureGuide.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TreasureEntities : DbContext
    {
        public TreasureEntities()
            : base("name=TreasureEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TeamSocket> TeamSockets { get; set; }
        public virtual DbSet<Round> Rounds { get; set; }
        public virtual DbSet<RoundUnitBehavior> RoundUnitBehaviors { get; set; }
        public virtual DbSet<RoundUnit> RoundUnits { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<Ship> Ships { get; set; }
        public virtual DbSet<Stage> Stages { get; set; }
        public virtual DbSet<TeamUnit> TeamUnits { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<TeamVote> TeamVotes { get; set; }
    }
}
