namespace ConferencePlanner.GraphQL.Attendees
{
    using Data;
    using HotChocolate.Types.Relay;

    public record CheckInAttendeeInput(
        [ID(nameof(Session))] int SessionId,
        [ID(nameof(Attendee))] int AttendeeId);
}