using Microsoft.EntityFrameworkCore;
using NakamaNetwork.Entities.Models;
using System;
using System.Linq;
using TreasureGuide.Common;
using Xunit;

namespace TreasureGuide.Test
{
    public class Tests
    {
        [Fact]
        public void MapperConfig_GeneratesValidMapping()
        {
            var mapper = MapperConfig.Create();
            Assert.NotNull(mapper);
        }

        [Fact]
        public void NakamaNetworkContext_IsValid()
        {
            var builder = new DbContextOptionsBuilder<NakamaNetworkContext>()
                .UseSqlServer("Server=(localdb)\\tcdb;Database=nndb;Trusted_Connection=True;MultipleActiveResultSets=true");
            var context = new NakamaNetworkContext(builder.Options);
            var getStuff = context.UserProfiles.FirstOrDefault();
            Assert.NotNull(context);
        }
    }
}