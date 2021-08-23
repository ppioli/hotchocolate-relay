namespace ConferencePlanner.GraphQL.Common
{
    using System;

    public class ClientError : Exception
    {
        public ClientError(string code, string displayMessage, string? message = null) : this(code, displayMessage,
            message, null)
        {
        }

        public ClientError(string code, string displayMessage, string? message, Exception? innerException) :
            base(message ?? displayMessage, innerException)
        {
            DisplayMessage = displayMessage;
            Code = code;
        }

        public string DisplayMessage { get; }
        public string Code { get; }
    }
}