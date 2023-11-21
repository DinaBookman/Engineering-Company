namespace DO;


/// <summary>
/// Exception for a case where an object does not exist.
/// </summary>
[Serializable]
public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException(string? message) : base(message) { }
}
/// <summary>
/// Exception for a case where an object already exists.
/// </summary>
[Serializable]
public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}
/// <summary>
/// Exception for a case where object deletion is prohibited.
/// </summary>
[Serializable]
public class DalDeletionImpossible : Exception
{
    public DalDeletionImpossible(string? message) : base(message) { }
}
/// <summary>
/// Exception for xml files.
/// </summary>
public class DalXMLFileLoadCreateException : Exception
{
    public DalXMLFileLoadCreateException(string? message) : base(message) { }
}
/// <summary>
/// Exception for format.
/// </summary>
public class FormatException : Exception
{
    public FormatException(string? message) : base(message) { }
}

