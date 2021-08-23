namespace ConferencePlanner.GraphQL.Accounting.Resolvers.Account
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Data;
    using HotChocolate;
    using HotChocolate.Types;
    using HotChocolate.Types.Relay;
    using Models;

    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class AddAccountMutation
    {
        [UseApplicationDbContext]
        public async Task<AddAccountPayload> AddAccount(AddAccountInput input,
            [ScopedService] ApplicationDbContext context,
            CancellationToken ct)
        {
            var account = new Account(input.Code, input.Name, input.Type)
            {
                ParentId = input.ParentId
            };

            context.Add(account);

            await context.SaveChangesAsync(ct);

            return new AddAccountPayload(account);
        }

        public record AddAccountInput(
            string Name,
            string Code,
            AccountClass Type,
            [ID(nameof(Account))] int? ParentId
        );
    }

    public class AddAccountPayload : Payload
    {
        public AddAccountPayload(Account account, IReadOnlyList<UserError>? errors = null) : base(errors)
        {
            Account = account;
        }

        public Account Account { get; }
    }
}