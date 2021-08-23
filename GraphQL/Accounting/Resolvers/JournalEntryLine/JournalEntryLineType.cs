namespace ConferencePlanner.GraphQL.Accounting.Resolvers.JournalEntryLine
{
    using HotChocolate.Types;
    using Models;

    public class JournalEntryLineType : ObjectType<JournalEntryLine>
    {
        protected override void Configure(IObjectTypeDescriptor<JournalEntryLine> descriptor)
        {
            descriptor.Field(f => f.Account)
                .IsProjected();
        }
    }
}