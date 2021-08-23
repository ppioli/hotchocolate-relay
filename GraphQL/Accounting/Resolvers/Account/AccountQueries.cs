namespace ConferencePlanner.GraphQL.Accounting.Resolvers.Account
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Data;
    using HotChocolate;
    using HotChocolate.Data;
    using HotChocolate.Types;
    using HotChocolate.Types.Relay;
    using Models;

    [ExtendObjectType(OperationTypeNames.Query)]
    public class AccountQueries
    {
        [UseApplicationDbContext]
        [UsePaging(MaxPageSize = 50)]
        [UseProjection]
        public IQueryable<Account> GetAccounts(
            [ScopedService] ApplicationDbContext context
        )
        {
            return context.Accounts;
        }

        [UseApplicationDbContext]
        public Task<Account> GetAccountById(
            [ID(nameof(Account))] int id,
            AccountByIdDataLoader dataLoader,
            CancellationToken ct
        )
        {
            return dataLoader.LoadAsync(id, ct);
        }

        [UseApplicationDbContext]
        public async Task<IEnumerable<Account>> GetAccountsById(
            [ID(nameof(Account))] int[] ids,
            AccountByIdDataLoader dataLoader,
            CancellationToken ct
        )
        {
            return await dataLoader.LoadAsync(ids, ct);
        }
    }
}