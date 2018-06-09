using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Interfaces;
using TreasureGuide.Web.Models;
using TreasureGuide.Web.Models.ShipModels;
using TreasureGuide.Web.Models.StageModels;
using TreasureGuide.Web.Models.UnitModels;

namespace TreasureGuide.Web.Helpers
{
    public interface IJSonStaticService
    {
        Task Save();
    }

    public class JsonStaticService : IJSonStaticService
    {
        private readonly string _root;
        private readonly TreasureEntities _entities;
        private readonly IMapper _mapper;

        public JsonStaticService(IHostingEnvironment env, TreasureEntities entities, IMapper mapper)
        {
            _root = Path.Combine(env.ContentRootPath, "wwwroot", "json");
            _entities = entities;
            _mapper = mapper;
        }

        public async Task Save()
        {
            if (!Directory.Exists(_root))
            {
                Directory.CreateDirectory(_root);
            }
            await Save<int, Unit, UnitStubModel>();
            await Save<int, Stage, StageStubModel>();
            await Save<int, Ship, ShipStubModel>();
        }

        private async Task Save<TEntityKey, TEntity, TCacheModel>()
            where TEntity : class, IIdItem<TEntityKey>, IEditedDateItem
            where TCacheModel : IIdItem<TEntityKey>
        {
            var data = await GetData<TEntityKey, TEntity, TCacheModel>();
            if (data != null)
            {
                var json = JsonConvert.SerializeObject(data);
                var file = Path.Combine(_root, typeof(TEntity).Name + ".json");
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
                File.WriteAllText(file, json);
            }
        }

        private async Task<CacheResults<TEntityKey, TCacheModel>> GetData<TEntityKey, TEntity, TCacheModel>()
            where TEntity : class, IIdItem<TEntityKey>, IEditedDateItem
            where TCacheModel : IIdItem<TEntityKey>
        {
            var set = _entities.Set<TEntity>().AsQueryable();
            var results = await set.ProjectTo<TCacheModel>(_mapper.ConfigurationProvider).ToListAsync();
            if (results.Any())
            {
                var result = new CacheResults<TEntityKey, TCacheModel>
                {
                    Items = results,
                    Timestamp = DateTimeOffset.Now,
                    Deleted = Enumerable.Empty<int>(),
                    Reset = true
                };
                return result;
            }
            return null;
        }
    }
}
