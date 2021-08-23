namespace ConferencePlanner.GraphQL.Conferences.Sessions
{
    using Data;
    using HotChocolate.Types.Relay;

    public record RenameSessionInput(
        [ID(nameof(Session))] string SessionId,
        string Title);
}