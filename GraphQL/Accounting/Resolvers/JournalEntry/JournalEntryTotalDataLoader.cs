namespace ConferencePlanner.GraphQL.Accounting.Resolvers.JournalEntry
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Data;
    using GreenDonut;
    using HotChocolate.Fetching;
    using Microsoft.EntityFrameworkCore;

    public class JournalEntryTotalDataLoader : BatchDataLoader<int, decimal>
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public JournalEntryTotalDataLoader(IDbContextFactory<ApplicationDbContext> dbContextFactory,
            IBatchScheduler batchScheduler) : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory ??
                                throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<IReadOnlyDictionary<int, decimal>> LoadBatchAsync(IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            await using ApplicationDbContext dbContext =
                _dbContextFactory.CreateDbContext();

            var list = string.Join(',', keys);
            return await dbContext.r
                .FromSqlRaw(
                    $"select \"JournalEntryId\", sum(\"Amount\") as \"Total\" from \"JournalEntryLines\" where \"JournalEntryId\" in ({list}) group by \"JournalEntryId\";")
                .ToDictionaryAsync(t => t.JournalEntryId,
                    s => s.Total,
                    cancellationToken);
        }
    }

    public record QueryResult(
        int JournalEntryId,
        decimal Total
    );
}