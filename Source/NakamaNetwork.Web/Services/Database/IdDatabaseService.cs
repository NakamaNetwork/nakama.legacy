using System;
using NakamaNetwork.Entities.Models;

namespace NakamaNetwork.Web.Services.Database
{
    public interface IIdDatabaseService<IIdType, IEntity> : IGenericDatabaseService<IEntity>
        where IIdType : IComparable<IIdType>
        where IEntity : class
    {
        IEntity Get(IIdType id);
        IEntity Delete(IIdType id);
    }

    public class IdDatabaseService<IIdType, IEntity> : GenericDatabaseService<IEntity>, IIdDatabaseService<IIdType, IEntity>
        where IIdType : IComparable<IIdType>
        where IEntity : class
    {
        public IdDatabaseService(NakamaNetworkContext context) : base(context)
        {
        }

        public IEntity Get(IIdType id)
        {
            return DbContext.Find<IEntity>(id);
        }

        public IEntity Delete(IIdType id)
        {
            var found = Get(id);
            if (found != null)
            {
                DbContext.Remove(found);
            }
            return found;
        }
    }
}