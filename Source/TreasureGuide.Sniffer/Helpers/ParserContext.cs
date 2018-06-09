using System;
using System.Threading;
using System.Threading.Tasks;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Sniffer.Helpers
{
    public class ParserContext : TreasureEntities
    {
        private static readonly DateTimeOffset Zero = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);

        public ParserContext(string connection) : base(connection)
        {
        }

        public override Task<int> SaveChangesAsync()
        {
            UpdateEditables();
            return base.SaveChangesAsync();
        }

        public override int SaveChanges()
        {
            UpdateEditables();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            UpdateEditables();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateEditables()
        {
            foreach (var item in ChangeTracker.Entries<IEditedDateItem>())
            {
                item.Entity.EditedDate = Zero;
            }
        }
    }
}
