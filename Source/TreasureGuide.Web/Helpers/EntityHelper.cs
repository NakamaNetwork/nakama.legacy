using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using TreasureGuide.Entities;

namespace TreasureGuide.Web.Helpers
{
    public static class EntityHelper
    {
        public static async Task SaveChangesSafe(this TreasureEntities entities)
        {
            try
            {
                await entities.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        var message = $"{validationErrors.Entry.Entity}:{validationError.ErrorMessage}";
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }
    }
}
