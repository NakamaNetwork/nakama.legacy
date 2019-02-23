using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreasureGuide.Common;
using NakamaNetwork.Entities;

namespace TreasureGuide.Web.Test.UnitTests.Configuration
{
    [TestClass]
    public class MapperTests
    {
        [TestMethod]
        public void MapperConfig_GeneratesValidMapping()
        {
            var mapper = MapperConfig.Create();
            Assert.IsNotNull(mapper);
        }

        [TestMethod]
        public void DataContext_IsValid()
        {
            var context = new TreasureEntities("Server=(localdb)\\tcdb;Database=tcdb;Trusted_Connection=True;MultipleActiveResultSets=true");
            var getStuff = context.UserProfiles.FirstOrDefault();
            Assert.IsNotNull(context);
        }
    }
}
