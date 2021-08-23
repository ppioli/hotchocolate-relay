namespace ConferencePlanner.GraphQL.Accounting.Resolvers.Account
{
    using HotChocolate.Types;
    using Models;

    public class AccountType : ObjectType<Account>
    {
        protected override void Configure(IObjectTypeDescriptor<Account> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<AccountByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));
        }
    }
}