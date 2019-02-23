using NakamaNetwork.Entities.Models;
using System.Linq;
using System.Threading.Tasks;

namespace NakamaNetwork.Web.Services.Database
{
    public interface IGenericDatabaseService<IEntity>
        where IEntity : class
    {
        IQueryable<IEntity> GetAll();
        IEntity CreateOrUpdate(IEntity entity);
        IEntity Delete(IEntity entity);

        Task SaveChangesAsync();
    }

    public class GenericDatabaseService<IEntity> : IGenericDatabaseService<IEntity>
        where IEntity : class
    {
        protected readonly NakamaNetworkContext DbContext;

        public GenericDatabaseService(NakamaNetworkContext context)
        {
            DbContext = context;
        }

        public IQueryable<IEntity> GetAll()
        {
            return DbContext.Set<IEntity>().AsQueryable();
        }

        public IEntity CreateOrUpdate(IEntity entity)
        {
            DbContext.Attach(entity);
            return entity;
        }

        public IEntity Delete(IEntity entity)
        {
            DbContext.Remove(entity);
            return entity;
        }

        public async Task SaveChangesAsync()
        {
            DbContext.SaveChangesAsync();
        }
    }
}
