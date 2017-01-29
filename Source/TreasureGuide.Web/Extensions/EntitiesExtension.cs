using System;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;

namespace TreasureGuide.Web.Extensions
{
    public static class EntitiesExtension
    {
        public static async Task<TEntity> Import<TEntity, TSaved>(this DbSet<TEntity> entities, TSaved saved, IMapper mapper, Expression<Func<TEntity, bool>> findFunc)
            where TEntity : class
        {
            TEntity result;
            var existing = await entities.SingleOrDefaultAsync(findFunc);
            if (existing != null)
            {
                result = mapper.Map(saved, existing);
                entities.Attach(existing);
            }
            else
            {
                result = mapper.Map<TEntity>(saved);
                entities.Add(result);
            }
            return result;
        }
    }
}