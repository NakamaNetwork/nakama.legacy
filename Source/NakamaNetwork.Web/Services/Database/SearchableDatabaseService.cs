using System.Linq;

namespace NakamaNetwork.Web.Services.Database
{
    public interface ISearchableDatabaseService<IEntity, ISearchModel> : IGenericDatabaseService<IEntity>
        where IEntity : class
    {
        IQueryable<IEntity> Search(ISearchModel model);
    }
}
