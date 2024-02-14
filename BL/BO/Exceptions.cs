namespace BO;
[Serializable]
// Exception thrown when a business logic operation does not exist.
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}
[Serializable]
// Exception thrown when a business logic property is null.
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
}

// Exception thrown when a business logic operation already exists.
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException)
                : base(message, innerException) { }
}
// Exception thrown when a format is invalid.
public class FormatException : Exception
{
    public FormatException(string message) : base(message) { }
}
