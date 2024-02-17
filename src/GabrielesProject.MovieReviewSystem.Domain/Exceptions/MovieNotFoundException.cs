using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace GabrielesProject.MovieReviewSystem.Domain.Exceptions;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class MovieNotFoundException : Exception
{
    public MovieNotFoundException()
    {
    }

    [Obsolete("Obsolete")]
    protected MovieNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public MovieNotFoundException(string? message) : base(message)
    {
    }

    public MovieNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}