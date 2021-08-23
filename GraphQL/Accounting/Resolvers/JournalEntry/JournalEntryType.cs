namespace ConferencePlanner.GraphQL.Accounting.Resolvers.JournalEntry
{
    using System.Threading;
    using System.Threading.Tasks;
    using Data;
    using HotChocolate;
    using HotChocolate.Types;
    using Models;

    public class JournalEntryType : ObjectType<JournalEntry>
    {
        protected override void Configure(IObjectTypeDescriptor<JournalEntry> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(je => je.Id)
                .ResolveNode(
                    (ctx, id) => ctx.DataLoader<JournalEntryByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field("total")
                .UseDbContext<ApplicationDbContext>()
                .ResolveWith<JournalEntryResolver>(r => r.GetTotalAsync(default!, default!, default!));
        }

        private class JournalEntryResolver
        {
            public async Task<decimal> GetTotalAsync(
                [Parent] JournalEntry journalEntry,
                JournalEntryTotalDataLoader dataLoader,
                CancellationToken ct)
            {
                var total = await dataLoader.LoadAsync(journalEntry.Id, ct);
                return total;
            }
        }
    }
}