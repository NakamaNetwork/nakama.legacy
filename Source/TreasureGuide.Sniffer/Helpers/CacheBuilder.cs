using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TreasureGuide.Common;
using TreasureGuide.Entities;

namespace TreasureGuide.Sniffer.Helpers
{
    public static class CacheBuilder
    {
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Converters = new List<JsonConverter>
            {
                new StringTrimmingJsonConverter()
            }
        };

        public static async Task BuildCache<TEntity, TCache>(TreasureEntities entities, IMapper mapper, CacheItemType type, DateTimeOffset timestamp) where TEntity : class
        {
            Debug.WriteLine("Building cache for " + type);
            var data = entities.Set<TEntity>().AsQueryable();
            var cast = await data.ProjectTo<TCache>(mapper.ConfigurationProvider).ToListAsync();
            var json = JsonConvert.SerializeObject(cast, JsonSettings);

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
            existing.EditedDate = timestamp;
            await entities.SaveChangesAsync();
            Debug.WriteLine("Cache built.");
        }
    }
}
