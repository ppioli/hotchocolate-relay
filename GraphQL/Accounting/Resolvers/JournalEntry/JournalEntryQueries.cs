namespace ConferencePlanner.GraphQL.Accounting.Resolvers.JournalEntry
{
    using System.Linq;
    using Data;
    using HotChocolate;
    using HotChocolate.Data;
    using HotChocolate.Types;
    using Microsoft.EntityFrameworkCore;
    using Models;

    [ExtendObjectType(OperationTypeNames.Query)]
    public class JournalEntryQueries
    {
        [UseApplicationDbContext]
        [UsePaging]
        [UseProjection]
        public IQueryable<JournalEntry> GetJournalEntries(
            [ScopedService] ApplicationDbContext context
        )
        {
            return context.JournalEntries
                .Include(je => je.JournalEntryLines)
                .ThenInclude(jel => jel.Account);
        }
    }
}