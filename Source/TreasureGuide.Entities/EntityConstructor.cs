using System;
using System.Data.Entity;

namespace TreasureGuide.Entities
{
    public partial class TreasureEntities : DbContext
    {
        private const string EntityFrameworkString =
                @"metadata=res://*/TreasureEntities.csdl|res://*/TreasureEntities.ssdl|res://*/TreasureEntities.msl;provider=System.Data.SqlClient;provider connection string=""{0}"""
            ;

        public TreasureEntities(string connection) : base(TransformToEntityFramework(connection))
        {
        }

        private static string TransformToEntityFramework(string connection)
        {
            if (connection.IndexOf("metadata") == 0)
            {
                return connection;
            }
            var result = String.Format(EntityFrameworkString, connection);
            return result;
        }
    }
}
