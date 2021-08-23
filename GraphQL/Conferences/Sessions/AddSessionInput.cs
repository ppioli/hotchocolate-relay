namespace ConferencePlanner.GraphQL.Conferences.Sessions
{
    using System.Collections.Generic;
    using Data;
    using HotChocolate.Types.Relay;

    public record AddSessionInput(
        string Title,
        string? Abstract,
        [ID(nameof(Speaker))] IReadOnlyList<int> SpeakerIds);
}