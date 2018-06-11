using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using TreasureGuide.Entities;

namespace TreasureGuide.Sniffer.Helpers
{
    public static class CacheBuilder
    {
        public static async Task BuildCache<TEntity, TCache>(TreasureEntities entities, IMapper mapper, CacheItemType type) where TEntity : class
        {
            Debug.WriteLine("Building cache for " + type);
            var data = entities.Set<TEntity>().AsQueryable();
            var cast = await data.ProjectTo<TCache>(mapper.ConfigurationProvider).ToListAsync();
            var json = JsonConvert.SerializeObject(cast);

            var existing = await entities.CacheSets.SingleOrDefaultAsync(x => x.Type == type);
            var existed = existing != null;
            if (!existed)
            {
                existing = new CacheSet
                {
                    Type = type
                };
                entities.CacheSets.Add(existing);
            }
            existing.JSON = json;
            await entities.SaveChangesAsync();
            Debug.WriteLine("Cache built.");
        }
    }
}
