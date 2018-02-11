using System;
using System.Data.Entity;
using System.Threading.Tasks;
using TreasureGuide.Entities;

namespace TreasureGuide.Web.Services
{
    public interface IPreferenceService
    {
        Task SetPreference(string userId, UserPreferenceType type, string value);
        Task ClearPreference(string userId, UserPreferenceType type);
    }

    public class PreferenceService : IPreferenceService
    {
        private readonly TreasureEntities _entities;

        public PreferenceService(TreasureEntities entities)
        {
            _entities = entities;
        }

        public async Task SetPreference(string userId, UserPreferenceType type, string value)
        {
            var existing = await _entities.UserPreferences.SingleOrDefaultAsync(x => x.UserId == userId && x.Key == type);
            var existed = existing != null;
            if (String.IsNullOrEmpty(value))
            {
                if (existed)
                {
                    _entities.UserPreferences.Remove(existing);
                    await _entities.SaveChangesAsync();
                }
            }
            else
            {
                existing = existing ?? new UserPreference { UserId = userId, Key = type };
                existing.Value = value;
                if (!existed)
                {
                    _entities.UserPreferences.Add(existing);
                }
                await _entities.SaveChangesAsync();
            }
        }

        public async Task ClearPreference(string userId, UserPreferenceType type)
        {
            await SetPreference(userId, type, null);
        }
    }
}
