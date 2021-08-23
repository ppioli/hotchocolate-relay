namespace ConferencePlanner.GraphQL.Accounting.Resolvers.JournalEntry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Data;
    using GreenDonut;
    using HotChocolate.Fetching;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class JournalEntryByIdDataLoader : BatchDataLoader<int, JournalEntry>
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public JournalEntryByIdDataLoader(IBatchScheduler batchScheduler,
            IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory ??
                                throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<IReadOnlyDictionary<int, JournalEntry>> LoadBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            await using ApplicationDbContext dbContext =
                _dbContextFactory.CreateDbContext();

            return await dbContext.JournalEntries
                .Where(s => keys.Contains(s.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken);
        }
    }
}