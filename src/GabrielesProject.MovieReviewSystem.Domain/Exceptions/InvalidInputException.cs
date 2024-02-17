using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace GabrielesProject.MovieReviewSystem.Domain.Exceptions;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class InvalidInputException : Exception
{
    public InvalidInputException()
    {
    }

    [Obsolete("Obsolete")]
    protected InvalidInputException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public InvalidInputException(string? message) : base(message)
    {
    }

    public InvalidInputException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}