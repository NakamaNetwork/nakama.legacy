using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreasureGuide.Web.Configurations;

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
    }
}
