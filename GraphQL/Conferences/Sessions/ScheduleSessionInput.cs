namespace ConferencePlanner.GraphQL.Conferences.Sessions
{
    using System;
    using Data;
    using HotChocolate.Types.Relay;

    public record ScheduleSessionInput(
        [ID(nameof(Session))] int SessionId,
        [ID(nameof(Track))] int TrackId,
        DateTimeOffset StartTime,
        DateTimeOffset EndTime);
}