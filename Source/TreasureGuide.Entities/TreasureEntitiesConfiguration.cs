using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace TreasureGuide.Entities
{
    public class TreasureEntitiesConfiguration : DbConfiguration
    {
        public TreasureEntitiesConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }

}
